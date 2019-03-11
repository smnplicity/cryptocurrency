using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoCurrency.Repository.Edm.Historian
{
    [Table("exchange_trade")]
    public class ExchangeTradeEntity
    {
        [Column("exchange_id")]
        public int ExchangeId { get; set; }

        [Column("symbol_id")]
        public int SymbolId { get; set; }

        [Column("timestamp")]
        public long Timestamp { get; set; }

        [Column("trade_id")]
        public long TradeId { get; set; }

        [Column("order_side_id")]
        public int? OrderSideId { get; set; }
        
        [Column("price")]
        public decimal Price { get; set; }

        [Column("volume")]
        public decimal Volume { get; set; }
    }
}
