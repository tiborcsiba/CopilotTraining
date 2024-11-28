using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Data;
using PersonalFinanceTracker.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Controllers
{
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly FinanceContext _context;

        public CategoryController(FinanceContext context)
        {
            _context = context;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddCategory([FromBody] Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Category added successfully.", Category = category });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            return Ok(categories);
        }

        [HttpPost("{transactionId}/tags")]
        public async Task<IActionResult> TagTransaction(int transactionId, [FromBody] string[] tags)
        {
            var transaction = await _context.Transactions.FindAsync(transactionId);
            if (transaction == null)
            {
                return NotFound();
            }

            transaction.Tags = tags.ToList();
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Tags added successfully.", TransactionId = transactionId, Tags = tags });
        }

        [HttpGet("tags/{tag}")]
        public async Task<IActionResult> GetTransactionsByTag(string tag)
        {
            var transactions = await _context.Transactions.Where(t => t.Tags.Contains(tag)).ToListAsync();
            return Ok(transactions);
        }
    }
}
