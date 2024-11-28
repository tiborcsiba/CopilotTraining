using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Data;
using PersonalFinanceTracker.Models;

public class TransactionService : ITransactionService
{
    private readonly FinanceContext _context;

    public TransactionService(FinanceContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> AddTransaction(Transaction transaction)
    {
        if (transaction.Amount == 0)
        {
            return new BadRequestObjectResult("Amount must be non-zero.");
        }

        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();

        return new OkObjectResult(new { Message = "Transaction added successfully.", Transaction = transaction });
    }

    public async Task<IActionResult> GetTransactions()
    {
        var transactions = await _context.Transactions.ToListAsync();
        return new OkObjectResult(transactions);
    }

    public async Task<IActionResult> GetTransactionsByCategory(int categoryId)
    {
        var transactions = await _context.Transactions.Where(t => t.CategoryId == categoryId).ToListAsync();
        return new OkObjectResult(transactions);
    }

    public async Task<IActionResult> UpdateTransaction(int transactionId, Transaction updatedTransaction)
    {
        var transaction = await _context.Transactions.FindAsync(transactionId);
        if (transaction == null)
        {
            return new NotFoundResult();
        }

        transaction.Title = updatedTransaction.Title;
        transaction.Amount = updatedTransaction.Amount;
        transaction.Date = updatedTransaction.Date;
        transaction.Type = updatedTransaction.Type;
        transaction.CategoryId = updatedTransaction.CategoryId;
        transaction.Tags = updatedTransaction.Tags;

        await _context.SaveChangesAsync();

        return new OkObjectResult(new { Message = "Transaction updated successfully.", Transaction = transaction });
    }

    public async Task<IActionResult> DeleteTransaction(int transactionId)
    {
        var transaction = await _context.Transactions.FindAsync(transactionId);
        if (transaction == null)
        {
            return new NotFoundResult();
        }

        _context.Transactions.Remove(transaction);
        await _context.SaveChangesAsync();

        return new OkObjectResult(new { Message = $"Transaction with ID {transactionId} deleted successfully." });
    }

    public async Task<IActionResult> GetCurrentBalance()
    {
        var income = await _context.Transactions.Where(t => t.Type == "Income").SumAsync(t => t.Amount);
        var expenses = await _context.Transactions.Where(t => t.Type == "Expense").SumAsync(t => t.Amount);
        var balance = income - expenses;

        return new OkObjectResult(new { Balance = balance });
    }

    public async Task<IActionResult> GetMonthlySummary(int year, int month)
    {
        var income = await _context.Transactions
            .Where(t => t.Type == "Income" && t.Date.Year == year && t.Date.Month == month)
            .SumAsync(t => t.Amount);

        var expenses = await _context.Transactions
            .Where(t => t.Type == "Expense" && t.Date.Year == year && t.Date.Month == month)
            .SumAsync(t => t.Amount);

        var savings = income - expenses;

        return new OkObjectResult(new
        {
            Month = $"{year}-{month}",
            Income = income,
            Expenses = expenses,
            Savings = savings
        });
    }

    public async Task<IActionResult> GetYearlySummary(int year)
    {
        var income = await _context.Transactions
            .Where(t => t.Type == "Income" && t.Date.Year == year)
            .SumAsync(t => t.Amount);

        var expenses = await _context.Transactions
            .Where(t => t.Type == "Expense" && t.Date.Year == year)
            .SumAsync(t => t.Amount);

        var savings = income - expenses;

        return new OkObjectResult(new
        {
            Year = year,
            Income = income,
            Expenses = expenses,
            Savings = savings
        });
    }

    public async Task<IActionResult> SearchTransactions(string keyword, DateTime? startDate, DateTime? endDate)
    {
        var transactions = await _context.Transactions
            .Where(t => t.Title.Contains(keyword) &&
                        (!startDate.HasValue || t.Date >= startDate) &&
                        (!endDate.HasValue || t.Date <= endDate))
            .ToListAsync();

        return new OkObjectResult(transactions);
    }

    public async Task<IActionResult> FilterTransactions(decimal? minAmount, decimal? maxAmount, string type)
    {
        var transactions = await _context.Transactions
            .Where(t => (!minAmount.HasValue || t.Amount >= minAmount) &&
                        (!maxAmount.HasValue || t.Amount <= maxAmount) &&
                        (string.IsNullOrEmpty(type) || t.Type == type))
            .ToListAsync();

        return new OkObjectResult(transactions);
    }

    public async Task<IActionResult> GetSpendingReport(DateTime startDate, DateTime endDate)
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

        return new OkObjectResult(new { Categories = report });
    }

    public async Task<IActionResult> ExportTransactionsToCsv()
    {
        var transactions = await _context.Transactions.ToListAsync();
        var csv = "Id,Title,Amount,Date,Type,CategoryId,Tags\n" +
                  string.Join("\n", transactions.Select(t => $"{t.Id},{t.Title},{t.Amount},{t.Date},{t.Type},{t.CategoryId},{string.Join(";", t.Tags)}"));

        return new FileContentResult(System.Text.Encoding.UTF8.GetBytes(csv), "text/csv") { FileDownloadName = "transactions.csv" };
    }
}

public class CategoryService : ICategoryService
{
    private readonly FinanceContext _context;

    public CategoryService(FinanceContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> AddCategory(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return new OkObjectResult(new { Message = "Category added successfully.", Category = category });
    }

    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await _context.Categories.ToListAsync();
        return new OkObjectResult(categories);
    }
}

public class BudgetAlertService : IBudgetAlertService
{
    private readonly FinanceContext _context;

    public BudgetAlertService(FinanceContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> SetBudgetAlert(BudgetAlert budgetAlert)
    {
        _context.BudgetAlerts.Add(budgetAlert);
        await _context.SaveChangesAsync();

        return new OkObjectResult(new { Message = "Budget alert set successfully.", Alert = budgetAlert });
    }

    public async Task<IActionResult> GetBudgetStatus()
    {
        var alerts = await _context.BudgetAlerts.ToListAsync();
        var status = alerts.Select(alert => new
        {
            alert.CategoryId,
            alert.Threshold,
            Spent = _context.Transactions.Where(t => t.CategoryId == alert.CategoryId && t.Type == "Expense").Sum(t => t.Amount)
        });

        return new OkObjectResult(new { Alerts = status });
    }
}
