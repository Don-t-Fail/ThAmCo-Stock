using System.Collections.Generic;
using System.Threading.Tasks;
using ThAmCo.Stock.Models.Dto;

namespace ThAmCo.Stock.Data.StockContext
{
    public interface IStockContext
    {
        Task<IEnumerable<ProductStockDto>> GetAll();
        Task<IEnumerable<Price>> GetAllPrices();
        Task<ProductStockDto> GetProductStockAsync(int id);
        Task<IEnumerable<OrderRequest>> GetAllOrderRequests();
        Task<OrderRequest> GetOrderRequest(int id);
        void AddProductStockAsync();
        Price AddPriceAsync(Price price);
        void AddOrderRequest(OrderRequest order);
        void UpdateProductStockAsync(ProductStock productStock);
        void SaveAndUpdateContext();
    }
}