using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Controllers
{
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly FinanceContext _context;

        public SearchController(FinanceContext context)
        {
            _context = context;
        }

        [HttpGet("transactions")]
        public async Task<IActionResult> SearchTransactions([FromQuery] string keyword, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var transactions = await _context.Transactions
                .Where(t => t.Title.Contains(keyword) &&
                            (!startDate.HasValue || t.Date >= startDate) &&
                            (!endDate.HasValue || t.Date <= endDate))
                .ToListAsync();

            return Ok(transactions);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> FilterTransactions([FromQuery] decimal? minAmount, [FromQuery] decimal? maxAmount, [FromQuery] string type)
        {
            var transactions = await _context.Transactions
                .Where(t => (!minAmount.HasValue || t.Amount >= minAmount) &&
                            (!maxAmount.HasValue || t.Amount <= maxAmount) &&
                            (string.IsNullOrEmpty(type) || t.Type == type))
                .ToListAsync();

            return Ok(transactions);
        }
    }
}
