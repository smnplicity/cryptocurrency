using System.Collections.Generic;

using CryptoCurrency.Core.Currency;
using CryptoCurrency.Core.Exchange;
using CryptoCurrency.Core.Exchange.Model;
using CryptoCurrency.Core.Extensions;
using CryptoCurrency.Core.OrderSide;
using CryptoCurrency.Core.OrderState;
using CryptoCurrency.Core.OrderType;
using CryptoCurrency.Core.Symbol;

namespace CryptoCurrency.ExchangeClient.CoinbasePro
{
    public class CoinbasePro : IExchange
    {
        private ISymbolFactory SymbolFactory { get; set; }

        public CoinbasePro(ISymbolFactory symbolFactory)
        {
            SymbolFactory = symbolFactory;
        }

        public ExchangeEnum Name => ExchangeEnum.CoinbasePro;

        public ICollection<ExchangeCurrency> Currency
        {
            get
            {
                return new List<ExchangeCurrency>()
                {
                    new ExchangeCurrency { CurrencyCode = CurrencyCodeEnum.BTC, Precision = 8 },
                    new ExchangeCurrency { CurrencyCode = CurrencyCodeEnum.ETH, Precision = 8 },
                    new ExchangeCurrency { CurrencyCode = CurrencyCodeEnum.LTC, Precision = 8 },
                    new ExchangeCurrency { CurrencyCode = CurrencyCodeEnum.USD, Precision = 2 }
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
        public OrderStateEnum GetOrderState(string status)
        {
            switch (status)
            {
                case "open":
                case "active":
                    return OrderStateEnum.Processing;
                case "done":
                    return OrderStateEnum.Complete;
                case "pending":
                default:
                    return OrderStateEnum.Pending;
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

        public ISymbol DecodeProductId(string productId)
        {
            var baseCurrencyCode = this.GetStandardisedCurrencyCode(productId.Substring(0, 3));
            var quoteCurrencyCode = this.GetStandardisedCurrencyCode(productId.Substring(4, 3));

            return SymbolFactory.Get(baseCurrencyCode, quoteCurrencyCode);            
        }

        public string EncodeProductId(ISymbol symbol)
        {
            return $"{symbol.BaseCurrencyCode}-{symbol.QuoteCurrencyCode}";
        }
        #endregion
    }
}
