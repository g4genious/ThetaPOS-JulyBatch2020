using System;
using System.Collections.Generic;

namespace ThetaPOS.Models
{
    public partial class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string Features { get; set; }
        public int? ProductBrandId { get; set; }
        public int? ProductCategoryId { get; set; }
        public decimal? CurrentSalePrice { get; set; }
        public decimal? LatestPurchasePrice { get; set; }
        public string Images { get; set; }
        public int? Views { get; set; }
        public decimal? OpeningStock { get; set; }
        public DateTime? OpeningDate { get; set; }
        public decimal? CurrentStock { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    }
}
