using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CryptoCurrency.Repository.Edm.Historian
{
    public class HistorianDbContext : DbContext
    {
        private ILoggerFactory LoggerFactory { get; set; }
        
        public DbSet<CurrencyEntity> Currency { get; set; }

        public DbSet<SymbolEntity> Symbol { get; set; }

        public DbSet<ExchangeEntity> Exchange { get; set; }
        
        public DbSet<ExchangeSymbolEntity> ExchangeSymbol { get; set; }
        
        public DbSet<ExchangeTradeEntity> ExchangeTrade { get; set; }

        public DbSet<ExchangeTradeStatEntity> ExchangeTradeStat { get; set; }

        public DbSet<ExchangeTradeAggregateEntity> ExchangeTradeAggregate { get; set; }

        public DbSet<IntervalKeyEntity> IntervalKey { get; set; }

        public DbSet<IntervalEntity> Interval { get; set; }

        public DbSet<HistorianExchangeSymbolEntity> HistorianExchangeSymbol { get; set; }

        public DbSet<HistorianTradeCatchupEntity> HistorianTradeCatchup { get; set; }

        public DbSet<HistorianLogEntity> HistorianLog { get; set; }

        public DbSet<OrderSideEntity> OrderSide { get; set; }

        public HistorianDbContext(DbContextOptions<HistorianDbContext> options, ILoggerFactory loggerFactory) : base(options)
        {
            LoggerFactory = loggerFactory;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define any composite key tables
            modelBuilder.Entity<ExchangeTradeEntity>()
                .HasKey(k => new { k.ExchangeId, k.SymbolId, k.Timestamp, k.TradeId });

            modelBuilder.Entity<ExchangeTradeStatEntity>()
                .HasKey(k => new { k.ExchangeId, k.SymbolId, k.StatKeyId, k.Timestamp, k.TradeStatId });

            modelBuilder.Entity<ExchangeTradeAggregateEntity>()
                .HasKey(k => new { k.ExchangeId, k.SymbolId, k.IntervalKey, k.Timestamp });

            modelBuilder.Entity<ExchangeSymbolEntity>()
                .HasKey(k => new { k.ExchangeId, k.SymbolId });
            
            modelBuilder.Entity<IntervalEntity>()
                .HasKey(k => new { k.IntervalKey, k.FromTimestamp });

            modelBuilder.Entity<HistorianExchangeSymbolEntity>()
                .HasKey(k => new { k.ExchangeId, k.SymbolId });

            modelBuilder.Entity<HistorianTradeCatchupEntity>()
                .HasKey(k => new { k.ExchangeId, k.SymbolId, k.TradeFilter });
        }
    }
}