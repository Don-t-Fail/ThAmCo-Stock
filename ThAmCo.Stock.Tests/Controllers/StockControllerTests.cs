using System.Collections;
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
                new ProductStock { Id = 2, Stock = 0, PriceId = 3, ProductId = 326 },
                new ProductStock { Id = 3, Stock = 4, PriceId = 4, ProductId = 273 },
                new ProductStock { Id = 4, Stock = 43, PriceId = 6, ProductId = 456 },
                new ProductStock { Id = 5, Stock = 2, PriceId = 7, ProductId = 32 },
                new ProductStock { Id = 6, Stock = 14, PriceId = 8, ProductId = 33 },
                new ProductStock { Id = 7, Stock = 1, PriceId = 9, ProductId = 21 }
            };
            
            public static List<Price> Prices() => new List<Price>
            {
                new Price { Id = 1, ProductStockId = 1, ProductPrice = 8.99 },
                new Price { Id = 2, ProductStockId = 2, ProductPrice = 24.99 },
                new Price { Id = 3, ProductStockId = 2, ProductPrice = 19.99 },
                new Price { Id = 4, ProductStockId = 3, ProductPrice = 54.56 },
                new Price { Id = 5, ProductStockId = 4, ProductPrice = 73.00 },
                new Price { Id = 6, ProductStockId = 4, ProductPrice = 29.99 },
                new Price { Id = 7, ProductStockId = 5, ProductPrice = 13.65 },
                new Price { Id = 8, ProductStockId = 6, ProductPrice = 3.99 },
                new Price { Id = 9, ProductStockId = 7, ProductPrice = 5.67}
            };
            
            public static List<ProductStockDto> ProductStockDtos() => new List<ProductStockDto>
            {
                new ProductStockDto { ProductStock = ProductStocks()[0], Price = Prices()[0] },
                new ProductStockDto { ProductStock = ProductStocks()[1], Price = Prices()[2] },
                new ProductStockDto { ProductStock = ProductStocks()[2], Price = Prices()[3] },
                new ProductStockDto { ProductStock = ProductStocks()[3], Price = Prices()[5] },
                new ProductStockDto { ProductStock = ProductStocks()[4], Price = Prices()[6] },
                new ProductStockDto { ProductStock = ProductStocks()[5], Price = Prices()[7] },
                new ProductStockDto { ProductStock = ProductStocks()[6], Price = Prices()[8] }
            };
        }
        
        private const int OutOfBoundsId = 8;
        private const int NegativeId = -1;
        
        [TestMethod]
        public async Task GetAll_AllValid_AllReturned()
        {
            var context = new MockStockContext(Data.ProductStocks(), Data.Prices());
            var controller = new StockController(context, null);

            var result = await controller.ProductStocks();
            
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

            var result = await controller.ProductStocks();
            
            Assert.IsNotNull(result);
            var objectResult = result.Result as OkObjectResult;
            Assert.IsNotNull(objectResult);
            var enumerableResult = objectResult.Value as IEnumerable<ProductStockDto>;
            Assert.IsNotNull(enumerableResult);
            var listResult = enumerableResult.ToList();
            Assert.AreEqual(listResult.Count, 0);
        }

        [TestMethod]
        public async Task GetDetails_ValidIdPassed_CorrectAndValidObjectReturned()
        {
            var context = new MockStockContext(Data.ProductStocks(), Data.Prices());
            var controller = new StockController(context, null);
            const int id = 2;

            var result = await controller.Details(id);
            
            Assert.IsNotNull(result);
            var objectResult = result.Result as OkObjectResult;
            Assert.IsNotNull(objectResult);
            var returnedResult = objectResult.Value as ProductStockDetailsDto;
            Assert.IsNotNull(returnedResult);
            
            Assert.AreEqual(returnedResult.ProductID, Data.ProductStocks()[id - 1].ProductId);
            Assert.AreEqual(returnedResult.Price, Data.Prices().FirstOrDefault( p => p.Id == Data.ProductStocks()[id - 1].PriceId)?.ProductPrice);
            Assert.AreEqual(returnedResult.Stock, Data.ProductStocks()[id - 1].Stock);
        }
        
        [TestMethod]
        public async Task GetDetails_OutOfBoundsPositiveId_NotFoundReturned()
        {
            var context = new MockStockContext(Data.ProductStocks(), Data.Prices());
            var controller = new StockController(context, null);
            const int id = OutOfBoundsId;

            var result = await controller.Details(id);
            
            Assert.IsNotNull(result);
            var objectResult = result.Result as NotFoundResult;
            Assert.IsNotNull(objectResult);
        }
        
        [TestMethod]
        public async Task GetDetails_OutOfBoundsNegativeId_NotFoundReturned()
        {
            var context = new MockStockContext(Data.ProductStocks(), Data.Prices());
            var controller = new StockController(context, null);
            const int id = NegativeId;

            var result = await controller.Details(id);
            
            Assert.IsNotNull(result);
            var objectResult = result.Result as NotFoundResult;
            Assert.IsNotNull(objectResult);
        }

        [TestMethod]
        public async Task PriceHistory_ValidIdPassed_CorrectAndValidObjectReturned()
        {
            var context = new MockStockContext(Data.ProductStocks(), Data.Prices());
            var controller = new StockController(context, null);
            const int id = 1;

            var result = await controller.PriceHistory(id);
            var validPrices = Data.Prices().Where(p => p.ProductStockId == id).ToList();
            
            Assert.IsNotNull(result);
            var objectResult = result.Result as OkObjectResult;
            Assert.IsNotNull(objectResult);
            var returnedResult = objectResult.Value as ProductStockPricingHistoryDto;
            Assert.IsNotNull(returnedResult);
            
            Assert.AreEqual(returnedResult.ProductID, Data.ProductStocks()[id - 1].ProductId);
            Assert.AreEqual(returnedResult.Stock, Data.ProductStocks()[id - 1].Stock);
            Assert.AreEqual(returnedResult.Prices.Count, validPrices.Count);
            foreach (var prices in returnedResult.Prices)
            {
                Assert.AreEqual(prices.Id, validPrices.FirstOrDefault(p => p.Id == prices.Id)?.Id);
                Assert.AreEqual(prices.Date, validPrices.FirstOrDefault(p => p.Id == prices.Id)?.Date);
                Assert.AreEqual(prices.ProductPrice, validPrices.FirstOrDefault(p => p.Id == prices.Id)?.ProductPrice);
                Assert.AreEqual(prices.ProductStockId, validPrices.FirstOrDefault(p => p.Id == prices.Id)?.ProductStockId);
            }
        }

        [TestMethod]
        public async Task PriceHistory_OutOfBoundsPositiveId_NotFoundReturned()
        {
            var context = new MockStockContext(Data.ProductStocks(), Data.Prices());
            var controller = new StockController(context, null);
            const int id = OutOfBoundsId;

            var result = await controller.PriceHistory(id);

            Assert.IsNotNull(result);
            var objectResult = result.Result as NotFoundResult;
            Assert.IsNotNull(objectResult);
        }
        
        [TestMethod]
        public async Task PriceHistory_OutOfBoundsNegativeId_NotFoundReturned()
        {
            var context = new MockStockContext(Data.ProductStocks(), Data.Prices());
            var controller = new StockController(context, null);
            const int id = NegativeId;

            var result = await controller.PriceHistory(id);

            Assert.IsNotNull(result);
            var objectResult = result.Result as NotFoundResult;
            Assert.IsNotNull(objectResult);
        }

        [TestMethod]
        public void Low_ValidCountPassedLowerThanDefault_CorrectAndValidObjectReturned()
        {
            var context = new MockStockContext(Data.ProductStocks(), Data.Prices());
            var controller = new StockController(context, null);
            const int count = 4;

            var result = controller.Low(count);
            var expectedResult = Data.ProductStockDtos().OrderBy(ps => ps.ProductStock.Stock).Take(count).ToList();
            
            Assert.IsNotNull(result);
            var priceResult = result.Result as ViewResult;
            Assert.IsNotNull(priceResult);
            var objectResult = priceResult.Model as IEnumerable<ProductStockDto>;
            Assert.IsNotNull(objectResult);
            var returnedListResult = objectResult.ToList();
            Assert.AreEqual(returnedListResult.Count, count);

            for (int i = 0; i < count; i++)
            {
                Assert.AreEqual(returnedListResult[i].Price.Date, expectedResult[i].Price.Date);
                Assert.AreEqual(returnedListResult[i].Price.Id, expectedResult[i].Price.Id);
                Assert.AreEqual(returnedListResult[i].Price.ProductPrice, expectedResult[i].Price.ProductPrice);
                Assert.AreEqual(returnedListResult[i].Price.ProductStockId, expectedResult[i].Price.ProductStockId);
                
                Assert.AreEqual(returnedListResult[i].ProductStock.Id, expectedResult[i].ProductStock.Id);
                Assert.AreEqual(returnedListResult[i].ProductStock.Stock, expectedResult[i].ProductStock.Stock);
                Assert.AreEqual(returnedListResult[i].ProductStock.PriceId, expectedResult[i].ProductStock.PriceId);
                Assert.AreEqual(returnedListResult[i].ProductStock.ProductId, expectedResult[i].ProductStock.ProductId);
            }
        }
        
        [TestMethod]
        public void Low_ValidCountPassedHigherThanDefault_CorrectAndValidObjectReturned()
        {
            var context = new MockStockContext(Data.ProductStocks(), Data.Prices());
            var controller = new StockController(context, null);
            const int count = 6;

            var result = controller.Low(count);
            var expectedResult = Data.ProductStockDtos().OrderBy(ps => ps.ProductStock.Stock).Take(count).ToList();
            
            Assert.IsNotNull(result);
            var priceResult = result.Result as ViewResult;
            Assert.IsNotNull(priceResult);
            var objectResult = priceResult.Model as IEnumerable<ProductStockDto>;
            Assert.IsNotNull(objectResult);
            var returnedListResult = objectResult.ToList();
            Assert.AreEqual(returnedListResult.Count, count);

            for (int i = 0; i < count; i++)
            {
                Assert.AreEqual(returnedListResult[i].Price.Date, expectedResult[i].Price.Date);
                Assert.AreEqual(returnedListResult[i].Price.Id, expectedResult[i].Price.Id);
                Assert.AreEqual(returnedListResult[i].Price.ProductPrice, expectedResult[i].Price.ProductPrice);
                Assert.AreEqual(returnedListResult[i].Price.ProductStockId, expectedResult[i].Price.ProductStockId);
                
                Assert.AreEqual(returnedListResult[i].ProductStock.Id, expectedResult[i].ProductStock.Id);
                Assert.AreEqual(returnedListResult[i].ProductStock.Stock, expectedResult[i].ProductStock.Stock);
                Assert.AreEqual(returnedListResult[i].ProductStock.PriceId, expectedResult[i].ProductStock.PriceId);
                Assert.AreEqual(returnedListResult[i].ProductStock.ProductId, expectedResult[i].ProductStock.ProductId);
            }
        }
        
        [TestMethod]
        public void Low_NullValuePassed_DefaultAmountCorrectAndValidObjectsReturned()
        {
            var context = new MockStockContext(Data.ProductStocks(), Data.Prices());
            var controller = new StockController(context, null);
            const int count = 5;

            var result = controller.Low(null);
            var expectedResult = Data.ProductStockDtos().OrderBy(ps => ps.ProductStock.Stock).Take(count).ToList();
            
            Assert.IsNotNull(result);
            var priceResult = result.Result as ViewResult;
            Assert.IsNotNull(priceResult);
            var objectResult = priceResult.Model as IEnumerable<ProductStockDto>;
            Assert.IsNotNull(objectResult);
            var returnedListResult = objectResult.ToList();
            Assert.AreEqual(returnedListResult.Count, count);

            for (int i = 0; i < count; i++)
            {
                Assert.AreEqual(returnedListResult[i].Price.Date, expectedResult[i].Price.Date);
                Assert.AreEqual(returnedListResult[i].Price.Id, expectedResult[i].Price.Id);
                Assert.AreEqual(returnedListResult[i].Price.ProductPrice, expectedResult[i].Price.ProductPrice);
                Assert.AreEqual(returnedListResult[i].Price.ProductStockId, expectedResult[i].Price.ProductStockId);
                
                Assert.AreEqual(returnedListResult[i].ProductStock.Id, expectedResult[i].ProductStock.Id);
                Assert.AreEqual(returnedListResult[i].ProductStock.Stock, expectedResult[i].ProductStock.Stock);
                Assert.AreEqual(returnedListResult[i].ProductStock.PriceId, expectedResult[i].ProductStock.PriceId);
                Assert.AreEqual(returnedListResult[i].ProductStock.ProductId, expectedResult[i].ProductStock.ProductId);
            }
        }
        
        [TestMethod]
        public void Low_OutOfBoundsCountPassed_AllValuesReturnedCorrectly()
        {
            var context = new MockStockContext(Data.ProductStocks(), Data.Prices());
            var controller = new StockController(context, null);
            const int count = OutOfBoundsId;

            var result = controller.Low(OutOfBoundsId);
            var expectedResult = Data.ProductStockDtos().OrderBy(ps => ps.ProductStock.Stock).Take(count).ToList();
            
            Assert.IsNotNull(result);
            var priceResult = result.Result as ViewResult;
            Assert.IsNotNull(priceResult);
            var objectResult = priceResult.Model as IEnumerable<ProductStockDto>;
            Assert.IsNotNull(objectResult);
            var returnedListResult = objectResult.ToList();
            Assert.AreEqual(returnedListResult.Count, expectedResult.Count);

            for (int i = 0; i < expectedResult.Count; i++)
            {
                Assert.AreEqual(returnedListResult[i].Price.Date, expectedResult[i].Price.Date);
                Assert.AreEqual(returnedListResult[i].Price.Id, expectedResult[i].Price.Id);
                Assert.AreEqual(returnedListResult[i].Price.ProductPrice, expectedResult[i].Price.ProductPrice);
                Assert.AreEqual(returnedListResult[i].Price.ProductStockId, expectedResult[i].Price.ProductStockId);
                
                Assert.AreEqual(returnedListResult[i].ProductStock.Id, expectedResult[i].ProductStock.Id);
                Assert.AreEqual(returnedListResult[i].ProductStock.Stock, expectedResult[i].ProductStock.Stock);
                Assert.AreEqual(returnedListResult[i].ProductStock.PriceId, expectedResult[i].ProductStock.PriceId);
                Assert.AreEqual(returnedListResult[i].ProductStock.ProductId, expectedResult[i].ProductStock.ProductId);
            }
        }

        [TestMethod]
        public void Low_OutOfBoundsNegativePassed_NotFoundReturned()
        {
            var context = new MockStockContext(Data.ProductStocks(), Data.Prices());
            var controller = new StockController(context, null);
            const int count = NegativeId;

            var result = controller.Low(count);

            Assert.IsNotNull(result);
            var priceResult = result.Result as NotFoundResult;
            Assert.IsNotNull(priceResult);
        }
    }
}
