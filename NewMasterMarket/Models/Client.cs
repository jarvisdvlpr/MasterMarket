using System.ComponentModel.DataAnnotations;

namespace NewMasterMarket.Models
{
    public class Client
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength:35)]
        public string Fullname { get; set; }
        [Required]
        [StringLength(maximumLength: 15)]
        public string Number { get; set; }
        [Required]
        [StringLength(maximumLength: 150)]
        public string Address { get; set; }

        [DataType(DataType.EmailAddress)]
        [StringLength(maximumLength:100)]
        public string? Email { get; set; }

        public bool IsDeleted { get; set; } = false;

    }
}
