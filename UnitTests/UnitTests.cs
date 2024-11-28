using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersonalFinanceTracker.Controllers;
using PersonalFinanceTracker.Models;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PersonalFinanceTracker.Tests
{
    [TestClass]
    public class PersonalFinanceControllerTests
    {
        private Mock<ITransactionService> _mockTransactionService;
        private Mock<ICategoryService> _mockCategoryService;
        private Mock<IBudgetAlertService> _mockBudgetAlertService;
        private FinanceController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockTransactionService = new Mock<ITransactionService>();
            _mockCategoryService = new Mock<ICategoryService>();
            _mockBudgetAlertService = new Mock<IBudgetAlertService>();
            _controller = new FinanceController(_mockTransactionService.Object, _mockCategoryService.Object, _mockBudgetAlertService.Object);
        }

        [TestMethod]
        public async Task AddTransaction_ShouldReturnExpectedResult()
        {
            var transaction = new Transaction { Id = 1, Title = "Test", Amount = 100, Date = DateTime.Now, Type = "Expense" };
            _mockTransactionService.Setup(service => service.AddTransaction(transaction)).ReturnsAsync(new OkResult());

            var result = await _controller.AddTransaction(transaction);

            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task GetTransactions_ShouldReturnExpectedResult()
        {
            _mockTransactionService.Setup(service => service.GetTransactions()).ReturnsAsync(new OkResult());

            var result = await _controller.GetTransactions();

            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task GetTransactionsByCategory_ShouldReturnExpectedResult()
        {
            int categoryId = 1;
            _mockTransactionService.Setup(service => service.GetTransactionsByCategory(categoryId)).ReturnsAsync(new OkResult());

            var result = await _controller.GetTransactionsByCategory(categoryId);

            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task UpdateTransaction_ShouldReturnExpectedResult()
        {
            int transactionId = 1;
            var updatedTransaction = new Transaction { Id = 1, Title = "Updated Test", Amount = 200, Date = DateTime.Now, Type = "Income" };
            _mockTransactionService.Setup(service => service.UpdateTransaction(transactionId, updatedTransaction)).ReturnsAsync(new OkResult());

            var result = await _controller.UpdateTransaction(transactionId, updatedTransaction);

            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task DeleteTransaction_ShouldReturnExpectedResult()
        {
            int transactionId = 1;
            _mockTransactionService.Setup(service => service.DeleteTransaction(transactionId)).ReturnsAsync(new OkResult());

            var result = await _controller.DeleteTransaction(transactionId);

            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task GetCurrentBalance_ShouldReturnExpectedResult()
        {
            _mockTransactionService.Setup(service => service.GetCurrentBalance()).ReturnsAsync(new OkResult());

            var result = await _controller.GetCurrentBalance();

            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task GetMonthlySummary_ShouldReturnExpectedResult()
        {
            int year = 2023;
            int month = 10;
            _mockTransactionService.Setup(service => service.GetMonthlySummary(year, month)).ReturnsAsync(new OkResult());

            var result = await _controller.GetMonthlySummary(year, month);

            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task GetYearlySummary_ShouldReturnExpectedResult()
        {
            int year = 2023;
            _mockTransactionService.Setup(service => service.GetYearlySummary(year)).ReturnsAsync(new OkResult());

            var result = await _controller.GetYearlySummary(year);

            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task SearchTransactions_ShouldReturnExpectedResult()
        {
            string keyword = "Test";
            DateTime? startDate = DateTime.Now.AddMonths(-1);
            DateTime? endDate = DateTime.Now;
            _mockTransactionService.Setup(service => service.SearchTransactions(keyword, startDate, endDate)).ReturnsAsync(new OkResult());

            var result = await _controller.SearchTransactions(keyword, startDate, endDate);

            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task FilterTransactions_ShouldReturnExpectedResult()
        {
            decimal? minAmount = 50;
            decimal? maxAmount = 500;
            string type = "Expense";
            _mockTransactionService.Setup(service => service.FilterTransactions(minAmount, maxAmount, type)).ReturnsAsync(new OkResult());

            var result = await _controller.FilterTransactions(minAmount, maxAmount, type);

            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task GetSpendingReport_ShouldReturnExpectedResult()
        {
            DateTime startDate = DateTime.Now.AddMonths(-1);
            DateTime endDate = DateTime.Now;
            _mockTransactionService.Setup(service => service.GetSpendingReport(startDate, endDate)).ReturnsAsync(new OkResult());

            var result = await _controller.GetSpendingReport(startDate, endDate);

            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task ExportTransactionsToCsv_ShouldReturnExpectedResult()
        {
            _mockTransactionService.Setup(service => service.ExportTransactionsToCsv()).ReturnsAsync(new OkResult());

            var result = await _controller.ExportTransactionsToCsv();

            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task AddCategory_ShouldReturnExpectedResult()
        {
            var category = new Category { Id = 1, Name = "Test Category" };
            _mockCategoryService.Setup(service => service.AddCategory(category)).ReturnsAsync(new OkResult());

            var result = await _controller.AddCategory(category);

            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task GetAllCategories_ShouldReturnExpectedResult()
        {
            _mockCategoryService.Setup(service => service.GetAllCategories()).ReturnsAsync(new OkResult());

            var result = await _controller.GetAllCategories();

            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task SetBudgetAlert_ShouldReturnExpectedResult()
        {
            var budgetAlert = new BudgetAlert { Id = 1, CategoryId = 1, Threshold = 1000, AlertMessage = "Test Alert" };
            _mockBudgetAlertService.Setup(service => service.SetBudgetAlert(budgetAlert)).ReturnsAsync(new OkResult());

            var result = await _controller.SetBudgetAlert(budgetAlert);

            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task GetBudgetStatus_ShouldReturnExpectedResult()
        {
            _mockBudgetAlertService.Setup(service => service.GetBudgetStatus()).ReturnsAsync(new OkResult());

            var result = await _controller.GetBudgetStatus();

            Assert.IsInstanceOfType(result, typeof(OkResult));
        }
    }
}
