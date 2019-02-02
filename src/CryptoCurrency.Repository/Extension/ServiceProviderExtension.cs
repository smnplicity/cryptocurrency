using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using CryptoCurrency.Repository.Edm.Historian;

namespace CryptoCurrency.Repository.Extension
{
    public static class ServiceProviderExtension
    {
        public static bool WarmUpDbContext(this ServiceProvider serviceProvider, ILogger logger)
        {
            var historianDbContext = serviceProvider.GetService<IDesignTimeDbContextFactory<HistorianDbContext>>();

            try
            {
                using (var ctx = historianDbContext.CreateDbContext(null))
                {
                    var connection = ctx.Database.GetDbConnection();

                    connection.Open();
                    connection.Close();
                }

                return true;
            }
            catch(Exception ex)
            {
                logger.LogCritical(ex, "Unable to warm up db context");

                return false;
            }
        }
    }
}
