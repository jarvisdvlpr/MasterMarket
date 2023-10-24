using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewMasterMarket.Models
{
    public class Item
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Называние не может быть пустым.")]
        [StringLength(maximumLength: 5000)]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }

        [StringLength(maximumLength: 5000)]
        public string Description { get; set; }

        [StringLength(maximumLength: 5000)]
        public string Imgsrc { get; set; }
        public string Code { get; set; }
        public bool IsDeleted { get; set; } = false;

        //newOne
        public int MinAmount { get; set; } = 1;
        public Company Company { get; set; }
        public Category Category { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
