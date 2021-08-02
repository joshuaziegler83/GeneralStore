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
    public class TransactionController : ApiController
    {
        ApplicationDbContext _context = new ApplicationDbContext();

        [HttpPost]
        public async Task<IHttpActionResult> CreateNewTransactionsAsync(Transaction model)
        {
            if (ModelState.IsValid && model != null)
            {
                if (model.Product.IsInStock && model.Product.NumberInInventory >= model.ItemCount)
                {
                    _context.Transactions.Add(model);
                    model.Product.NumberInInventory -= model.ItemCount;
                    await _context.SaveChangesAsync();
                    return Ok("New transaction created successfully!");
                }
            }
            return BadRequest(ModelState);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAllTransactionsAsync()
        {
            if (ModelState.IsValid)
            {
                List<Transaction> transactions = await _context.Transactions.ToListAsync();
                return Ok(transactions);
            }
            return BadRequest(ModelState);
        }

        [HttpGet]

        public async Task<IHttpActionResult> GetTransactionByCustomerAsync(int CustomerId)
        {
            Customer customer = await _context.Customers.FindAsync(CustomerId);

            List<Transaction> transactions = customer.Transactions.ToList();

            if (transactions is null)
            {
                return NotFound();
            }
            return Ok(transactions);
        }

        public async Task<IHttpActionResult> GetTransactionByIDAsync(int id)
        {
            Transaction transaction = await _context.Transactions.FindAsync(id);

            if (transaction is null)
            {
                return NotFound();
            }
            return Ok(transaction);
        }

        [HttpPut]

        public async Task<IHttpActionResult> UpdateTransactionAsync([FromUri] int id, [FromBody] Transaction model)
        {
            if (ModelState.IsValid)
            {
                Transaction transaction = await _context.Transactions.FindAsync(id);

                if (transaction != null)
                {
                    if (model.Product.IsInStock && model.Product.NumberInInventory >= model.ItemCount)
                    {
                        transaction.Product.NumberInInventory += transaction.ItemCount;
                        transaction.Id = model.Id;
                        transaction.CustomerId = model.CustomerId;
                        transaction.ProductSKU = model.ProductSKU;
                        transaction.ItemCount = model.ItemCount;
                        transaction.DateOfTransaction = model.DateOfTransaction;
                        transaction.Product.NumberInInventory -= model.ItemCount;
                        await _context.SaveChangesAsync();
                        return Ok("The transaction was updated successfully!");
                    }
                }
                return NotFound();
            }
            return BadRequest(ModelState);
        }

        [HttpDelete]

        public async Task<IHttpActionResult> DeleteTransactionAsync(int id)
        {
            Transaction transaction = await _context.Transactions.FindAsync(id);

            if (transaction is null)
            {
                return NotFound();
            }

            transaction.Product.NumberInInventory += transaction.ItemCount;
            _context.Transactions.Remove(transaction);

            if (await _context.SaveChangesAsync() == 1)
            {
                return Ok("The transaction was successfully removed!");
            }
            return InternalServerError();
        }
    }
}
