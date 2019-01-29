using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoCurrency.Repository.Edm.Historian
{
    [Table("historian_exchange_symbol")]
    public class HistorianExchangeSymbolEntity
    {
        [Column("exchange_id")]
        public int ExchangeId { get; set; }

        [Column("symbol_id")]
        public int SymbolId { get; set; }

        [Column("trade_filter")]
        public string TradeFilter { get; set; }

        [Column("last_trade_id")]
        public long? LastTradeId { get; set; }

        [Column("last_trade_stat_id")]
        public long? LastTradeStatId { get; set; }
    }
}
