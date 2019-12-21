using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ThAmCo.Stock.Controllers;
using ThAmCo.Stock.Data;
using ThAmCo.Stock.Data.StockContext;
using ThAmCo.Stock.Models.Dto;

namespace ThAmCo.Stock.Tests.Controllers
{
    [TestClass]
    public class StockControllerTests
    {
        private static class Data
        {
            public static List<ProductStock> ProductStocks() => new List<ProductStock>
            {
                new ProductStock { Id = 1, Stock = 10, PriceId = 1, ProductId = 436 },
                new ProductStock { Id = 2, Stock = 0, PriceId = 3, ProductId = 326 }
            };
            
            public static List<Price> Prices() => new List<Price>
            {
                new Price { Id = 1, ProductStockId = 1, ProductPrice = 8.99 },
                new Price { Id = 2, ProductStockId = 2, ProductPrice = 24.99 },
                new Price { Id = 3, ProductStockId = 2, ProductPrice = 19.99 }
            };
        }
        
        [TestMethod]
        public async Task GetAll_AllValid_AllReturned()
        {
            var context = new MockStockContext(Data.ProductStocks(), Data.Prices());
            var controller = new StockController(context, null);

            var result = await controller.GetProductStock();
            
            Assert.IsNotNull(result);
            var objectResult = result.Result as OkObjectResult;
            Assert.IsNotNull(objectResult);
            var enumerableResult = objectResult.Value as IEnumerable<ProductStockDto>;
            Assert.IsNotNull(enumerableResult);
            var listResult = enumerableResult.ToList();
            foreach (var p in listResult)
            {
                Assert.AreEqual(p.Price.Date, Data.Prices()[p.Price.Id - 1].Date);
                Assert.AreEqual(p.Price.Id, Data.Prices()[p.Price.Id - 1].Id);
                Assert.AreEqual(p.Price.ProductPrice, Data.Prices()[p.Price.Id - 1].ProductPrice);
                Assert.AreEqual(p.Price.ProductStockId, Data.Prices()[p.Price.Id - 1].ProductStockId);
                
                Assert.AreEqual(p.ProductStock.Id, Data.ProductStocks()[p.ProductStock.Id - 1].Id);
                Assert.AreEqual(p.ProductStock.Stock, Data.ProductStocks()[p.ProductStock.Id - 1].Stock);
                Assert.AreEqual(p.ProductStock.PriceId, Data.ProductStocks()[p.ProductStock.Id - 1].PriceId);
                Assert.AreEqual(p.ProductStock.ProductId, Data.ProductStocks()[p.ProductStock.Id - 1].ProductId);
            }
        }

        [TestMethod]
        public async Task GetAll_EmptyModel_EmptyListReturned()
        {
            var context = new MockStockContext(new List<ProductStock>(), new List<Price>());
            var controller = new StockController(context, null);

            var result = await controller.GetProductStock();
            
            Assert.IsNotNull(result);
            var objectResult = result.Result as OkObjectResult;
            Assert.IsNotNull(objectResult);
            var enumerableResult = objectResult.Value as IEnumerable<ProductStockDto>;
            Assert.IsNotNull(enumerableResult);
            var listResult = enumerableResult.ToList();
            Assert.AreEqual(listResult.Count, 0);
        }
    }
}
