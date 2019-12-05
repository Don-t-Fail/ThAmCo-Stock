using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThAmCo.Stock.Data;

namespace ThAmCo.Stock.Models.Dto
{
    public class ProductStockPricingHistoryDto
    {
        public int ProductID { get; set; }
        public int Stock { get; set; }
        public List<Price> Prices { get; set; }
    }
}
