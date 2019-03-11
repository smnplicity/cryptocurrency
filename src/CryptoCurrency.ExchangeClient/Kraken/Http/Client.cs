using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using CryptoCurrency.Core;
using CryptoCurrency.Core.Currency;
using CryptoCurrency.Core.Exchange;
using CryptoCurrency.Core.Exchange.Model;
using CryptoCurrency.Core.Extensions;
using CryptoCurrency.Core.Market;
using CryptoCurrency.Core.OrderSide;
using CryptoCurrency.Core.OrderType;
using CryptoCurrency.Core.Symbol;
using CryptoCurrency.Core.RateLimiter;

using CryptoCurrency.ExchangeClient.Kraken.Model;

namespace CryptoCurrency.ExchangeClient.Kraken.Http
{
    public class Client : IExchangeHttpClient
    {
        private Kraken Exchange { get; set; }
        private ICurrencyFactory CurrencyFactory { get; set; }
        private ISymbolFactory SymbolFactory { get; set; }

        public IRateLimiter RateLimiter { get; set; }

        public Client(Kraken exchange, ICurrencyFactory currencyFactory, ISymbolFactory symbolFactory)
        {
            Exchange = exchange;
            CurrencyFactory = currencyFactory;
            SymbolFactory = symbolFactory;

            RateLimiter = new BinanceRateLimiter();
        }

        public string ApiUrl => "https://api.kraken.com";

        public bool MultiTickSupported => true;

        public string InitialTradeFilter => "0";

        public async Task<WrappedResponse<CancelOrder>> CancelOrder(ISymbol symbol, string[] orderIds)
        {
            var relativeUrl = "CancelOrder";

            var nvc = new NameValueCollection();
            nvc.Add("txid", orderIds.First());

            return await InternalRequest<KrakenCancelOrderResult, CancelOrder>(true, relativeUrl, HttpMethod.Post, nvc);
        }

        public async Task<WrappedResponse<CreateOrder>> CreateOrder(ISymbol symbol, OrderTypeEnum orderType, OrderSideEnum orderSide, decimal price, decimal volume)
        {
            Exchange.EnsureSymbol(symbol);

            var relativeUrl = "AddOrder";

            var nvc = new NameValueCollection();
            nvc.Add("pair", $"{Exchange.GetCurrencyCode(symbol.BaseCurrencyCode)}{Exchange.GetCurrencyCode(symbol.QuoteCurrencyCode)}");
            nvc.Add("type", orderSide == OrderSideEnum.Buy ? "buy" : "sell");
            nvc.Add("ordertype", orderType == OrderTypeEnum.Market ? "market" : "limit");
            if (orderType == OrderTypeEnum.Limit)
                nvc.Add("price", price.ToString());
            nvc.Add("volume", volume.ToString());

            return await InternalRequest<KrakenAddOrderResult, CreateOrder>(true, relativeUrl, HttpMethod.Post, nvc);
        }

        public async Task<WrappedResponse<ICollection<AccountBalance>>> GetBalance()
        {
            var relativeUrl = "Balance";

            return await InternalRequest<KrakenAccount, ICollection<AccountBalance>>(true, relativeUrl, HttpMethod.Post, null);
        }

        public async Task<WrappedResponse<ICollection<OrderItem>>> GetOpenOrders(ISymbol symbol, int pageNumber, int pageSize)
        {
            Exchange.EnsureSymbol(symbol);

            var relativeUrl = "OpenOrders";

            var nvc = new NameValueCollection();
            nvc.Add("pair", $"{Exchange.GetCurrencyCode(symbol.BaseCurrencyCode)}{Exchange.GetCurrencyCode(symbol.QuoteCurrencyCode)}");

            return await InternalRequest<KrakenOpenOrders, ICollection<OrderItem>>(true, relativeUrl, HttpMethod.Post, nvc);
        }

        public async Task<WrappedResponse<OrderBook>> GetOrderBook(ISymbol symbol, int buyCount, int sellCount)
        {
            Exchange.EnsureSymbol(symbol);

            var relativeUrl = "Depth";

            var nvc = new NameValueCollection();
            nvc.Add("pair", $"{Exchange.GetCurrencyCode(symbol.BaseCurrencyCode)}{Exchange.GetCurrencyCode(symbol.QuoteCurrencyCode)}");

            return await InternalRequest<KrakenOrderBook, OrderBook>(false, relativeUrl, HttpMethod.Get, nvc);
        }

        public async Task<WrappedResponse<MarketTick>> GetTick(ISymbol symbol)
        {
            var relativeUrl = "Ticker";

            var nvc = new NameValueCollection();
            nvc.Add("pair", $"{Exchange.GetCurrencyCode(symbol.BaseCurrencyCode)}{Exchange.GetCurrencyCode(symbol.QuoteCurrencyCode)}");

            return await InternalRequest<KrakenTick, MarketTick>(false, relativeUrl, HttpMethod.Get, nvc);
        }

        public async Task<WrappedResponse<TradeFee>> GetTradeFee(OrderSideEnum orderSide, ISymbol symbol)
        {
            var relativeUrl = "TradeVolume";

            var nvc = new NameValueCollection();
            nvc.Add("pair", $"{Exchange.GetCurrencyCode(symbol.BaseCurrencyCode)}{Exchange.GetCurrencyCode(symbol.QuoteCurrencyCode)}");
            nvc.Add("fee-info", "true");

            return await InternalRequest<KrakenTradeVolume, TradeFee>(true, relativeUrl, HttpMethod.Post, nvc);
        }

        public Task<WrappedResponse<WithdrawCrypto>> WithdrawCrypto(CurrencyCodeEnum cryptoCurrencyCode, decimal withdrawalFee, decimal volume, string address)
        {
            throw new NotImplementedException();
        }

        public void SetApiAccess(string privateKey, string publicKey, string passphrase)
        {
            PrivateKey = privateKey;
            PublicKey = publicKey;
        }

        public async Task<WrappedResponse<ICollection<TradeItem>>> GetTradeHistory(ISymbol symbol, int pageNumber, int pageSize, string fromTradeId)
        {
            var relativeUrl = "TradesHistory";

            var nvc = new NameValueCollection();
            nvc.Add("pair", $"{Exchange.GetCurrencyCode(symbol.BaseCurrencyCode)}{Exchange.GetCurrencyCode(symbol.QuoteCurrencyCode)}");

            return await InternalRequest<KrakenTradeHistory, ICollection<TradeItem>>(true, relativeUrl, HttpMethod.Post, nvc);
        }

        public Task<WrappedResponse<ICollection<Deposit>>> GetDeposits(CurrencyCodeEnum currencyCode, int limit)
        {
            throw new NotImplementedException();
        }

        public Task<WrappedResponse<Deposit>> GetDeposit(CurrencyCodeEnum currencyCode, string transactionId)
        {
            throw new NotImplementedException();
        }

        public async Task<WrappedResponse<ICollection<MarketTick>>> GetTicks(ICollection<ISymbol> symbols)
        {
            var relativeUrl = "Ticker";

            var nvc = new NameValueCollection();
            nvc.Add("pair", string.Join(",", symbols.Select(p => $"{Exchange.GetCurrencyCode(p.BaseCurrencyCode)}{Exchange.GetCurrencyCode(p.QuoteCurrencyCode)}")));

            return await InternalRequest<KrakenTicks, ICollection<MarketTick>>(false, relativeUrl, HttpMethod.Get, nvc);
        }

        public async Task<WrappedResponse<TradeResult>> GetTrades(ISymbol symbol, int limit, string filter)
        {
            var relativeUrl = "Trades";

            var nvc = new NameValueCollection();
            nvc.Add("pair", $"{Exchange.GetCurrencyCode(symbol.BaseCurrencyCode)}{Exchange.GetCurrencyCode(symbol.QuoteCurrencyCode)}");

            if (!string.IsNullOrEmpty(filter))
                nvc.Add("since", filter);

            return await InternalRequest<KrakenTrade<string, List<List<object>>>, TradeResult>(false, relativeUrl, HttpMethod.Get, nvc);
        }


        public Task<WrappedResponse<ICollection<ExchangeStats>>> GetStats(ISymbol symbol, ExchangeStatsKeyEnum statsKey)
        {
            throw new NotImplementedException();
        }

        #region Private Functionality
        private string PublicKey { get; set; }

        private string PrivateKey { get; set; }

        private string ApiVersion { get { return "0"; } }

        private string ToQueryString(NameValueCollection nvc)
        {
            if (nvc == null)
                return null;

            var array = (from key in nvc.AllKeys.OrderBy(k => k)
                         from value in nvc.GetValues(key)
                         select string.Format("{0}={1}", key, value))
                .ToArray();
            return string.Join("&", array);
        }
        
        private async Task<WrappedResponse<T2>> InternalRequest<T, T2>(bool authRequired, string relativeUrl, HttpMethod method, NameValueCollection extraParams)
        {
            await RateLimiter.Wait();

            relativeUrl = $"/{ApiVersion}/{(authRequired ? "private" : "public")}/{relativeUrl}";

            var url = this.GetFullUrl(relativeUrl);

            string queryString = null;

            var headers = new NameValueCollection();

            if (authRequired)
            {
                var nonce = Epoch.Now.Timestamp;

                if (extraParams == null)
                    extraParams = new NameValueCollection();

                extraParams.Add("nonce", nonce.ToString());

                queryString = ToQueryString(extraParams);

                headers.Add("API-Key", PublicKey);

                var encoding = Encoding.UTF8;

                var prehash = $"{nonce}{queryString}";
                var messageBytes = encoding.GetBytes(prehash);

                byte[] hash256 = null;

                using (var hash = SHA256.Create())
                {
                    hash256 = hash.ComputeHash(messageBytes);
                }

                var privateKeyBytes = Convert.FromBase64String(PrivateKey);

                var pathBytes = encoding.GetBytes(relativeUrl);

                var mergedMessageBytes = new byte[hash256.Count() + pathBytes.Count()];
                Buffer.BlockCopy(pathBytes, 0, mergedMessageBytes, 0, pathBytes.Length);
                Buffer.BlockCopy(hash256, 0, mergedMessageBytes, pathBytes.Length, hash256.Length);

                using (var hmacsha512 = new HMACSHA512(privateKeyBytes))
                {
                    var signatureBytes = hmacsha512.ComputeHash(mergedMessageBytes);

                    headers.Add("API-Sign", Convert.ToBase64String(signatureBytes));
                }
            }
            else
                queryString = ToQueryString(extraParams);

            if (queryString != null && method == HttpMethod.Get)
                url += "?" + queryString;

            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(url),
                    Method = method
                };

                if (method == HttpMethod.Post && queryString != null)
                    request.Content = new StringContent(queryString, Encoding.UTF8, "application/x-www-form-urlencoded");

                request.Headers.Add("User-Agent", "cryptocurrency Client");

                if (headers != null)
                {
                    request.Headers.AcceptCharset.Add(new StringWithQualityHeaderValue("UTF-8"));

                    foreach (var header in headers.AllKeys)
                    {
                        request.Headers.Add(header, headers[header]);
                    }
                }

                try
                {
                    using (var response = await client.SendAsync(request))
                    {
                        var json = await response.Content.ReadAsStringAsync();

                        try
                        {
                            response.EnsureSuccessStatusCode();

                            var krakenResponse = JsonConvert.DeserializeObject<KrakenWrappedResponse<T>>(json);

                            json = null;

                            response.Content.Dispose();

                            if (krakenResponse.Error != null && krakenResponse.Error.Count > 0)
                            {
                                return new WrappedResponse<T2>
                                {
                                    StatusCode = WrappedResponseStatusCode.ApiError,
                                    ErrorMessage = string.Join(". ", krakenResponse.Error)
                                };
                            }
                            else
                            {
                                return new WrappedResponse<T2>
                                {
                                    StatusCode = WrappedResponseStatusCode.Ok,
                                    Data = Exchange.ChangeType<T, T2>(CurrencyFactory, SymbolFactory, extraParams, krakenResponse.Result)
                                };
                            }
                        }
                        catch (HttpRequestException ex)
                        {
                            return new WrappedResponse<T2>
                            {
                                StatusCode = WrappedResponseStatusCode.FatalError,
                                ErrorCode = null,
                                ErrorMessage = ex.Message
                            };
                        }
                    }
                }
                catch (Exception e)
                {
                    return new WrappedResponse<T2>
                    {
                        StatusCode = WrappedResponseStatusCode.FatalError,
                        ErrorCode = null,
                        ErrorMessage = e.Message
                    };
                }
            }
        }       
        #endregion
    }
}
