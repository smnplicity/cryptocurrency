using System;

using CryptoCurrency.Core.Symbol;

namespace CryptoCurrency.Core.Exchange
{
    public class ExchangeTradeStatProvider : IExchangeTradeStatProvider
    {
        public SymbolCodeEnum Convert(SymbolCodeEnum baseSymbolCode, ExchangeStatsKeyEnum statsKey)
        {
            string convertedSymbolCode = null;

            switch(statsKey)
            {
                case ExchangeStatsKeyEnum.OpenLongs:
                    convertedSymbolCode = baseSymbolCode.ToString() + "LONGS"; 
                    break;
                case ExchangeStatsKeyEnum.OpenShorts:
                    convertedSymbolCode = baseSymbolCode.ToString() + "SHORTS";
                    break;
                default:
                    throw new ArgumentException($"Unsupported stats key: {statsKey}");
            }

            if (convertedSymbolCode == null)
                throw new ArgumentException($"Unable to convert {baseSymbolCode} {statsKey}");

            return (SymbolCodeEnum)Enum.Parse(typeof(SymbolCodeEnum), convertedSymbolCode);
        }
    }
}
