using System.ComponentModel.DataAnnotations;

namespace NewMasterMarket.Models
{
    public class Order
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength:37)]
        public string Code { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        public Client Client { get; set; }
        public List<Item> Items { get; set; }

        //status s enumom poprobuy sdelat 

        public bool IsDeleted { get; set; } = false;
    }
}
