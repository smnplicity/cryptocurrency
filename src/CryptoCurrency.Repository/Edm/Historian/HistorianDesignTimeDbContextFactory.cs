using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CryptoCurrency.Repository.Edm.Historian
{
    public class HistorianDesignTimeDbContextFactory : IDesignTimeDbContextFactory<HistorianDbContext>
    {
        private IOptions<DbContextConfigurationOptions> Configuration { get; set; }

        private ILoggerFactory LoggerFactory { get; set; }

        private DbContextOptions<HistorianDbContext> Options { get; set; }

        public HistorianDesignTimeDbContextFactory(IOptions<DbContextConfigurationOptions> configuration, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;

            LoggerFactory = loggerFactory;

            var builder = new DbContextOptionsBuilder<HistorianDbContext>();

            var connectionString = Configuration.Value.HistorianConnectionString;

            builder.UseLoggerFactory(LoggerFactory);

            builder.UseMySQL(connectionString);

            builder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            Options = builder.Options;
        }

        public HistorianDbContext CreateDbContext(string[] args)
        {
            return new HistorianDbContext(Options, LoggerFactory);
        }
    }
}
