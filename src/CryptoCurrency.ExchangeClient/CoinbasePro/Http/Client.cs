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
using CryptoCurrency.Core.OrderType;
using CryptoCurrency.Core.OrderSide;
using CryptoCurrency.Core.RateLimiter;
using CryptoCurrency.Core.Symbol;

using CryptoCurrency.ExchangeClient.CoinbasePro.Model;

namespace CryptoCurrency.ExchangeClient.CoinbasePro.Http
{
    public class Client : IExchangeHttpClient
    {
        private CoinbasePro Exchange { get; set; }

        private ICurrencyFactory CurrencyFactory { get; set; }

        private ISymbolFactory SymbolFactory { get; set; }

        public IRateLimiter RateLimiter { get; set; }

        public Client(CoinbasePro exchange, ICurrencyFactory currencyFactory, ISymbolFactory symbolFactory)
        {
            Exchange = exchange;
            CurrencyFactory = currencyFactory;
            SymbolFactory = symbolFactory;

            RateLimiter = new CoinbaseProRateLimiter();
        }

        public string ApiUrl => "https://api.pro.coinbase.com";

        public bool MultiTickSupported => false;

        public string InitialTradeFilter => "100";
        
        public async Task<WrappedResponse<CancelOrder>> CancelOrder(ISymbol symbol, string[] orderIds)
        {
            var relativeUrl = $"/orders/{orderIds.First()}";

            return await InternalRequest<CoinbaseProCancelOrder, CancelOrder>(true, relativeUrl, HttpMethod.Delete, null);
        }

        public async Task<WrappedResponse<CreateOrder>> CreateOrder(ISymbol symbol, OrderTypeEnum orderType, OrderSideEnum orderSide, double price, double volume)
        {
            var relativeUrl = "/orders";

            var request = new CoinbaseProCreateOrderRequest
            {
                Size = volume,
                Price = price,
                Side = orderSide == OrderSideEnum.Buy ? "buy" : "sell",
                ProductId = Exchange.EncodeProductId(symbol),
                Type = orderType == OrderTypeEnum.Market ? "market" : "limit"
            };

            return await InternalRequest<CoinbaseProCreateOrderResponse, CreateOrder>(true, relativeUrl, HttpMethod.Post, request);
        }

        public async Task<WrappedResponse<ICollection<AccountBalance>>> GetBalance()
        {
            var relativeUrl = "/accounts";

            return await InternalRequest<ICollection<CoinbaseProAccount>, ICollection<AccountBalance>>(true, relativeUrl, HttpMethod.Get, null);
        }

        public Task<WrappedResponse<Deposit>> GetDeposit(CurrencyCodeEnum currencyCode, string transactionId)
        {
            throw new NotImplementedException();
        }

        public Task<WrappedResponse<ICollection<Deposit>>> GetDeposits(CurrencyCodeEnum currencyCode, int limit)
        {
            throw new NotImplementedException();
        }

        public async Task<WrappedResponse<ICollection<OrderItem>>> GetOpenOrders(ISymbol symbol, int pageNumber, int pageSize)
        {
            var relativeUrl = $"/orders?status=open&status=pending&status=active&product_id={Exchange.EncodeProductId(symbol)}";

            return await InternalRequest<ICollection<CoinbaseProOrder>, ICollection<OrderItem>>(true, relativeUrl, HttpMethod.Get, null);
        }

        public async Task<WrappedResponse<OrderBook>> GetOrderBook(ISymbol symbol, int buyCount, int sellCount)
        {
            var relativeUrl = $"/products/{Exchange.EncodeProductId(symbol)}/book?level=2";

            var nvc = new NameValueCollection();
            nvc.Add("product_id", Exchange.EncodeProductId(symbol));

            return await InternalRequest<CoinbaseProOrderBook, OrderBook>(false, relativeUrl, HttpMethod.Get, nvc);
        }

        public async Task<WrappedResponse<MarketTick>> GetTick(ISymbol symbol)
        {
            var relativeUrl = $"/products/{Exchange.EncodeProductId(symbol)}/ticker";

            var nvc = new NameValueCollection();

            nvc.Add("product_id", Exchange.EncodeProductId(symbol));

            return await InternalRequest<CoinbaseProTick, MarketTick>(false, relativeUrl, HttpMethod.Get, nvc);
        }

        public Task<WrappedResponse<ICollection<MarketTick>>> GetTicks(ICollection<ISymbol> symbols)
        {
            throw new NotImplementedException();
        }

        public async Task<WrappedResponse<TradeFee>> GetTradeFee(OrderSideEnum orderSide, ISymbol symbol)
        {
            return await Task.Run(() =>
            {
                return new WrappedResponse<TradeFee>
                {
                    StatusCode = WrappedResponseStatusCode.Ok,
                    Data = new TradeFee
                    {
                        CurrencyCode = symbol.QuoteCurrencyCode,
                        Taker = 0.003,
                        Maker = 0.0
                    }
                };
            });
        }

        public async Task<WrappedResponse<ICollection<TradeItem>>> GetTradeHistory(ISymbol symbol, int pageNumber, int pageSize, string fromTradeId)
        {
            var relativeUrl = $"/fills?product_id={Exchange.EncodeProductId(symbol)}";

            return await InternalRequest<ICollection<CoinbaseProFill>, ICollection<TradeItem>>(true, relativeUrl, HttpMethod.Get, null);
        }

        public void SetApiAccess(string privateKey, string publicKey, string passphrase)
        {
            PrivateKey = privateKey;
            PublicKey = publicKey;
            Passphrase = passphrase;
        }

        public Task<WrappedResponse<WithdrawCrypto>> WithdrawCrypto(CurrencyCodeEnum cryptoCurrencyCode, double withdrawalFee, double volume, string address)
        {
            throw new NotImplementedException();
        }

        public async Task<WrappedResponse<TradeResult>> GetTrades(ISymbol symbol, int limit, string filter)
        {
            var safeLimit = (limit > 100 ? 100 : limit);

            var relativeUrl = $"products/{Exchange.EncodeProductId(symbol)}/trades?limit={safeLimit}";

            if (!string.IsNullOrEmpty(filter))
                relativeUrl += $"&after={filter}";

            var data = new Dictionary<string, object>();
            data.Add("SymbolCode", symbol.Code);
            data.Add("Limit", safeLimit);
            
            return await InternalRequest<ICollection<CoinbaseProMarketTrade>, TradeResult>(false, relativeUrl, HttpMethod.Get, data);
        }

        public Task<WrappedResponse<ICollection<ExchangeStats>>> GetStats(ISymbol symbol, ExchangeStatsKeyEnum statsKey)
        {
            throw new NotImplementedException();
        }

        #region Private Functionality     
        private string PrivateKey { get; set; }

        private string PublicKey { get; set; }

        private string Passphrase { get; set; }

        private async Task<WrappedResponse<T2>> InternalRequest<T, T2>(bool authRequired, string relativeUrl, HttpMethod method, object requestData)
        {
            NameValueCollection headers = null;

            var postData = requestData != null ? JsonConvert.SerializeObject(requestData) : null;

            if (authRequired)
            {
                var timeStamp = Epoch.Now.Timestamp.ToString();

                headers = new NameValueCollection();
                headers.Add("CB-ACCESS-KEY", PublicKey);
                headers.Add("CB-ACCESS-TIMESTAMP", timeStamp);
                headers.Add("CB-ACCESS-PASSPHRASE", Passphrase);

                var encoding = new UTF8Encoding();
                //var keyByte = encoding.GetBytes(PrivateKey);

                var prehash = $"{timeStamp}{method.ToString().ToUpper()}{relativeUrl}{postData}";

                var messageBytes = encoding.GetBytes(prehash);

                using (var hmacsha256 = new HMACSHA256(Convert.FromBase64String(PrivateKey)))
                {
                    var hash = hmacsha256.ComputeHash(messageBytes);
                    var signature = Convert.ToBase64String(hash);

                    headers.Add("CB-ACCESS-SIGN", signature);
                }
            }

            var url = this.GetFullUrl(relativeUrl);

            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(url),
                    Method = method
                };

                if (method == HttpMethod.Post && postData != null)
                    request.Content = new StringContent(postData, Encoding.UTF8, "application/json");

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

                            long? cbBefore = null;

                            if (response.Headers.Contains("cb-before"))
                                cbBefore = Convert.ToInt64(response.Headers.GetValues("cb-before").First());

                            response.Content.Dispose();

                            var obj = JsonConvert.DeserializeObject<T>(json);

                            json = null;

                            return new WrappedResponse<T2>
                            {
                                StatusCode = WrappedResponseStatusCode.Ok,
                                Data = Exchange.ChangeType<T, T2>(CurrencyFactory, SymbolFactory, requestData, obj, cbBefore)
                            };
                        }
                        catch (HttpRequestException ex)
                        {
                            try
                            {
                                var error = JsonConvert.DeserializeObject<CoinbaseProRequestError>(json);

                                return new WrappedResponse<T2>
                                {
                                    StatusCode = WrappedResponseStatusCode.ApiError,
                                    ErrorMessage = error.Message
                                };
                            }
                            catch (Exception)
                            {
                                return new WrappedResponse<T2>
                                {
                                    StatusCode = WrappedResponseStatusCode.FatalError,
                                    ErrorMessage = ex.Message
                                };
                            }
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
