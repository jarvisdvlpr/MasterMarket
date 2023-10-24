using NewMasterMarket.Models;

namespace NewMasterMarket.ViewModels
{
    public class CheckoutFrontViewModel
    {
        public List<Category> CategoryList { get; set; }
        public List<BasketCountModel> Items { get; set; }
        public List<BasketItemViewModel> BasketItems { get; set; }
        public Client CurrentClient { get; set; }

    }
}
