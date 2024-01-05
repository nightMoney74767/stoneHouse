using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace J85452___CO5227_Restaurant_Project.Data
{
    public class MenuClass
    {
        // Columns for menu table
        [Key]
        public int ItemID { get; set; }
        [StringLength(25)]
        public string ItemName { get; set; }
        [StringLength(100)]
        public string ItemDescription { get; set; }
        public int NumOfItemsAvailable { get; set; }
        public decimal ItemPrice { get; set; }
        public byte[] ItemImage { get; set; }
    }
}
