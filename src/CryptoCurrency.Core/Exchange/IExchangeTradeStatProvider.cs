using CryptoCurrency.Core.Symbol;

namespace CryptoCurrency.Core.Exchange
{
    public interface IExchangeTradeStatProvider
    {
        SymbolCodeEnum Convert(SymbolCodeEnum baseSymbolCode, ExchangeStatsKeyEnum statsKey);
    }
}