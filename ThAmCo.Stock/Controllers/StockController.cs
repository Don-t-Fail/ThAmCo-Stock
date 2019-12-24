using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThAmCo.Stock.Data;
using ThAmCo.Stock.Data.StockContext;
using ThAmCo.Stock.Models.Dto;
using ThAmCo.Stock.Models.ViewModel;

namespace ThAmCo.Stock.Controllers
{
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
        public async Task<ActionResult<ProductStockDto>> AdjustCost(int id)
        {
            var productPrice = await _context.GetProductStockAsync(id);

            if (productPrice == null)
                return NotFound();

            var client = GetHttpClient("StandardRequest");
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");

            var response = await client.GetAsync("https://localhost:44385/stock/");
            if (response.IsSuccessStatusCode)
            {
                var objectResult = await response.Content.ReadAsAsync<ProductDto>();
            }

                return View(new AdjustCostViewModel { Id = productPrice.ProductStock.Id, Cost = productPrice.Price.ProductPrice });
        }

        [HttpPost]
        public async Task<ActionResult> AdjustCost(int id, double cost)
        {
            var productStock = await _context.GetProductStockAsync(id);

            if (productStock == null)
                return NotFound();

            var price = _context.AddPriceAsync(new Price { ProductPrice = cost, ProductStockId = id, Date = DateTime.Now });

            productStock.ProductStock.PriceId = price.Id;
            _context.UpdateProductStockAsync(productStock.ProductStock);
            return Ok();
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
    }
}
