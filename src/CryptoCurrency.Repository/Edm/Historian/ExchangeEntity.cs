using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoCurrency.Repository.Edm.Historian
{
    [Table("exchange")]
    public class ExchangeEntity
    {
        [Key]
        [Column("exchange_id")]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }
    }
}
