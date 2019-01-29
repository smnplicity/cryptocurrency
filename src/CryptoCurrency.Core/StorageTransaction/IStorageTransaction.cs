using System;
using System.Threading.Tasks;

namespace CryptoCurrency.Core.StorageTransaction
{
    public interface IStorageTransaction : IDisposable
    {
        object GetContext();

        Task Commit();

        Task Rollback();
    }
}
