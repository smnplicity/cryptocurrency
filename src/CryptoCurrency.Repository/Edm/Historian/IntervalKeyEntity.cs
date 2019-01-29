using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoCurrency.Repository.Edm.Historian
{
    [Table("interval_key")]
    public class IntervalKeyEntity
    {
        [Key]
        [Column("interval_key")]
        public string IntervalKey { get; set; }

        [Required]
        [Column("interval_group_id")]
        public int IntervalGroupId { get; set; }

        [Required]
        [Column("label")]
        public string Label { get; set; }
    }
}