using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace J85452___CO5227_Restaurant_Project.Data
{
    public class RestaurantClass
    {
        // Columns for restaurant table
        [Key]
        public int RestaurantID { get; set; }
        [StringLength(25)]
        public string RestaurantName { get; set; }
        public string RestaurantStreetName { get; set; }
        public string RestaurantParish { get; set; }
        public string RestaurantCity { get; set; }
        public string RestaurantCounty { get; set; }
        [StringLength(8)]
        public string RestaurantPostCode { get; set; }
        public string RestaurantPhone { get; set; }
        [StringLength(50)]
        public string RestaurantEmail { get; set; }
    }
}
