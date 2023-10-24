using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewMasterMarket.Models
{
    public class Company
    {
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 5000)]
        public string Name { get; set; }

        [StringLength(maximumLength: 5000)]
        public string Description { get; set; }

        [StringLength(maximumLength: 500)]
        public string ImgSrc { get; set; }
        public bool IsDeleted { get; set; } = false;

        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
