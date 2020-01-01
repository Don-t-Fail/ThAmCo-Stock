using System.Collections.Generic;
using ThAmCo.Stock.Models.Dto;

namespace ThAmCo.Stock.Models.ViewModel
{
    public class VendorProductIndexModel
    {
        public IEnumerable<VendorProductDto> Products { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Vendor { get; set; }
        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }
        public double? PriceLow { get; set; }
        public double? PriceHigh { get; set; }
    }
}
