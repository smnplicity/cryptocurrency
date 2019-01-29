using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoCurrency.Repository.Edm.Historian
{
    [Table("exchange_symbol")]
    public class ExchangeSymbolEntity
    {
        [Key]
        [Column("exchange_id")]
        public int ExchangeId { get; set; }

        [Key]
        [Column("symbol_id")]
        public int SymbolId { get; set; }
    }
}
