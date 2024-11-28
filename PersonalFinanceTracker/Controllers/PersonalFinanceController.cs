using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Data;
using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.Controllers
{

    [Route("api/[controller]")]
    public class FinanceController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly ICategoryService _categoryService;
        private readonly IBudgetAlertService _budgetAlertService;

        public FinanceController(ITransactionService transactionService, ICategoryService categoryService, IBudgetAlertService budgetAlertService)
        {
            _transactionService = transactionService;
            _categoryService = categoryService;
            _budgetAlertService = budgetAlertService;
        }

        // Transaction-Related Endpoints
        [HttpPost("transaction/add")]
        public Task<IActionResult> AddTransaction([FromBody] Transaction transaction)
        {
            return _transactionService.AddTransaction(transaction);
        }

        [HttpGet("transactions")]
        public Task<IActionResult> GetTransactions()
        {
            return _transactionService.GetTransactions();
        }

        [HttpGet("category/{categoryId}")]
        public Task<IActionResult> GetTransactionsByCategory(int categoryId)
        {
            return _transactionService.GetTransactionsByCategory(categoryId);
        }

        [HttpPut("{transactionId}")]
        public Task<IActionResult> UpdateTransaction(int transactionId, [FromBody] Transaction updatedTransaction)
        {
            return _transactionService.UpdateTransaction(transactionId, updatedTransaction);
        }

        [HttpDelete("{transactionId}")]
        public Task<IActionResult> DeleteTransaction(int transactionId)
        {
            return _transactionService.DeleteTransaction(transactionId);
        }

        [HttpGet("balance")]
        public Task<IActionResult> GetCurrentBalance()
        {
            return _transactionService.GetCurrentBalance();
        }

        [HttpGet("summary/monthly/{year}/{month}")]
        public Task<IActionResult> GetMonthlySummary(int year, int month)
        {
            return _transactionService.GetMonthlySummary(year, month);
        }

        [HttpGet("summary/yearly/{year}")]
        public Task<IActionResult> GetYearlySummary(int year)
        {
            return _transactionService.GetYearlySummary(year);
        }

        [HttpGet("search")]
        public Task<IActionResult> SearchTransactions([FromQuery] string keyword, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            return _transactionService.SearchTransactions(keyword, startDate, endDate);
        }

        [HttpGet("filter")]
        public Task<IActionResult> FilterTransactions([FromQuery] decimal? minAmount, [FromQuery] decimal? maxAmount, [FromQuery] string type)
        {
            return _transactionService.FilterTransactions(minAmount, maxAmount, type);
        }

        [HttpGet("reports/spending")]
        public Task<IActionResult> GetSpendingReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            return _transactionService.GetSpendingReport(startDate, endDate);
        }

        [HttpGet("reports/transactions/export")]
        public Task<IActionResult> ExportTransactionsToCsv()
        {
            return _transactionService.ExportTransactionsToCsv();
        }

        // Category and Tagging Endpoints
        [HttpPost("category/add")]
        public Task<IActionResult> AddCategory([FromBody] Category category)
        {
            return _categoryService.AddCategory(category);
        }

        [HttpGet("categories")]
        public Task<IActionResult> GetAllCategories()
        {
            return _categoryService.GetAllCategories();
        }

        // Notifications and Reminders
        [HttpPost("budget/alert")]
        public Task<IActionResult> SetBudgetAlert([FromBody] BudgetAlert budgetAlert)
        {
            return _budgetAlertService.SetBudgetAlert(budgetAlert);
        }

        [HttpGet("budget/status")]
        public Task<IActionResult> GetBudgetStatus()
        {
            return _budgetAlertService.GetBudgetStatus();
        }
    }
}
