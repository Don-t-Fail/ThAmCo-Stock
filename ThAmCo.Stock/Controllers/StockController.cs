using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThAmCo.Stock.Data;

namespace ThAmCo.Stock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
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
        public async Task<ActionResult<ProductStock>> Details(int id)
        {
            var productStock = await _context.ProductStocks.FindAsync(id);

            if (productStock == null)
            {
                return NotFound();
            }

            return productStock;
        }

        // PUT: api/Stock/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductStock(int id, ProductStock productStock)
        {
            if (id != productStock.Id)
            {
                return BadRequest();
            }

            _context.Entry(productStock).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductStockExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
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
            {
                return NotFound();
            }

            _context.ProductStocks.Remove(productStock);
            await _context.SaveChangesAsync();

            return productStock;
        }

        private bool ProductStockExists(int id)
        {
            return _context.ProductStocks.Any(e => e.Id == id);
        }
    }
}
