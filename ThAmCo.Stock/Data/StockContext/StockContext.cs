using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ThAmCo.Stock.Models.Dto;

namespace ThAmCo.Stock.Data.StockContext
{
    public class StockContext : IStockContext
    {
        private readonly StockDbContext _context;

        public StockContext(StockDbContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<ProductStockDto>> GetAll()
        {
            var productStocks = new List<ProductStockDto>();
            //Janky Database handling; Not linked so have to do this...
            foreach (var p in _context.ProductStocks)
            {
                productStocks.Add(new ProductStockDto
                {
                    ProductStock = p,
                    Price = await _context.Prices.FirstOrDefaultAsync(price => price.Id == p.PriceId)
                });
            }

            return productStocks;
        }

        public async Task<IEnumerable<Price>> GetAllPrices()
        {
            return await _context.Prices.ToListAsync();
        }

        public async Task<ProductStockDto> GetProductStockAsync(int id)
        {
            var prod = await _context.ProductStocks.FirstOrDefaultAsync(p => p.Id == id);
            return new ProductStockDto
            {
                ProductStock = prod,
                Price = await _context.Prices.FirstOrDefaultAsync(p => p.Id == prod.PriceId)
            };
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