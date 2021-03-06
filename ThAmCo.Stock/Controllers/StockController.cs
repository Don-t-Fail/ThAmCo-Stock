using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ThAmCo.Stock.Data;
using ThAmCo.Stock.Data.StockContext;
using ThAmCo.Stock.Models.Dto;
using ThAmCo.Stock.Models.ViewModel;

namespace ThAmCo.Stock.Controllers
{
    [Authorize(Policy = "StaffOnly")]
    public class StockController : Controller
    {
        private readonly IStockContext _context;
        private readonly IHttpClientFactory _clientFactory;

        public HttpClient HttpClient { get; set; }

        public StockController(IStockContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _clientFactory = httpClientFactory;
        }

        // GET: Stock
        public async Task<IActionResult> Index()
        {
            return View(_context.GetAll().Result.ToList());
        }

        [AllowAnonymous]
        // GET: Stock/Details/5
        public async Task<ActionResult<ProductStockDetailsDto>> Details(int id)
        {
            if (id < 0)
                return NotFound();

            var productStock = await _context.GetProductStockAsync(id);

            if (productStock == null)
                return NotFound();

            return Ok(new ProductStockDetailsDto
            {
                ProductID = productStock.ProductStock.ProductId,
                Stock = productStock.ProductStock.Stock,
                Price = productStock.Price.ProductPrice
            });
        }

        // GET: Stock/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stock/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductId,Stock,PriceId")] ProductStock productStock)
        {
            if (ModelState.IsValid)
            {
                //_context.AddProductStockAsync(productStock);
                return RedirectToAction(nameof(Index));
            }
            return View(productStock);
        }

        // GET: Stock/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productStock = await _context.GetProductStockAsync(id ?? 1);
            if (productStock == null)
            {
                return NotFound();
            }
            return View(productStock);
        }

        // POST: Stock/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,Stock,PriceId")] ProductStock productStock)
        {
            if (id != productStock.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.UpdateProductStockAsync(productStock);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductStockExists(productStock.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(productStock);
        }

        // GET: Stock/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productStock = await _context.GetProductStockAsync(id ?? 1);
            if (productStock == null)
            {
                return NotFound();
            }

            return View(productStock);
        }

        // POST: Stock/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            /*var productStock = await _context.ProductStocks.FindAsync(id);
            _context.ProductStocks.Remove(productStock);
            await _context.SaveChangesAsync();*/
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ProductStockDto>>> ProductStocks()
        {
            return Ok(await _context.GetAll());
        }

        public async Task<ActionResult<ProductStockPricingHistoryDto>> PriceHistory(int id)
        {
            if (id < 0)
                return NotFound();

            var productStock = await _context.GetProductStockAsync(id);

            if (productStock == null)
                return NotFound();

            var productPrices = _context.GetAllPrices().Result.Where(p => p.ProductStockId == productStock.ProductStock.PriceId).ToList();

            var productDetails = new ProductStockPricingHistoryDto
            {
                ProductID = productStock.ProductStock.ProductId,
                Stock = productStock.ProductStock.Stock,
                Prices = productPrices
            };

            return Ok(productDetails);
        }

        public ActionResult<IEnumerable<ProductStockDto>> Low(int? count)
        {
            if (count != null && count < 0)
                return NotFound(); //0 is technically valid, since the request would want 0. below 0 would be an invalid request.

            var listToCount = _context.GetAll().Result.OrderBy(ps => ps.ProductStock.Stock).Take(count ?? 5).ToList();

            return View(listToCount);
        }

        [HttpGet]
        public async Task<ActionResult<AdjustCostViewModel>> AdjustCost(int id)
        {
            var productPrice = await _context.GetProductStockAsync(id);

            if (productPrice == null)
                return NotFound();

            AdjustCostViewModel objectResponse;
            var client = GetHttpClient("StandardRequest");
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");

            var response = await client.GetAsync("https://localhost:44375/products/getProduct/" + id);
            if (response.IsSuccessStatusCode)
            {
                var objectResult = await response.Content.ReadAsAsync<ProductDto>();
                if (objectResult == null)
                    return NotFound();
                objectResponse = new AdjustCostViewModel
                {
                    Id = productPrice.ProductStock.Id,
                    Cost = productPrice.Price.ProductPrice,
                    Name = objectResult.Name,
                    Description = objectResult.Description
                };
            }
            else return NotFound();

            return View(objectResponse);
        }

        [HttpPost]
        public async Task<ActionResult> AdjustCost(int id, double cost)
        {
            var productStock = await _context.GetProductStockAsync(id);

            if (productStock == null)
                return NotFound();
            if (cost <= 0)
                return BadRequest();

            var price = _context.AddPriceAsync(new Price { ProductPrice = cost, ProductStockId = id, Date = DateTime.Now });

            productStock.ProductStock.PriceId = price.Id;
            _context.UpdateProductStockAsync(productStock.ProductStock);
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult> VendorProducts(string supplier)
        {
            var vendorProducts = new VendorProductIndexModel
            {
                Vendor = supplier
            };

            HttpResponseMessage response = null;

            var url = GetURLForSupplier(supplier);
            if (url == null)
                return NotFound();

            var client = GetHttpClient("StandardRequest");
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");

            response = await client.GetAsync(url + "product");

            if (response?.IsSuccessStatusCode == true)
            {
                var objectResult = await response.Content.ReadAsAsync<List<VendorProductDto>>();
                if (objectResult == null)
                    goto View;
                vendorProducts.Products = objectResult;
            }
            else return NotFound();

            View:
            return View(vendorProducts);
        }

        [HttpGet]
        public async Task<ActionResult> OrderRequest(int id, string supplier = null)
        {
            if (id <= 0)
                return NotFound();

            var url = GetURLForSupplier(supplier);
            if (url == null)
                return NotFound();

            var client = GetHttpClient("StandardRequest");
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");

            HttpResponseMessage response = null;
            response = await client.GetAsync(url + "product/" + id);

            if (response?.IsSuccessStatusCode == true)
            {
                var objectResult = await response.Content.ReadAsAsync<VendorProductDto>();

                if (objectResult == null)
                    return NotFound();
                if (objectResult.Id != id)
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);

                return View(new OrderRequestModel { Id = id, Name = objectResult.Name, Description = objectResult.Description, Supplier = supplier });
            }
            else return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> OrderRequestSubmitted(int id, string supplier, int quantity)
        {
            var orderRequest = new OrderRequest
            {
                Quantity = quantity,
                Supplier = supplier,
                SubmittedTime = DateTime.Now,
                Approved = false,
                ApprovedTime = null,
                Deleted = false
            };

            var client = GetHttpClient("StandardRequest");
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");

            var url = GetURLForSupplier(supplier);
            if (url == null)
                return NotFound();

            HttpResponseMessage response = null;

            response = await client.GetAsync(url + "product/" + id);

            if (response?.IsSuccessStatusCode == true)
            {
                var objectResult = await response.Content.ReadAsAsync<VendorProductDto>();
                if (objectResult == null)
                    return NotFound();
                orderRequest.ProductId = objectResult.Id;
                orderRequest.Price = objectResult.Price * quantity;

                _context.AddOrderRequest(orderRequest);

                return RedirectToAction(nameof(VendorProducts), new { supplier });
            }
            else return NotFound();
        }

        [HttpGet]
        public async Task<ActionResult> OrderRequests()
        {
            return View(_context.GetAllOrderRequests().Result.Where(or => !or.Approved).ToList());
        }

        [HttpGet]
        public async Task<ActionResult> OrderRequestReview(int id)
        {
            var orderRequest = _context.GetOrderRequest(id).Result;
            if (orderRequest == null)
                return NotFound();

            var client = GetHttpClient("StandardRequest");
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");

            var url = GetURLForSupplier(orderRequest.Supplier);
            if (url == null)
                return NotFound();

            HttpResponseMessage response = null;

            response = await client.GetAsync(url + "product/" + orderRequest.ProductId);

            if (response?.IsSuccessStatusCode == true)
            {
                var objectResult = await response.Content.ReadAsAsync<VendorProductDto>();
                if (objectResult == null)
                    return NotFound();

                //Update order details incase service changes prices.
                if (orderRequest.Price != objectResult.Price * orderRequest.Quantity)
                {
                    orderRequest.Price = objectResult.Price * orderRequest.Quantity;
                    _context.UpdateOrderRequest(orderRequest);
                }

                var reviewModel = new OrderRequestReviewModel
                {
                    Id = orderRequest.Id,
                    Name = objectResult.Name,
                    Description = objectResult.Description,
                    Price = orderRequest.Price,
                    Quantity = orderRequest.Quantity,
                    Supplier = orderRequest.Supplier
                };

                return View(reviewModel);
            }
            else return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> OrderRequestApproved(int id)
        {
            var orderRequest = _context.GetOrderRequest(id).Result;
            if (orderRequest == null)
                return NotFound();
            var orderDto = new ProductOrderPostDto
            {
                AccountName = "sample string 1",
                CardNumber = "sample string 2",
                ProductId = orderRequest.ProductId,
                Quantity = orderRequest.Quantity
            };

            var client = GetHttpClient("StandardRequest");
            var result = await client.PostAsJsonAsync(GetURLForSupplier(orderRequest.Supplier) + "order", orderDto);
            if (result.IsSuccessStatusCode)
            {
                _context.ApproveOrderRequest(id);
                return Ok(result.Content);
                //return RedirectToAction(nameof(OrderRequests));
            }
            return NotFound();
        }

        private bool ProductStockExists(int id)
        {
            return _context.GetAll().Result.Any(e => e.ProductStock.Id == id);
        }

        private HttpClient GetHttpClient(string s)
        {
            if (_clientFactory == null && HttpClient != null) return HttpClient;

            return _clientFactory.CreateClient(s);
        }

        private string GetURLForSupplier(string s)
        {
            if (s == "undercutters") return "http://undercutters.azurewebsites.net/api/";
            else if (s == "dodgydealers") return "http://dodgydealers.azurewebsites.net/api/";
            else return null;
        }
    }
}
