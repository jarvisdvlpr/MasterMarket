using System.ComponentModel.DataAnnotations;

namespace NewMasterMarket.Areas.Manage.ViewModels.FormViewModels
{
    public class CategoryFormViewModel
    {
        [Required(ErrorMessage = "Называние не может быть пустым!")]
        [StringLength(maximumLength: 1000)]
        public string Name { get; set; }

        public IFormFile ImageFile { get; set; }
    }
}
