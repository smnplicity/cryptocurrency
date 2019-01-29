using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoCurrency.Repository.Edm.Historian
{
    [Table("currency")]
    public class CurrencyEntity
    {
        [Key]
        [Column("currency_id")]
        public int Id { get; set; }

        [Required]
        [Column("code")]
        public string Code { get; set; }

        [Required]
        [Column("symbol")]
        public string Symbol { get; set; }

        [Column("label")]
        public string Label { get; set; }
    }
}
