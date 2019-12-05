using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Stock.Data
{
    public class Price
    {
        public int Id { get; set; }
        public int ProductStockId { get; set; }
        public double ProductPrice { get; set; }
        public DateTime Date { get; set; }
    }
}
