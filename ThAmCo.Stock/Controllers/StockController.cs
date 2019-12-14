using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThAmCo.Stock.Data;
using ThAmCo.Stock.Models.Dto;

namespace ThAmCo.Stock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : Controller
    {
        private readonly StockDbContext _context;

        public StockController(StockDbContext context)
        {
            _context = context;
        }

        // GET: api/Stock
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductStock>>> GetProductStocks()
        {
            return await _context.ProductStocks.ToListAsync();
        }

        // GET: api/Stock/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductStockDetailsDto>> Details(int id)
        {
            var productStock = await _context.ProductStocks.FindAsync(id);

            if (productStock == null)
                return NotFound();

            var productPrice = await _context.Prices.FirstOrDefaultAsync(p => p.Id == productStock.PriceId);

            var productDetails = new ProductStockDetailsDto()
            {
                ProductID = productStock.ProductId,
                Stock = productStock.Stock,
                Price = productPrice.ProductPrice
            };

            return productDetails;
        }

        [HttpGet("PriceHistory/{id}")]
        public async Task<ActionResult<ProductStockPricingHistoryDto>> PriceHistory(int id)
        {
            var productStock = await _context.ProductStocks.FindAsync(id);

            if (productStock == null)
                return NotFound();

            var productPrice = await _context.Prices.Where(p => p.ProductStockId == productStock.ProductId).ToListAsync();

            var productDetails = new ProductStockPricingHistoryDto()
            {
                ProductID = productStock.ProductId,
                Stock = productStock.Stock,
                Prices = productPrice
            };

            return productDetails;
        }

        // PUT: api/Stock/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductStock(int id, ProductStock productStock)
        {
            if (id != productStock.Id)
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
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/Stock
        [HttpPost]
        public async Task<ActionResult<ProductStock>> PostProductStock(ProductStock productStock)
        {
            _context.ProductStocks.Add(productStock);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductStock", new { id = productStock.Id }, productStock);
        }

        // DELETE: api/Stock/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductStock>> DeleteProductStock(int id)
        {
            var productStock = await _context.ProductStocks.FindAsync(id);
            if (productStock == null)
                return NotFound();

            _context.ProductStocks.Remove(productStock);
            await _context.SaveChangesAsync();

            return productStock;
        }

        [HttpGet("low/{count}")]
        public async Task<ActionResult<IEnumerable<ProductStock>>> Low(int? count)
        {
            var listToCount = await _context.ProductStocks.OrderBy(ps => ps.Stock).Take(count ?? 15).ToListAsync();

            return View(listToCount);
        }

        private bool ProductStockExists(int id)
        {
            return _context.ProductStocks.Any(e => e.Id == id);
        }
    }
}
