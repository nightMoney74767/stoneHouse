using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace J85452___CO5227_Restaurant_Project.Data
{
    public class OrderHistoryClass
    {
        // Columns for order history table
        [Key]
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public int RestaurantID { get; set; }
        public string DateOfSale { get; set; }
        public decimal FinalPrice { get; set; }
    }
}
