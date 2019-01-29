using System;

using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

using CryptoCurrency.Repository.Edm.Historian;

namespace CryptoCurrency.Repository.Extension
{
    public static class ServiceProviderExtension
    {
        public static IServiceProvider WarmUpDbContext(this ServiceProvider serviceProvider)
        {
            var historianDbContext = serviceProvider.GetService<IDesignTimeDbContextFactory<HistorianDbContext>>();

            using (var ctx = historianDbContext.CreateDbContext(null))
            {
                // Do nothing...
            }

            return serviceProvider;
        }
    }
}
