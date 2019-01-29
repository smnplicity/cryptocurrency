using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Design;

using CryptoCurrency.Core.OrderSide;

using CryptoCurrency.Repository.Edm.Historian;

namespace CryptoCurrency.Repository
{
    public class OrderSideRepository : IOrderSideRepository
    {
        private ILogger Logger { get; set; }

        private IDesignTimeDbContextFactory<HistorianDbContext> ContextFactory { get; set; }

        public OrderSideRepository(ILoggerFactory loggerFactory, IDesignTimeDbContextFactory<HistorianDbContext> contextFactory)
        {
            ContextFactory = contextFactory;

            Logger = loggerFactory.CreateLogger<MarketRepository>();
        }

        public async Task Add(OrderSideEnum orderSide)
        {
            using (var context = ContextFactory.CreateDbContext(null))
            {
                var entity = new OrderSideEntity
                {
                    OrderSideId = (int)orderSide,
                    Label = orderSide.ToString()
                };

                if(await context.OrderSide.FindAsync(entity.OrderSideId) == null)
                {
                    await context.OrderSide.AddAsync(entity);

                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
