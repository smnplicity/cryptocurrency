using System.Threading.Tasks;

using CryptoCurrency.Core.Symbol;

namespace CryptoCurrency.Core.Exchange
{
    public interface IExchangeRepository
    {
        Task Add(IExchange exchange);

        Task AddSymbol(ExchangeEnum exchange, SymbolCodeEnum symbolCode);
    }
}