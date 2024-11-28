using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Controllers
{
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly FinanceContext _context;

        public ReportController(FinanceContext context)
        {
            _context = context;
        }

        [HttpGet("spending")]
        public async Task<IActionResult> GetSpendingReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var report = await _context.Transactions
                .Where(t => t.Type == "Expense" && t.Date >= startDate && t.Date <= endDate)
                .GroupBy(t => t.CategoryId)
                .Select(g => new
                {
                    Category = _context.Categories.FirstOrDefault(c => c.Id == g.Key).Name,
                    Total = g.Sum(t => t.Amount)
                })
                .ToListAsync();

            return Ok(new { Categories = report });
        }

        [HttpGet("transactions/export")]
        public async Task<IActionResult> ExportTransactionsToCsv()
        {
            var transactions = await _context.Transactions.ToListAsync();
            var csv = "Id,Title,Amount,Date,Type,CategoryId,Tags\n" +
                      string.Join("\n", transactions.Select(t => $"{t.Id},{t.Title},{t.Amount},{t.Date},{t.Type},{t.CategoryId},{string.Join(";", t.Tags)}"));

            return File(System.Text.Encoding.UTF8.GetBytes(csv), "text/csv", "transactions.csv");
        }
    }
}
