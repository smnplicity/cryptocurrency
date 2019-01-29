using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoCurrency.Repository.Edm.Historian
{
    [Table("historian_trade_catchup")]
    public class HistorianTradeCatchupEntity
    {
        [Column("exchange_id")]
        public int ExchangeId { get; set; }

        [Column("symbol_id")]
        public int SymbolId { get; set; }

        [Column("trade_filter")]
        public string TradeFilter { get; set; }

        [Column("timestamp_to")]
        public long TimestampTo { get; set; }

        [Column("current_trade_filter")]
        public string CurrentTradeFilter { get; set; }

        [Column("priority")]
        public int Priority { get; set; }
    }
}
