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
            throw new NotImplementedException();
        }

        [HttpGet("transactions")]
        public IActionResult GetTransactions()
        {
            throw new NotImplementedException();
        }

        [HttpGet("category/{categoryId}")]
        public IActionResult GetTransactionsByCategory(int categoryId)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{transactionId}")]
        public IActionResult UpdateTransaction(int transactionId, [FromBody] Transaction updatedTransaction)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{transactionId}")]
        public IActionResult DeleteTransaction(int transactionId)
        {
            throw new NotImplementedException();
        }

        // Balance and Summary Endpoints
        [HttpGet("balance")]
        public IActionResult GetCurrentBalance()
        {
            throw new NotImplementedException();
        }

        [HttpGet("summary/monthly/{year}/{month}")]
        public IActionResult GetMonthlySummary(int year, int month)
        {
            throw new NotImplementedException();
        }

        [HttpGet("summary/yearly/{year}")]
        public IActionResult GetYearlySummary(int year)
        {
            throw new NotImplementedException();
        }

        // Category and Tagging Endpoints
        [HttpPost]
        public IActionResult AddCategory([FromBody] Category category)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public IActionResult GetAllCategories()
        {
            throw new NotImplementedException();
        }

        [HttpPost("{transactionId}/tags")]
        public IActionResult TagTransaction(int transactionId, [FromBody] string[] tags)
        {
            throw new NotImplementedException();
        }

        [HttpGet("tags/{tag}")]
        public IActionResult GetTransactionsByTag(string tag)
        {
            throw new NotImplementedException();
        }

        // Search and Filter Endpoints
        [HttpGet("search")]
        public IActionResult SearchTransactions([FromQuery] string keyword, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            throw new NotImplementedException();
        }

        [HttpGet("filter")]
        public IActionResult FilterTransactions([FromQuery] decimal? minAmount, [FromQuery] decimal? maxAmount, [FromQuery] string type)
        {
            throw new NotImplementedException();
        }

        // Reporting and Visualization
        [HttpGet("reports/spending")]
        public IActionResult GetSpendingReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            throw new NotImplementedException();
        }

        [HttpGet("reports/transactions/export")]
        public IActionResult ExportTransactionsToCsv()
        {
            throw new NotImplementedException();
        }

        // Notifications and Reminders
        [HttpPost("budget/alert")]
        public IActionResult SetBudgetAlert([FromBody] BudgetAlertDto budgetAlert)
        {
            throw new NotImplementedException();
        }

        [HttpGet("budget/status")]
        public IActionResult GetBudgetStatus()
        {
            throw new NotImplementedException();
        }
    }
}
