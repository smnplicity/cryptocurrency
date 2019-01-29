using System.Collections.Generic;

using CryptoCurrency.Core.Currency;
using CryptoCurrency.Core.Exchange;
using CryptoCurrency.Core.Exchange.Model;
using CryptoCurrency.Core.Extensions;
using CryptoCurrency.Core.Symbol;

namespace CryptoCurrency.ExchangeClient.Bitfinex
{
    public class Bitfinex : IExchange
    {
        private ISymbolFactory SymbolFactory { get; set; }

        public Bitfinex(ISymbolFactory symbolFactory)
        {
            SymbolFactory = symbolFactory;
        }

        public int Id => 6;

        public ExchangeEnum Name => ExchangeEnum.Bitfinex;

        public ICollection<ExchangeCurrency> Currency
        {
            get
            {
                return new List<ExchangeCurrency>
                {
                    new ExchangeCurrency { CurrencyCode = CurrencyCodeEnum.BTC, Precision = 5 },
                    new ExchangeCurrency { CurrencyCode = CurrencyCodeEnum.ETH, Precision = 5 },
                    new ExchangeCurrency { CurrencyCode = CurrencyCodeEnum.LTC, Precision = 5 },
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

        public ICollection<ExchangeStatsKeyEnum> SupportedStatKeys
        {
            get
            {
                return new List<ExchangeStatsKeyEnum>
                {
                    ExchangeStatsKeyEnum.OpenShorts,
                    ExchangeStatsKeyEnum.OpenLongs
                };
            }
        }

        public bool SupportsHistoricalLoad => true;

        public IExchangeHttpClient GetHttpClient()
        {
            return new Http.Client(this, SymbolFactory);
        }

        public IExchangeWebSocketClient GetWebSocketClient()
        {
            return new WebSocket.Client(this, SymbolFactory);
        }

        #region Custom Functionality
        public CurrencyCodeEnum[] DecodeSymbol(string symbol)
        {
            return new CurrencyCodeEnum[2]
            {
                this.GetStandardisedCurrencyCode(symbol.Substring(1, 3)),
                this.GetStandardisedCurrencyCode(symbol.Substring(4, 3))
            };
        }
        #endregion
    }
}