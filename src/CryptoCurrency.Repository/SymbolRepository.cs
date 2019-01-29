using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using CryptoCurrency.Core.Symbol;
using CryptoCurrency.Repository.Edm.Historian;
using Microsoft.EntityFrameworkCore.Design;

namespace CryptoCurrency.Repository
{
    public class SymbolRepository : ISymbolRepository
    {
        private ILoggerFactory LoggerFactory { get; set; }

        private IDesignTimeDbContextFactory<HistorianDbContext> ContextFactory { get; set; }

        public SymbolRepository(ILoggerFactory loggerFactory, IDesignTimeDbContextFactory<HistorianDbContext> contextFactory)
        {
            LoggerFactory = loggerFactory;

            ContextFactory = contextFactory;
        }

        public async Task Add(ISymbol symbol)
        {
            using (var context = ContextFactory.CreateDbContext(null))
            {
                var entity = new SymbolEntity
                {
                    Id = (int)symbol.Code,
                    Code = symbol.Code.ToString(),
                    BaseCurrencyId = (int)symbol.BaseCurrencyCode,
                    QuoteCurrencyId = (int)symbol.QuoteCurrencyCode,
                    Tradable = symbol.Tradable ? 1 : 0
                };

                if (await context.Symbol.FindAsync(entity.Id) == null)
                {
                    await context.Symbol.AddAsync(entity);

                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
