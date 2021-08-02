using GeneralStore.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GeneralStore.WebAPI.Controllers
{
    public class ProductController : ApiController
    {
        ApplicationDbContext _context = new ApplicationDbContext();

        [HttpPost]
        public async Task<IHttpActionResult> CreateNewProductAsync(Product model)
        {
            if (ModelState.IsValid && model != null)
            {
                _context.Products.Add(model);
                await _context.SaveChangesAsync();
                return Ok("New product created successfully!");
            }
            return BadRequest(ModelState);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAllProductsAsync()
        {
            if (ModelState.IsValid)
            {
                List<Product> products = await _context.Products.ToListAsync();
                return Ok(products);
            }
            return BadRequest(ModelState);
        }

        [HttpGet]

        public async Task<IHttpActionResult> GetProductByIDAsync(int id)
        {
            Product product = await _context.Products.SingleOrDefaultAsync();

            if (product is null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPut]

        public async Task<IHttpActionResult> UpdateProductAsync([FromUri] int id, [FromBody] Product model)
        {
            if (ModelState.IsValid)
            {
                Product product = await _context.Products.FindAsync(id);

                if (product != null)
                {
                    product.SKU = model.SKU;
                    product.Name = model.Name;
                    product.Cost = model.Cost;
                    product.NumberInInventory = model.NumberInInventory;
                    await _context.SaveChangesAsync();
                    return Ok("The product was updated successfully!");
                }
                return NotFound();
            }
            return BadRequest(ModelState);
        }

        [HttpDelete]

        public async Task<IHttpActionResult> DeleteProductAsync(int id)
        {
            Product product = await _context.Products.FindAsync(id);
            if (product is null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            if (await _context.SaveChangesAsync() == 1)
            {
                return Ok("The product was successfully removed!");
            }
            return InternalServerError();

        }
    }
}
