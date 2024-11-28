using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Data;
using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.Controllers
{
    [Route("api/[controller]")]
    public class FinanceController : ControllerBase
    {
        private readonly FinanceContext _context;

        public FinanceController(FinanceContext context)
        {
            _context = context;
        }

        // Transaction-Related Endpoints
        [HttpPost("transaction/add")]
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

        [HttpGet("transactions")]
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

        // Balance and Summary Endpoints
        [HttpGet("balance")]
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

        // Category and Tagging Endpoints
        [HttpPost("category/add")]
        public async Task<IActionResult> AddCategory([FromBody] Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Category added successfully.", Category = category });
        }

        [HttpGet("categories")]
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

        // Search and Filter Endpoints
        [HttpGet("search")]
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

        // Reporting and Visualization
        [HttpGet("reports/spending")]
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

        [HttpGet("reports/transactions/export")]
        public async Task<IActionResult> ExportTransactionsToCsv()
        {
            var transactions = await _context.Transactions.ToListAsync();
            var csv = "Id,Title,Amount,Date,Type,CategoryId,Tags\n" +
                      string.Join("\n", transactions.Select(t => $"{t.Id},{t.Title},{t.Amount},{t.Date},{t.Type},{t.CategoryId},{string.Join(";", t.Tags)}"));

            return File(System.Text.Encoding.UTF8.GetBytes(csv), "text/csv", "transactions.csv");
        }

        // Notifications and Reminders
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
