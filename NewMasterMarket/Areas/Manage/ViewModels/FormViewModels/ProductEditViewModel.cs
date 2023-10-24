using System.ComponentModel.DataAnnotations;

namespace NewMasterMarket.Areas.Manage.ViewModels.FormViewModels
{
    public class ProductEditViewModel
    {
        public int CompanyId { get; set; }
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Называние не может быть пустым.")]
        [StringLength(maximumLength: 5000)]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }

        [StringLength(maximumLength: 5000)]
        public string Description { get; set; }

        public IFormFile? ImageFile { get; set; }
    }
}
