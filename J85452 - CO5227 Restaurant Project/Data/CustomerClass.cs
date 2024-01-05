using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using J85452___CO5227_Restaurant_Project.Data;

namespace J85452___CO5227_Restaurant_Project.Data
{
    public class CustomerClass
    {
        // Columns for customer table
        // The column below the word 'key' is the primary key
        [Key]
        public int CustomerID { get; set; }
        [StringLength(25)]
        public string CustomerName { get; set; }
        public string CustomerStreetName { get; set; }
        public string CustomerParish { get; set; }
        public string CustomerCity { get; set; }
        public string CustomerCounty { get; set; }
        [StringLength(8)]
        public string CustomerPostCode { get; set; }
        [StringLength(50)]
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
    }
}
