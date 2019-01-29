using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoCurrency.Repository.Edm.Historian
{
    [Table("interval")]
    public class IntervalEntity
    {
        [Required]
        [Column("interval_key")]
        public string IntervalKey { get; set; }

        [Required]
        [Column("from_timestamp")]
        public long FromTimestamp { get; set; }

        [Required]
        [Column("to_timestamp")]
        public long ToTimestamp { get; set; }
    }
}