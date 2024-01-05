using System.ComponentModel.DataAnnotations;

namespace J85452___CO5227_Restaurant_Project.CS
{
    // Class for storing basket items
    public class BasketItemClass
    {
        [Required]
        public int ItemID { get; set; }
        [Required]
        public int BasketID { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
