using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoCurrency.Repository.Edm.Historian
{
    [Table("historian_log")]
    public class HistorianLogEntity
    {
        [Key]
        [Column("log_id")]
        public int Id { get; set; }

        [Column("timestamp")]
        public long Timestamp { get; set; }

        [Column("log_level_id")]
        public int LevelId { get; set; }

        [Column("category")]
        public string Category { get; set; }

        [Column("message")]
        public string Message { get; set; }

        [Column("exception")]
        public string Exception { get; set; }

        [Column("exchange_id")]
        public int? ExchangeId { get; set; }

        [Column("symbol_id")]
        public int? SymbolId { get; set; }
        
        [Column("protocol")]
        public string Protocol { get; set; }
    }
}