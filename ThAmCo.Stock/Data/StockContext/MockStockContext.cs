using System;
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
        private readonly List<OrderRequest> _orderRequests;

        public MockStockContext(List<ProductStock> productStocks, List<Price> prices, List<OrderRequest> orderRequests)
        {
            _productStocks = productStocks;
            _prices = prices;
            _orderRequests = orderRequests;
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

        public Task<IEnumerable<OrderRequest>> GetAllOrderRequests()
        {
            return Task.FromResult(_orderRequests.AsEnumerable());
        }

        public Task<OrderRequest> GetOrderRequest(int id)
        {
            return Task.FromResult(_orderRequests.FirstOrDefault(or => or.Id == id));
        }

        public void AddProductStockAsync()
        {
            throw new System.NotImplementedException();
        }

        public Price AddPriceAsync(Price price)
        {
            price.Id = _prices.OrderByDescending(p => p.Id).First().Id;
            _prices.Add(price);
            return price;
        }

        public void AddOrderRequest(OrderRequest order)
        {
            _orderRequests.Add(order);
        }

        public void UpdateProductStockAsync(ProductStock productStock)
        {
            var update = _productStocks.FirstOrDefault(p => p.Id == productStock.Id);
            if (update != null && productStock != null)
                update.Id = productStock.Id;
        }

        public void ApproveOrderRequest(int id)
        {
            var approve = _orderRequests.FirstOrDefault(or => or.Id == id);
            if (approve != null)
            {
                approve.Approved = true;
                approve.ApprovedTime = DateTime.Now;
            }
        }

        public void SaveAndUpdateContext()
        {
            throw new System.NotImplementedException();
        }
    }
}