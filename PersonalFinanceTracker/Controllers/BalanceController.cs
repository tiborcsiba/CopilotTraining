using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Data;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Controllers
{
    [Route("api/[controller]")]
    public class BalanceController : ControllerBase
    {
        private readonly FinanceContext _context;

        public BalanceController(FinanceContext context)
        {
            _context = context;
        }

        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentBalance()
        {
            var income = await _context.Transactions.Where(t => t.Type == "Income").SumAsync(t => t.Amount);
            var expenses = await _context.Transactions.Where(t => t.Type == "Expense").SumAsync(t => t.Amount);
            var balance = income - expenses;

            return Ok(new { Balance = balance });
        }

        [HttpGet("summary/monthly/{year}/{month}")]
        public async Task<IActionResult> GetMonthlySummary(int year, int month)
        {
            var income = await _context.Transactions
                .Where(t => t.Type == "Income" && t.Date.Year == year && t.Date.Month == month)
                .SumAsync(t => t.Amount);

            var expenses = await _context.Transactions
                .Where(t => t.Type == "Expense" && t.Date.Year == year && t.Date.Month == month)
                .SumAsync(t => t.Amount);

            var savings = income - expenses;

            return Ok(new
            {
                Month = $"{year}-{month}",
                Income = income,
                Expenses = expenses,
                Savings = savings
            });
        }

        [HttpGet("summary/yearly/{year}")]
        public async Task<IActionResult> GetYearlySummary(int year)
        {
            var income = await _context.Transactions
                .Where(t => t.Type == "Income" && t.Date.Year == year)
                .SumAsync(t => t.Amount);

            var expenses = await _context.Transactions
                .Where(t => t.Type == "Expense" && t.Date.Year == year)
                .SumAsync(t => t.Amount);

            var savings = income - expenses;

            return Ok(new
            {
                Year = year,
                Income = income,
                Expenses = expenses,
                Savings = savings
            });
        }
    }
}
