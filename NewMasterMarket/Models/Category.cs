using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewMasterMarket.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Называние не может быть пустым!")]
        [StringLength(maximumLength: 1000)]
        public string Name { get; set; }

        [StringLength(maximumLength: 1000)]
        public string IconSrc { get; set; }
        public bool IsDeleted { get; set; } = false;

        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
