using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ThetaPOS.Models
{
    public partial class ProductBrand
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
       
        public string Logo { get; set; }
        [Required]
        public string Website { get; set; }
        [Required]
        [MaxLength(500)]
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    }
}
