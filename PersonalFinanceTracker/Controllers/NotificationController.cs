using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Data;
using PersonalFinanceTracker.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Controllers
{
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly FinanceContext _context;

        public NotificationController(FinanceContext context)
        {
            _context = context;
        }

        [HttpPost("budget/alert")]
        public async Task<IActionResult> SetBudgetAlert([FromBody] BudgetAlert budgetAlert)
        {
            _context.BudgetAlerts.Add(budgetAlert);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Budget alert set successfully.", Alert = budgetAlert });
        }

        [HttpGet("budget/status")]
        public async Task<IActionResult> GetBudgetStatus()
        {
            var alerts = await _context.BudgetAlerts.ToListAsync();
            var status = alerts.Select(alert => new
            {
                alert.CategoryId,
                alert.Threshold,
                Spent = _context.Transactions.Where(t => t.CategoryId == alert.CategoryId && t.Type == "Expense").Sum(t => t.Amount)
            });

            return Ok(new { Alerts = status });
        }
    }
}
