using Microsoft.AspNetCore.Mvc;
using PersonalFinanceTracker.Models;

public interface ITransactionService
{
    Task<IActionResult> AddTransaction(Transaction transaction);
    Task<IActionResult> GetTransactions();
    Task<IActionResult> GetTransactionsByCategory(int categoryId);
    Task<IActionResult> UpdateTransaction(int transactionId, Transaction updatedTransaction);
    Task<IActionResult> DeleteTransaction(int transactionId);
    Task<IActionResult> GetCurrentBalance();
    Task<IActionResult> GetMonthlySummary(int year, int month);
    Task<IActionResult> GetYearlySummary(int year);
    Task<IActionResult> SearchTransactions(string keyword, DateTime? startDate, DateTime? endDate);
    Task<IActionResult> FilterTransactions(decimal? minAmount, decimal? maxAmount, string type);
    Task<IActionResult> GetSpendingReport(DateTime startDate, DateTime endDate);
    Task<IActionResult> ExportTransactionsToCsv();
}

public interface ICategoryService
{
    Task<IActionResult> AddCategory(Category category);
    Task<IActionResult> GetAllCategories();
}

public interface IBudgetAlertService
{
    Task<IActionResult> SetBudgetAlert(BudgetAlert budgetAlert);
    Task<IActionResult> GetBudgetStatus();
}
