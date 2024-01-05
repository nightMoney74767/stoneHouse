using System.ComponentModel.DataAnnotations;

namespace J85452___CO5227_Restaurant_Project.Data
{
    // Class for items logged during purchase
    public class OrderItemClass
    {
        [Required]
        public int OrderID { get; set; }
        [Required]
        public int ItemID { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal Subtotal { get; set; }
    }
}
