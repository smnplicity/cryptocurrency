using System.Collections.Generic;
using System.Threading.Tasks;

using CryptoCurrency.Core;
using CryptoCurrency.Core.Currency;
using CryptoCurrency.Core.Exchange;
using CryptoCurrency.Core.Exchange.Model;
using CryptoCurrency.Core.Extensions;
using CryptoCurrency.Core.Symbol;

using CryptoCurrency.ExchangeClient.Binance.Model;

namespace CryptoCurrency.ExchangeClient.Binance
{
    public class Binance : IExchange
    {
        private ISymbolFactory SymbolFactory { get; set; }

        public Binance(ISymbolFactory symbolFactory)
        {
            SymbolFactory = symbolFactory;
        }

        public ExchangeEnum Name => ExchangeEnum.Binance;

        public ICollection<ExchangeCurrency> Currency
        {
            get
            {
                return new List<ExchangeCurrency>()
                {
                    new ExchangeCurrency { CurrencyCode = CurrencyCodeEnum.BTC, Precision = 8  },
                    new ExchangeCurrency { CurrencyCode = CurrencyCodeEnum.ETH, Precision = 8  },
                    new ExchangeCurrency { CurrencyCode = CurrencyCodeEnum.LTC, Precision = 8  },
                    new ExchangeCurrency { CurrencyCode = CurrencyCodeEnum.XLM, Precision = 8  },
                    new ExchangeCurrency { CurrencyCode = CurrencyCodeEnum.USDT, Precision = 8  },
                };
            }
        }

        public ICollection<SymbolCodeEnum> Symbol
        {
            get
            {
                return new List<SymbolCodeEnum>
                {
                    SymbolCodeEnum.BTCUSDT,
                    SymbolCodeEnum.ETHUSDT,
                    SymbolCodeEnum.LTCUSDT,
                    SymbolCodeEnum.ETHBTC,
                    SymbolCodeEnum.LTCBTC,
                    SymbolCodeEnum.XLMBTC,
                    SymbolCodeEnum.XLMETH
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
        private BinanceInfo Info { get; set; }

        public async Task<BinanceInfo> GetExchangeInfo()
        {
            if (Info == null)
                Info = await HttpProxy.GetJson<BinanceInfo>(GetHttpClient().GetFullUrl("v1/exchangeInfo"), null);
  
            return Info;
        }
        #endregion
    }
}
