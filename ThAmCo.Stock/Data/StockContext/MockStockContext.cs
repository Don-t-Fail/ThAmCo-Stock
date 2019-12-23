using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThAmCo.Stock.Models.Dto;

namespace ThAmCo.Stock.Data.StockContext
{
    public class MockStockContext : IStockContext
    {
        private readonly List<ProductStock> _productStocks;
        private readonly List<Price> _prices;

        public MockStockContext(List<ProductStock> productStocks, List<Price> prices)
        {
            _productStocks = productStocks;
            _prices = prices;
        }
        
        public Task<IEnumerable<ProductStockDto>> GetAll()
        {
            var productStocks = new List<ProductStockDto>();
            //Janky List handling; Not linked so have to do this...
            foreach (var p in _productStocks)
            {
                productStocks.Add(new ProductStockDto
                {
                    ProductStock = p,
                    Price = _prices.FirstOrDefault(price => price.Id == p.PriceId)
                });
            }

            return Task.FromResult(productStocks.AsEnumerable());
        }
        
        public Task<IEnumerable<Price>> GetAllPrices()
        {
            return Task.FromResult(_prices.AsEnumerable());
        }

        public Task<ProductStockDto> GetProductStockAsync(int id)
        {
            var prod = _productStocks.FirstOrDefault(p => p.Id == id);
            
            if (prod == null)
                return Task.FromResult<ProductStockDto>(null);
            
            return Task.FromResult(new ProductStockDto
            {
                ProductStock = prod,
                Price = _prices.FirstOrDefault(p => p.Id == prod.PriceId)
            });
        }

        public void AddProductStockAsync()
        {
            throw new System.NotImplementedException();
        }

        public void SaveAndUpdateContext()
        {
            throw new System.NotImplementedException();
        }
    }
}