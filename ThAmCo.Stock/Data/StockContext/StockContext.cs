using System;
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

        public async Task<IEnumerable<OrderRequest>> GetAllOrderRequests()
        {
            return await _context.OrderRequests.ToListAsync();
        }

        public async Task<OrderRequest> GetOrderRequest(int id)
        {
            return await _context.OrderRequests.FirstOrDefaultAsync(or => or.Id == id);
        }

        public void AddProductStockAsync()
        {
            throw new System.NotImplementedException();
        }

        public Price AddPriceAsync(Price price)
        {
            _context.Add(price);
            _context.SaveChanges();
            return price;
        }

        public void AddOrderRequest(OrderRequest order)
        {
            _context.Add(order);
            _context.SaveChanges();
        }

        public void UpdateProductStockAsync(ProductStock productStock)
        {
            _context.Update(productStock);
            SaveAndUpdateContext();
        }

        public void UpdateOrderRequest(OrderRequest orderRequest)
        {
            _context.Update(orderRequest);
            _context.SaveChanges();
        }

        public void ApproveOrderRequest(int id)
        {
            var productStock = GetOrderRequest(id).Result;
            if (productStock == null) return;
            productStock.Approved = true;
            productStock.ApprovedTime = DateTime.Now;
            _context.Update(productStock);
        }

        public void SaveAndUpdateContext()
        {
            _context.SaveChangesAsync();
        }
    }
}