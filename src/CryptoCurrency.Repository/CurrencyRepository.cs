using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using CryptoCurrency.Core.Currency;
using CryptoCurrency.Repository.Edm.Historian;
using Microsoft.EntityFrameworkCore.Design;

namespace CryptoCurrency.Repository
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private ILoggerFactory LoggerFactory { get; set; }

        private IDesignTimeDbContextFactory<HistorianDbContext> ContextFactory { get; set; }

        public CurrencyRepository(ILoggerFactory loggerFactory, IDesignTimeDbContextFactory<HistorianDbContext> contextFactory)
        {
            LoggerFactory = loggerFactory;

            ContextFactory = contextFactory;
        }

        public async Task Add(ICurrency currency)
        {
            using (var context = ContextFactory.CreateDbContext(null))
            {
                var entity = new CurrencyEntity
                {
                    Id = (int)currency.Code,
                    Code = currency.Code.ToString(),
                    Symbol = currency.Symbol,
                    Label = currency.Label
                };

                if (await context.Currency.FindAsync(entity.Id) == null)
                {
                    await context.Currency.AddAsync(entity);

                    await context.SaveChangesAsync();
                }
            }
        }
    }
}