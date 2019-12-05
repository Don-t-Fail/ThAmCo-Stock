using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Stock.Models.Dto
{
    public class ProductStockDetailsDto
    {
        public int ProductID { get; set; }
        public int Stock { get; set; }
        public double Price { get; set; }
    }
}
