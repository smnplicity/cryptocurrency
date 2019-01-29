using System.Collections.Generic;
using System.Threading.Tasks;

using CryptoCurrency.Core;
using CryptoCurrency.Core.Currency;
using CryptoCurrency.Core.Extensions;
using CryptoCurrency.Core.Exchange;
using CryptoCurrency.Core.Exchange.Model;
using CryptoCurrency.Core.OrderState;
using CryptoCurrency.Core.OrderSide;
using CryptoCurrency.Core.OrderType;
using CryptoCurrency.Core.Symbol;

using CryptoCurrency.ExchangeClient.Kraken.Model;

namespace CryptoCurrency.ExchangeClient.Kraken
{
    public class Kraken : IExchange
    {
        private ISymbolFactory SymbolFactory { get; set; }

        public Kraken(ISymbolFactory symbolFactory)
        {
            SymbolFactory = symbolFactory;
        }

        public ExchangeEnum Name => ExchangeEnum.Kraken;

        public ICollection<ExchangeCurrency> Currency
        {
            get
            {
                return new List<ExchangeCurrency>()
                {
                    new ExchangeCurrency() { CurrencyCode = CurrencyCodeEnum.BTC, Precision = 8, AltCurrencyCode = "XBT" },
                    new ExchangeCurrency() { CurrencyCode = CurrencyCodeEnum.USD, Precision = 2 },
                    new ExchangeCurrency() { CurrencyCode = CurrencyCodeEnum.LTC, Precision = 8 },
                    new ExchangeCurrency() { CurrencyCode = CurrencyCodeEnum.ETH, Precision = 8 }
                };
            }
        }

        public ICollection<SymbolCodeEnum> Symbol
        {
            get
            {
                return new List<SymbolCodeEnum>
                {
                    SymbolCodeEnum.BTCUSD,
                    SymbolCodeEnum.ETHUSD,
                    SymbolCodeEnum.LTCUSD,
                    SymbolCodeEnum.ETHBTC,
                    SymbolCodeEnum.LTCBTC
                };
            }
        }

        public ICollection<ExchangeStatsKeyEnum> SupportedStatKeys => null;

        public bool SupportsHistoricalLoad => true;

        public IExchangeHttpClient GetHttpClient()
        {
            return new Http.Client(this, SymbolFactory);
        }

        public IExchangeWebSocketClient GetWebSocketClient()
        {
            return null;
        }

        #region Custom functionality
        public CurrencyCodeEnum[] DecodeQuotePair(string pair)
        {
            return new CurrencyCodeEnum[2]
            {
                this.GetStandardisedCurrencyCode(pair.Substring(1, 3)),
                this.GetStandardisedCurrencyCode(pair.Substring(5, 3))
            };
        }

        public CurrencyCodeEnum[] DecodeAssetPair(string pair)
        {
            return new CurrencyCodeEnum[2]
            {
                this.GetStandardisedCurrencyCode(pair.Substring(0, 3)),
                this.GetStandardisedCurrencyCode(pair.Substring(3, 3))
            };
        }

        public OrderStateEnum GetOrderState(string status)
        {
            switch (status)
            {
                case "pending":
                    return OrderStateEnum.Pending;
                case "open":
                    return OrderStateEnum.Processing;
                case "closed":
                    return OrderStateEnum.Complete;
                case "canceled":
                    return OrderStateEnum.Cancelled;
                default:
                    return OrderStateEnum.Cancelled;
            }
        }

        public OrderSideEnum GetOrderSide(string side)
        {
            return side == "buy" ? OrderSideEnum.Buy : OrderSideEnum.Sell;
        }

        public OrderTypeEnum GetOrderType(string type)
        {
            return type == "market" ? OrderTypeEnum.Market : OrderTypeEnum.Limit;
        }

        private Dictionary<string, KrakenAsset> Assets { get; set; }
        
        public async Task<Dictionary<string, KrakenAsset>> GetAssets()
        {
            if (Assets == null)
            {
                var response = await HttpProxy.GetJson<KrakenWrappedResponse<Dictionary<string, KrakenAsset>>>(GetHttpClient().GetFullUrl("0/public/Assets"), null);

                Assets = response.Result;
            }

            return Assets;
        }

        private Dictionary<string, KrakenAssetPair> AssetPairs { get; set; }

        public async Task<Dictionary<string, KrakenAssetPair>> GetAssetPairs()
        {
            if (AssetPairs == null)
            {
                var response = await HttpProxy.GetJson<KrakenWrappedResponse<Dictionary<string, KrakenAssetPair>>>(GetHttpClient().GetFullUrl("0/public/AssetPairs"), null);

                AssetPairs = response.Result;
            }

            return AssetPairs;
        }
        #endregion
    }
}
