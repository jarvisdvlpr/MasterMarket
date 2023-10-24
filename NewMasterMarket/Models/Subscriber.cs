using System.ComponentModel.DataAnnotations;

namespace NewMasterMarket.Models
{
    public class Subscriber
    {
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        [StringLength(maximumLength: 256)]
        public string Email { get; set; }
    }
}
