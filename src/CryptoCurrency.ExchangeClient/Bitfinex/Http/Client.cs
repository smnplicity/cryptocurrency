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
using Newtonsoft.Json.Linq;

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

using CryptoCurrency.ExchangeClient.Bitfinex.Model;

namespace CryptoCurrency.ExchangeClient.Bitfinex.Http
{
    public class Client : IExchangeHttpClient
    {
        private Bitfinex Exchange { get; set; }
        
        private ISymbolFactory SymbolFactory { get; set; }

        public IRateLimiter RateLimiter { get; set; }

        public Client(Bitfinex ex, ISymbolFactory symbolFactory)
        {
            Exchange = ex;

            SymbolFactory = symbolFactory;

            RateLimiter = new BitfinexRateLimiter();
        }

        public string ApiUrl => "https://api.bitfinex.com/";

        public bool MultiTickSupported => true;

        public string InitialTradeFilter => "0";

        public void SetApiAccess(string privateKey, string publicKey, string passphrase)
        {
            PrivateKey = privateKey;
            PublicKey = publicKey;
        }

        public Task<WrappedResponse<ICollection<AccountBalance>>> GetBalance()
        {
            throw new NotImplementedException();
        }

        public Task<WrappedResponse<MarketTick>> GetTick(ISymbol symbol)
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
                        CurrencyCode = symbol.QuoteCurrencyCode,
                        Maker = 0.002m,
                        Taker = 0.002m
                    }
                };
            });
        }

        public async Task<WrappedResponse<ICollection<MarketTick>>> GetTicks(ICollection<ISymbol> symbols)
        {
            var converted = symbols.Select(s => $"t{Exchange.GetCurrencyCode(s.BaseCurrencyCode)}{Exchange.GetCurrencyCode(s.QuoteCurrencyCode)}");

            var relativeUrl = $"v2/tickers?symbols={string.Join(",", symbols.Select(s => $"t{s.Code.ToString()}"))}";

            return await InternalRequest<ICollection<object[]>, ICollection<MarketTick>>(false, relativeUrl, HttpMethod.Get, null, null);
        }

        public Task<WrappedResponse<CreateOrder>> CreateOrder(ISymbol symbol, OrderTypeEnum orderType, OrderSideEnum orderSide, decimal price, decimal volume)
        {
            throw new NotImplementedException();
        }

        public Task<WrappedResponse<ICollection<TradeItem>>> GetTradeHistory(ISymbol symbol, int pageNumber, int pageSize, string fromTradeId)
        {
            throw new NotImplementedException();
        }

        public Task<WrappedResponse<ICollection<OrderItem>>> GetOpenOrders(ISymbol symbol, int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<WrappedResponse<CancelOrder>> CancelOrder(ISymbol symbol, string[] orderIds)
        {
            throw new NotImplementedException();
        }

        public Task<WrappedResponse<WithdrawCrypto>> WithdrawCrypto(CurrencyCodeEnum cryptoCurrencyCode, decimal withdrawalFee, decimal volume, string address)
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
            var relativeUrl = $"v2/trades/t{Exchange.GetCurrencyCode(symbol.BaseCurrencyCode)}{Exchange.GetCurrencyCode(symbol.QuoteCurrencyCode)}/hist";

            var query = new NameValueCollection();

            if (limit != 0)
                query.Add("limit", limit.ToString());

            if (filter != null)
            {
                query.Add("start", filter);

                query.Add("sort", "1");
            }

            var additionalData = new NameValueCollection();
            additionalData.Add("SymbolCode", symbol.Code.ToString());

            return await InternalRequest<dynamic, TradeResult>(false, relativeUrl, HttpMethod.Get, query, additionalData);
        }

        public async Task<WrappedResponse<ICollection<ExchangeStats>>> GetStats(ISymbol symbol, ExchangeStatsKeyEnum statsKey)
        {
            var relativeUrl = $"v2/stats1/pos.size:1m:t{Exchange.GetCurrencyCode(symbol.BaseCurrencyCode)}{Exchange.GetCurrencyCode(symbol.QuoteCurrencyCode)}:{(statsKey == ExchangeStatsKeyEnum.OpenLongs ? "long" : "short")}/hist";

            var additionalData = new NameValueCollection();
            additionalData.Add("StatKey", statsKey.ToString());
            additionalData.Add("SymbolCode", symbol.Code.ToString());

            return await InternalRequest<List<BitfinexMarketStats>, ICollection<ExchangeStats>>(false, relativeUrl, HttpMethod.Get, null, additionalData);
        }

        #region Private Functionality
        private string PrivateKey { get; set; }

        private string PublicKey { get; set; }

        private string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
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

        private async Task<WrappedResponse<T2>> InternalRequest<T, T2>(bool authRequired, string relativeUrl, HttpMethod method, NameValueCollection query, NameValueCollection additionalData)
        {
            await RateLimiter.Wait();

            var url = this.GetFullUrl(relativeUrl);

            if (method == HttpMethod.Get && query != null)
                url += "?" + ToQueryString(query);

            NameValueCollection headers = null;

            if (authRequired)
            {
                var nonce = Epoch.Now.Timestamp.ToString();

                headers = new NameValueCollection();
                headers.Add("bfx-apikey", PublicKey);
                headers.Add("bfx-nonce", nonce);

                var encoding = new UTF8Encoding();
                //var encoding = new ASCIIEncoding();
                var keyByte = encoding.GetBytes(PrivateKey);

                var prehash = $"/api/{relativeUrl}{nonce}{""}";

                var messageBytes = encoding.GetBytes(prehash);

                using (var hmacsha384 = new HMACSHA384(keyByte))
                {
                    var hash = hmacsha384.ComputeHash(messageBytes);
                    var signature = ByteArrayToString(hash);

                    headers.Add("bfx-signature", signature);
                }
            }

            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(url),
                    Method = method
                };

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
                                Data = Exchange.ChangeType<T, T2>(SymbolFactory, obj, query, additionalData)
                            };
                        }
                        catch (HttpRequestException)
                        {
                            var parsed = JToken.Parse(json);

                            if (parsed is JArray)
                            {
                                var error = JsonConvert.DeserializeObject<BitfinexApiErrorCollection>(json);

                                return new WrappedResponse<T2>
                                {
                                    StatusCode = WrappedResponseStatusCode.ApiError,
                                    ErrorCode = error[1],
                                    ErrorMessage = error[2]
                                };
                            }
                            else
                            {
                                var error = JsonConvert.DeserializeObject<BitfinexApiError>(json);

                                return new WrappedResponse<T2>
                                {
                                    StatusCode = WrappedResponseStatusCode.ApiError,
                                    ErrorCode = error.Code.ToString(),
                                    ErrorMessage = error.ErrorDescription
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
