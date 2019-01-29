using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoCurrency.Repository.Edm.Historian
{
    [Table("order_side")]
    public class OrderSideEntity
    {
        [Key]
        [Column("order_side_id")]
        public int OrderSideId { get; set; }

        [Column("label")]
        public string Label { get; set; }
    }
}
