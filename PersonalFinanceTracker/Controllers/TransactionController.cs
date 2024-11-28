using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Data;
using PersonalFinanceTracker.Models;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Controllers
{
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly FinanceContext _context;

        public TransactionController(FinanceContext context)
        {
            _context = context;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddTransaction([FromBody] Transaction transaction)
        {
            if (transaction.Amount == 0)
            {
                return BadRequest("Amount must be non-zero.");
            }

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Transaction added successfully.", Transaction = transaction });
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactions()
        {
            var transactions = await _context.Transactions.ToListAsync();
            return Ok(transactions);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetTransactionsByCategory(int categoryId)
        {
            var transactions = await _context.Transactions.Where(t => t.CategoryId == categoryId).ToListAsync();
            return Ok(transactions);
        }

        [HttpPut("{transactionId}")]
        public async Task<IActionResult> UpdateTransaction(int transactionId, [FromBody] Transaction updatedTransaction)
        {
            var transaction = await _context.Transactions.FindAsync(transactionId);
            if (transaction == null)
            {
                return NotFound();
            }

            transaction.Title = updatedTransaction.Title;
            transaction.Amount = updatedTransaction.Amount;
            transaction.Date = updatedTransaction.Date;
            transaction.Type = updatedTransaction.Type;
            transaction.CategoryId = updatedTransaction.CategoryId;
            transaction.Tags = updatedTransaction.Tags;

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Transaction updated successfully.", Transaction = transaction });
        }

        [HttpDelete("{transactionId}")]
        public async Task<IActionResult> DeleteTransaction(int transactionId)
        {
            var transaction = await _context.Transactions.FindAsync(transactionId);
            if (transaction == null)
            {
                return NotFound();
            }

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();

            return Ok(new { Message = $"Transaction with ID {transactionId} deleted successfully." });
        }
    }
}
