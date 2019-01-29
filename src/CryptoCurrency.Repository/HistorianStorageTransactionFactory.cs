using System.Threading.Tasks;

using CryptoCurrency.Core.StorageTransaction;
using CryptoCurrency.Repository.Edm.Historian;

using Microsoft.EntityFrameworkCore.Design;

namespace CryptoCurrency.Repository
{
    public class HistorianStorageTransactionFactory : IStorageTransactionFactory<HistorianDbContext>
    {
        private IDesignTimeDbContextFactory<HistorianDbContext> ContextFactory { get; set; }

        public HistorianStorageTransactionFactory(IDesignTimeDbContextFactory<HistorianDbContext> contextFactory)
        {
            ContextFactory = contextFactory;
        }

        public Task<IStorageTransaction> Begin() => Task.Run(() =>
        {
            return (IStorageTransaction)new HistorianStorageTransaction(ContextFactory.CreateDbContext(null));
        });
    }
}
