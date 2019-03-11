using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoCurrency.Repository.Edm.Historian
{
    [Table("exchange_trade_stat")]
    public class ExchangeTradeStatEntity
    {
        [Column("exchange_id")]
        public int ExchangeId { get; set; }

        [Column("symbol_id")]
        public int SymbolId { get; set; }

        [Column("stat_key_id")]
        public int StatKeyId { get; set; }

        [Column("timestamp")]
        public long Timestamp { get; set; }

        [Column("trade_stat_id")]
        public long TradeStatId { get; set; }

        [Column("value")]
        public decimal Value { get; set; }
    }
}
