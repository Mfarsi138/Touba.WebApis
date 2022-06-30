using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Touba.WebApis.DataLayer.Models.Product;
using Touba.WebApis.IdentityServer.DataLayer;

namespace Touba.WebApis.Api.Controllers
{
    [ApiController]
    [Route("ToubaWebApis/api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [EnableCors("_allowedOrigins")]
    public class ProductTestsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ILogger logger;

        public ProductTestsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/ProductTests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductTest>>> GetProductTest()
        {
            return await _context.ProductTest.ToListAsync();
        }


        // GET: api/ProductTests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductTest>> GetProductTest(string id)
        {
            var productTest = await _context.ProductTest.FindAsync(id);

            if (productTest == null)
            {
                return NotFound();
            }

            return productTest;
        }

        // PUT: api/ProductTests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductTest(string id, ProductTest productTest)
        {
            if (id != productTest.Id)
            {
                return BadRequest();
            }

            _context.Entry(productTest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductTestExists(id))
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

        // POST: api/ProductTests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductTest>> PostProductTest(ProductTest productTest)
        {
            _context.ProductTest.Add(productTest);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProductTestExists(productTest.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetProductTest", new { id = productTest.Id }, productTest);
        }

        // DELETE: api/ProductTests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductTest(string id)
        {
            var productTest = await _context.ProductTest.FindAsync(id);
            if (productTest == null)
            {
                return NotFound();
            }

            _context.ProductTest.Remove(productTest);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductTestExists(string id)
        {
            return _context.ProductTest.Any(e => e.Id == id);
        }
    }
}
