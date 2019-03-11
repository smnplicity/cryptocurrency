using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoCurrency.Repository.Edm.Historian
{
    [Table("exchange_trade_aggregate")]
    public class ExchangeTradeAggregateEntity
    {
        [Column("exchange_id")]
        public int ExchangeId { get; set; }

        [Column("symbol_id")]
        public int SymbolId { get; set; }

        [Column("interval_key")]
        public string IntervalKey { get; set; }

        [Column("timestamp")]
        public long Timestamp { get; set; }

        [Column("open")]
        public decimal Open { get; set; }

        [Column("open_timestamp")]
        public long OpenTimestamp { get; set; }

        [Column("high")]
        public decimal High { get; set; }
        
        [Column("low")]
        public decimal Low { get; set; }

        [Column("close")]
        public decimal Close { get; set; }

        [Column("close_timestamp")]
        public long CloseTimestamp { get; set; }

        [Column("buy_volume")]
        public decimal? BuyVolume { get; set; }

        [Column("sell_volume")]
        public decimal? SellVolume { get; set; }

        [Column("total_volume")]
        public decimal TotalVolume { get; set; }

        [Column("buy_count")]
        public int? BuyCount { get; set; }

        [Column("sell_count")]
        public int? SellCount { get; set; }

        [Column("total_count")]
        public int TotalCount { get; set; }
    }
}
