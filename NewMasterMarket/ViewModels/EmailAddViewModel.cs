using System.ComponentModel.DataAnnotations;

namespace NewMasterMarket.ViewModels
{
    public class EmailAddViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
