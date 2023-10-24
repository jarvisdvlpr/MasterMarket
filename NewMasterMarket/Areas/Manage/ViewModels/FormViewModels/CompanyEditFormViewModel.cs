using System.ComponentModel.DataAnnotations;

namespace NewMasterMarket.Areas.Manage.ViewModels.FormViewModels
{
    public class CompanyEditFormViewModel
    {
        [Required]
        [StringLength(maximumLength: 5000)]
        public string Name { get; set; }

        [StringLength(maximumLength: 5000)]
        public string Description { get; set; }

        public IFormFile? ImageFile { get; set; }
    }
}
