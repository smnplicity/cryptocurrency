using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoCurrency.Core.Currency;
using CryptoCurrency.Core.Exchange;
using CryptoCurrency.Core.Exchange.Model;
using CryptoCurrency.Core.Extensions;
using CryptoCurrency.Core.Symbol;

namespace CryptoCurrency.ExchangeClient.Bitfinex
{
    public class Bitfinex : IExchange
    {
        private ICurrencyFactory CurrencyFactory { get; set; }

        private ISymbolFactory SymbolFactory { get; set; }

        public Bitfinex(ICurrencyFactory currencyFactory, ISymbolFactory symbolFactory)
        {
            CurrencyFactory = currencyFactory;
            SymbolFactory = symbolFactory;
        }

        public int Id => 6;

        public ExchangeEnum Name => ExchangeEnum.Bitfinex;

        public ICollection<ExchangeCurrencyConverter> CurrencyConverter => new List<ExchangeCurrencyConverter>();

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

        public bool Initialized { get; private set; }

        public Task Initialize() => Task.Run(() =>
        {
            Initialized = true;
        });

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
                this.GetStandardisedCurrencyCode(CurrencyFactory, symbol.Substring(1, 3)),
                this.GetStandardisedCurrencyCode(CurrencyFactory, symbol.Substring(4, 3))
            };
        }
        #endregion
    }
}