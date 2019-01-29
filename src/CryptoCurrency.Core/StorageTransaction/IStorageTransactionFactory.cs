using System.Threading.Tasks;

namespace CryptoCurrency.Core.StorageTransaction
{
    public interface IStorageTransactionFactory<T>
    {
        Task<IStorageTransaction> Begin();
    }
}
