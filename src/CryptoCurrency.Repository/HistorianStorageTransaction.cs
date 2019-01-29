using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore.Storage;

using CryptoCurrency.Core.StorageTransaction;
using CryptoCurrency.Repository.Edm.Historian;

namespace CryptoCurrency.Repository
{
    public class HistorianStorageTransaction : IStorageTransaction
    {
        private HistorianDbContext Context { get; set; }
        
        public HistorianStorageTransaction(HistorianDbContext context)
        {
            Context = context;

            Context.Database.BeginTransaction();
        }

        public object GetContext()
        {
            return Context;
        }

        public async Task Commit()
        {
            await Context.SaveChangesAsync();

            Context.Database.CurrentTransaction.Commit();
        }

        public void Dispose()
        {
            Rollback().Wait();
        }

        public Task Rollback() => Task.Run(() =>
        {
            Context.Dispose();
        });
    }
}
