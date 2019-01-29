using System.Threading.Tasks;

namespace CryptoCurrency.Core.Currency
{
    public interface ICurrencyRepository
    {
        Task Add(ICurrency currency);
    }
}
