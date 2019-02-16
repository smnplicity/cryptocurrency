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
using CryptoCurrency.Core.RateLimiter;
using CryptoCurrency.Core.Symbol;

using CryptoCurrency.ExchangeClient.Binance.Model;

namespace CryptoCurrency.ExchangeClient.Binance.Http
{
    public class Client : IExchangeHttpClient
    {
        private Binance Exchange { get; set; }

        private ISymbolFactory SymbolFactory { get; set; }

        public IRateLimiter RateLimiter { get; set; }

        public Client(Binance exchange, ISymbolFactory symbolFactory)
        {
            Exchange = exchange;
            SymbolFactory = symbolFactory;

            RateLimiter = new CoinbaseProRateLimiter();
        }

        public string ApiUrl => "https://api.binance.com/api/";

        public bool MultiTickSupported => true;

        public string InitialTradeFilter => "0";

        public void SetApiAccess(string privateKey, string publicKey, string passphrase)
        {
            PrivateKey = privateKey;
            PublicKey = publicKey;
        }

        public Task<WrappedResponse<MarketTick>> GetTick(ISymbol symbol)
        {
            throw new NotImplementedException();
        }

        public async Task<WrappedResponse<ICollection<MarketTick>>> GetTicks(ICollection<ISymbol> symbols)
        {
            // Get last price
            var relativeUrl = "v3/ticker/price";

            var priceTicks = await InternalRequest<ICollection<BinancePriceTicker>, ICollection<MarketTick>>(false, relativeUrl, HttpMethod.Get, null);

            if (priceTicks.StatusCode != WrappedResponseStatusCode.Ok)
                return priceTicks;

            // Get order book prices
            relativeUrl = "v3/ticker/bookTicker";

            var bookTicks = await InternalRequest<ICollection<BinanceOrderBookTicker>, ICollection<MarketTick>>(false, relativeUrl, HttpMethod.Get, null);

            if (bookTicks.StatusCode != WrappedResponseStatusCode.Ok)
                return bookTicks;

            var ticks = new List<MarketTick>();

            foreach (var priceTick in priceTicks.Data.Where(x => symbols.Any(x2 => x2.Code == x.SymbolCode)))
            {
                var bookTick = bookTicks.Data.Where(x => x.SymbolCode == priceTick.SymbolCode).FirstOrDefault();

                ticks.Add(new MarketTick
                {
                    Epoch = priceTick.Epoch,
                    SymbolCode = priceTick.SymbolCode,
                    BuyPrice = bookTick.BuyPrice,
                    SellPrice = bookTick.SellPrice,
                    LastPrice = priceTick.LastPrice
                });
            }

            return new WrappedResponse<ICollection<MarketTick>>
            {
                StatusCode = WrappedResponseStatusCode.Ok,
                Data = ticks
            };
        }

        public Task<WrappedResponse<ICollection<AccountBalance>>> GetBalance()
        {
            throw new NotImplementedException();
        }

        public Task<WrappedResponse<CreateOrder>> CreateOrder(ISymbol symbol, OrderTypeEnum orderType, OrderSideEnum orderSide, double price, double volume)
        {
            throw new NotImplementedException();
        }

        public async Task<WrappedResponse<TradeFee>> GetTradeFee(OrderSideEnum orderSide, ISymbol symbol)
        {
            return await Task.Run(() =>
            {
                return new WrappedResponse<TradeFee>()
                {
                    StatusCode = WrappedResponseStatusCode.Ok,
                    Data = new TradeFee
                    {
                        CurrencyCode = orderSide == OrderSideEnum.Buy ? symbol.BaseCurrencyCode : symbol.QuoteCurrencyCode,
                        Maker = 0.001,
                        Taker = 0.001
                    }
                };
            });
        }

        public async Task<WrappedResponse<ICollection<TradeItem>>> GetTradeHistory(ISymbol symbol, int pageNumber, int pageSize, string fromTradeId)
        {
            var relativeUrl = "v3/myTrades";

            var query = new NameValueCollection();
            query.Add("symbol", $"{Exchange.GetCurrencyCode(symbol.BaseCurrencyCode)}{Exchange.GetCurrencyCode(symbol.QuoteCurrencyCode)}");

            if (!string.IsNullOrEmpty(fromTradeId))
                query.Add("fromId", fromTradeId);

            query.Add("limit", pageSize.ToString());

            return await InternalRequest<ICollection<BinanceTradeItem>, ICollection<TradeItem>>(true, relativeUrl, HttpMethod.Get, query);
        }

        public Task<WrappedResponse<ICollection<OrderItem>>> GetOpenOrders(ISymbol symbol, int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<WrappedResponse<CancelOrder>> CancelOrder(string[] orderIds)
        {
            throw new NotImplementedException();
        }

        public Task<WrappedResponse<WithdrawCrypto>> WithdrawCrypto(CurrencyCodeEnum cryptoCurrencyCode, double withdrawalFee, double volume, string address)
        {
            throw new NotImplementedException();
        }

        public Task<WrappedResponse<ICollection<Deposit>>> GetDeposits(CurrencyCodeEnum currencyCode, int limit)
        {
            throw new NotImplementedException();
        }

        public Task<WrappedResponse<Deposit>> GetDeposit(CurrencyCodeEnum currencyCode, string transactionId)
        {
            throw new NotImplementedException();
        }

        public async Task<WrappedResponse<TradeResult>> GetTrades(ISymbol symbol, int limit, string filter)
        {
            var relativeUrl = "v1/aggTrades";

            var query = new NameValueCollection();
            query.Add("symbol", $"{Exchange.GetCurrencyCode(symbol.BaseCurrencyCode)}{Exchange.GetCurrencyCode(symbol.QuoteCurrencyCode)}");

            if (filter != null)
            {
                var filterParts = filter.Split('&');

                if (filterParts.Length == 1)
                    query.Add("fromId", filterParts[0]);
                else
                {
                    query.Add("startTime", filterParts[0]);
                    query.Add("endTime", filterParts[1]);
                }
            }

            if (limit != 0)
                query.Add("limit", limit.ToString());

            return await InternalRequest<ICollection<BinanceAggTrade>, TradeResult>(false, relativeUrl, HttpMethod.Get, query);
        }


        public Task<WrappedResponse<ICollection<ExchangeStats>>> GetStats(ISymbol symbol, ExchangeStatsKeyEnum statsKey)
        {
            throw new NotImplementedException();
        }

        #region Private Functionality
        private string PublicKey { get; set; }

        private string PrivateKey { get; set; }

        private string GenerateSignature(string message)
        {
            var key = Encoding.UTF8.GetBytes(PrivateKey);

            using (var hmac = new HMACSHA256(key))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));

                return BitConverter.ToString(hash).Replace("-", "");
            }
        }

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

            var url = this.GetFullUrl(relativeUrl);

            var headers = new NameValueCollection();

            if (authRequired)
            {
                extraParams.Add("timestamp", Epoch.Now.TimestampMilliseconds.ToString());
                extraParams.Add("recvWindow", "1000000");

                var signature = GenerateSignature(ToQueryString(extraParams));

                extraParams.Add("signature", signature);

                headers.Add("X-MBX-APIKEY", PublicKey);
            }
            
            var queryString = ToQueryString(extraParams);

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

                            var obj = JsonConvert.DeserializeObject<T>(json);

                            json = null;

                            response.Content.Dispose();

                            return new WrappedResponse<T2>
                            {
                                StatusCode = WrappedResponseStatusCode.Ok,
                                Data = await Exchange.ChangeType<T, T2>(SymbolFactory, extraParams, obj)
                            };
                        }
                        catch (HttpRequestException)
                        {
                            var error = JsonConvert.DeserializeObject<BinanceApiError>(json);

                            return new WrappedResponse<T2>
                            {
                                StatusCode = WrappedResponseStatusCode.ApiError,
                                ErrorCode = error.Code.ToString(),
                                ErrorMessage = error.Message
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
