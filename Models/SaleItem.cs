﻿using System;
using System.Collections.Generic;

namespace ThetaPOS.Models
{
    public partial class SaleItem
    {
        public int Id { get; set; }
        public int? ProductSaleId { get; set; }
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
    }
}
