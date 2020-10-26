using System;
using System.Collections.Generic;

namespace ThetaPOS.Models
{
    public partial class ProductPurchase
    {
        public int Id { get; set; }
         public int ItemId { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? Discount { get; set; }
        public decimal? FinalPrice { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    }
}
