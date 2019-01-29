using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoCurrency.Repository.Edm.Historian
{
    [Table("symbol")]
    public class SymbolEntity
    {
        [Key]
        [Column("symbol_id")]
        public int Id { get; set; }

        [Required]
        [Column("code")]
        public string Code { get; set; }

        [Required]
        [Column("base_currency_id")]
        public int BaseCurrencyId { get; set; }

        [Required]
        [Column("quote_currency_id")]
        public int QuoteCurrencyId { get; set; }

        [Required]
        [Column("tradable")]
        public int Tradable { get; set; }
    }
}
