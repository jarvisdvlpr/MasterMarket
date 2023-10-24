using System.ComponentModel.DataAnnotations;

namespace NewMasterMarket.Models
{
    public class Setting
    {
        public int Id { get; set; }

        [Required]
        [StringLength(1000)]
        public string Key { get; set; }

        [Required]
        [StringLength(1000)]
        public string Value { get; set; }
    }
}
