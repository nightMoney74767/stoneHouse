using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace J85452___CO5227_Restaurant_Project.Data
{
    public class OfferClass
    {
        // Columns for offer table; unused
        [Key]
        public int OfferID { get; set; }
        [StringLength(25)]
        public string OfferName { get; set; }
        [StringLength(100)]
        public string OfferDescription { get; set; }
        public string OfferStart { get; set; }
        public string OfferExpiry { get; set; }
        public int ReductionPercentage { get; set; }
    }
}
