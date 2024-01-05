using System.ComponentModel.DataAnnotations;

namespace J85452___CO5227_Restaurant_Project.Data
{
    // Class for items processed in the checkout
    public class CheckoutItemClass
    {
        [Key, Required]
        public int ItemID { get; set; }
        [Required, StringLength(50)]
        public string ItemDescription { get; set; }
        [Required]
        public decimal ItemPrice { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
