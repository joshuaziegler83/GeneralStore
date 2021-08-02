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
    public class CustomerController : ApiController
    {
        ApplicationDbContext _context = new ApplicationDbContext();

        [HttpPost]
        public async Task<IHttpActionResult> CreateNewCustomerAsync(Customer model)
        {
            if (ModelState.IsValid && model != null)
            {
                _context.Customers.Add(model);
                await _context.SaveChangesAsync();
                return Ok("New customer created successfully!");
            }
            return BadRequest(ModelState);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAllCustomersAsync()
        {
            if (ModelState.IsValid)
            {
                List<Customer> customers = await _context.Customers.ToListAsync();
                return Ok(customers);
            }
            return BadRequest(ModelState);
        }

        [HttpGet]

        public async Task<IHttpActionResult> GetCustomerByIDAsync(int id)
        {
            Customer customer = await _context.Customers.SingleOrDefaultAsync();

            if (customer is null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        [HttpPut]

        public async Task<IHttpActionResult> UpdateCustomerAsync([FromUri] int id, [FromBody] Customer model)
        {
            if (ModelState.IsValid)
            {
                Customer customer = await _context.Customers.FindAsync(id);

                if (customer != null)
                {
                    customer.FirstName = model.FirstName;
                    customer.LastName = model.LastName;
                    await _context.SaveChangesAsync();
                    return Ok("The customer was updated successfully!");
                }
                return NotFound();
            }
            return BadRequest(ModelState);
        }

        [HttpDelete]

        public async Task<IHttpActionResult> DeleteCustomerAsync(int id)
        {
            Customer customer = await _context.Customers.FindAsync(id);
            if (customer is null)
            {
                return NotFound();
            }
            _context.Customers.Remove(customer);
            if (await _context.SaveChangesAsync() == 1)
            {
                return Ok("The customer was successfully removed!");
            }
            return InternalServerError();
        }
    }
}
