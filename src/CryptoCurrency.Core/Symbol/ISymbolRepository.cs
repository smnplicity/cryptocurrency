using System.Threading.Tasks;

namespace CryptoCurrency.Core.Symbol
{
    public interface ISymbolRepository
    {
        Task Add(ISymbol symbol);
    }
}
