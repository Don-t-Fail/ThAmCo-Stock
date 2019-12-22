using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThAmCo.Stock.Data;
using ThAmCo.Stock.Data.StockContext;
using ThAmCo.Stock.Models.Dto;

namespace ThAmCo.Stock.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StockController : Controller
    {
        private readonly IStockContext _context;
        private readonly IHttpClientFactory _clientFactory;

        public StockController(IStockContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _clientFactory = httpClientFactory;
        }

        // GET: api/Stock
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductStockDto>>> GetProductStock()
        {
            return Ok(await _context.GetAll());
        }

        // GET: api/Stock/5
        [HttpGet("{id}")]
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

        [HttpGet("PriceHistory/{id}")]
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

        // PUT: api/Stock/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductStock(int id, ProductStock productStock)
        {
            throw new NotImplementedException();
            /*if (id != productStock.Id)
                return BadRequest();

            _context.Entry(productStock).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductStockExists(id))
                    return NotFound();
                throw;
            }

            return NoContent();*/
        }

        // POST: api/Stock
        [HttpPost]
        public async Task<ActionResult<ProductStock>> PostProductStock(ProductStock productStock)
        {
            throw new NotImplementedException();
            /*_context.ProductStocks.Add(productStock);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductStock", new { id = productStock.Id }, productStock);*/
        }

        // DELETE: api/Stock/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductStock>> DeleteProductStock(int id)
        {
            throw new NotImplementedException();
            /*var productStock = await _context.ProductStocks.FindAsync(id);
            if (productStock == null)
                return NotFound();

            _context.ProductStocks.Remove(productStock);
            await _context.SaveChangesAsync();

            return productStock;*/
        }

        [HttpGet("low/{count}")]
        public async Task<ActionResult<IEnumerable<ProductStock>>> Low(int? count)
        {
            var listToCount = _context.GetAll().Result.OrderBy(ps => ps.ProductStock.Stock).Take(count ?? 15).ToList();

            return View(listToCount);
        }

        private bool ProductStockExists(int id)
        {
            return _context.GetAll().Result.Any(e => e.ProductStock.Id == id);
        }
    }
}
