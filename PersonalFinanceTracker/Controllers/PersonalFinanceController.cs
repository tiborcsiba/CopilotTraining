using Microsoft.AspNetCore.Mvc;
using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.Controllers
{
    [Route("api/[controller]")]
    public class FinanceController : ControllerBase
    {
        // Transaction-Related Endpoints
        [HttpPost("transaction/add")]
        public IActionResult AddTransaction([FromBody] Transaction transaction)
        {
            // Simulate adding a transaction
            if (transaction.Amount == 0)
            {
                return BadRequest("Amount must be non-zero.");
            }

            return Ok(new { Message = "Transaction added successfully.", Transaction = transaction });
        }

        [HttpGet("transactions")]
        public IActionResult GetTransactions()
        {
            // Simulate fetching transactions
            var transactions = new List<Transaction>
            {
                new Transaction { Id = 1, Title = "Grocery", Amount = -50, Date = DateTime.Today, Type = "Expense" },
                new Transaction { Id = 2, Title = "Salary", Amount = 2000, Date = DateTime.Today, Type = "Income" }
            };

            return Ok(transactions);
        }

        [HttpGet("category/{categoryId}")]
        public IActionResult GetTransactionsByCategory(int categoryId)
        {
            // Simulate fetching transactions by category
            var transactions = new List<Transaction>
            {
                new Transaction { Id = 1, Title = "Electric Bill", Amount = -100, Date = DateTime.Today, Type = "Expense", CategoryId = categoryId }
            };

            return Ok(transactions);
        }

        [HttpPut("{transactionId}")]
        public IActionResult UpdateTransaction(int transactionId, [FromBody] Transaction updatedTransaction)
        {
            // Simulate updating a transaction
            updatedTransaction.Id = transactionId;

            return Ok(new { Message = "Transaction updated successfully.", Transaction = updatedTransaction });
        }

        [HttpDelete("{transactionId}")]
        public IActionResult DeleteTransaction(int transactionId)
        {
            // Simulate deleting a transaction
            return Ok(new { Message = $"Transaction with ID {transactionId} deleted successfully." });
        }

        // Balance and Summary Endpoints
        [HttpGet("balance")]
        public IActionResult GetCurrentBalance()
        {
            // Simulate calculating current balance
            decimal balance = 2000 - 150;
            return Ok(new { Balance = balance });
        }

        [HttpGet("summary/monthly/{year}/{month}")]
        public IActionResult GetMonthlySummary(int year, int month)
        {
            // Simulate a monthly summary
            return Ok(new
            {
                Month = $"{year}-{month}",
                Income = 2000,
                Expenses = 1500,
                Savings = 500
            });
        }

        [HttpGet("summary/yearly/{year}")]
        public IActionResult GetYearlySummary(int year)
        {
            // Simulate a yearly summary
            return Ok(new
            {
                Year = year,
                Income = 24000,
                Expenses = 18000,
                Savings = 6000
            });
        }

        // Category and Tagging Endpoints
        [HttpPost("category/add")]
        public IActionResult AddCategory([FromBody] Category category)
        {
            // Simulate adding a category
            return Ok(new { Message = "Category added successfully.", Category = category });
        }

        [HttpGet("categories")]
        public IActionResult GetAllCategories()
        {
            // Simulate fetching categories
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Food" },
                new Category { Id = 2, Name = "Rent" }
            };

            return Ok(categories);
        }

        [HttpPost("{transactionId}/tags")]
        public IActionResult TagTransaction(int transactionId, [FromBody] string[] tags)
        {
            // Simulate tagging a transaction
            return Ok(new { Message = "Tags added successfully.", TransactionId = transactionId, Tags = tags });
        }

        [HttpGet("tags/{tag}")]
        public IActionResult GetTransactionsByTag(string tag)
        {
            // Simulate fetching transactions by tag
            var transactions = new List<Transaction>
            {
                new Transaction { Id = 1, Title = "Grocery", Amount = -50, Tags = new List<string> { "food", "essentials" } }
            };

            return Ok(transactions.Where(t => t.Tags.Contains(tag)));
        }

        // Search and Filter Endpoints
        [HttpGet("search")]
        public IActionResult SearchTransactions([FromQuery] string keyword, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            // Simulate searching transactions
            var transactions = new List<Transaction>
            {
                new Transaction { Id = 1, Title = "Grocery", Amount = -50, Date = DateTime.Today },
                new Transaction { Id = 2, Title = "Salary", Amount = 2000, Date = DateTime.Today }
            };

            return Ok(transactions.Where(t => t.Title.Contains(keyword)));
        }

        [HttpGet("filter")]
        public IActionResult FilterTransactions([FromQuery] decimal? minAmount, [FromQuery] decimal? maxAmount, [FromQuery] string type)
        {
            // Simulate filtering transactions
            var transactions = new List<Transaction>
            {
                new Transaction { Id = 1, Title = "Grocery", Amount = -50, Type = "Expense" },
                new Transaction { Id = 2, Title = "Salary", Amount = 2000, Type = "Income" }
            };

            return Ok(transactions.Where(t => (!minAmount.HasValue || t.Amount >= minAmount) &&
                                               (!maxAmount.HasValue || t.Amount <= maxAmount) &&
                                               (string.IsNullOrEmpty(type) || t.Type == type)));
        }

        // Reporting and Visualization
        [HttpGet("reports/spending")]
        public IActionResult GetSpendingReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            // Simulate a spending report
            var report = new
            {
                Categories = new List<object>
                {
                    new { Name = "Food", Total = 500 },
                    new { Name = "Rent", Total = 1000 }
                }
            };

            return Ok(report);
        }

        [HttpGet("reports/transactions/export")]
        public IActionResult ExportTransactionsToCsv()
        {
            // Simulate exporting transactions
            return File(new byte[0], "text/csv", "transactions.csv");
        }

        // Notifications and Reminders
        [HttpPost("budget/alert")]
        public IActionResult SetBudgetAlert([FromBody] BudgetAlertDto budgetAlert)
        {
            // Simulate setting a budget alert
            return Ok(new { Message = "Budget alert set successfully.", Alert = budgetAlert });
        }

        [HttpGet("budget/status")]
        public IActionResult GetBudgetStatus()
        {
            // Simulate fetching budget status
            return Ok(new
            {
                Alerts = new List<object>
                {
                    new { Category = "Food", Threshold = 500, Spent = 600 }
                }
            });
        }

    }
}
