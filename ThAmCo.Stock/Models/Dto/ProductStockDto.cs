using ThAmCo.Stock.Data;

namespace ThAmCo.Stock.Models.Dto
{
    public class ProductStockDto
    {
        public ProductStock ProductStock { get; set; }
        public Price Price { get; set; }
    }
}