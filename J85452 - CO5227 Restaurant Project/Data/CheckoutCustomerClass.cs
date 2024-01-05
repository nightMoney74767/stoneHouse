using System.ComponentModel.DataAnnotations;

namespace J85452___CO5227_Restaurant_Project.CS
{
    // Class for checkouts
    public class CheckoutCustomerClass
    {
        [Key]
        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(50)]
        public string Name { get; set; }
        public int BasketID { get; set; }
    }
}
