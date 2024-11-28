namespace PersonalFinanceTracker.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public int? CategoryId { get; set; }
        public List<string> Tags { get; set; }
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class BudgetAlert
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public decimal Threshold { get; set; }
        public string AlertMessage { get; set; }
    }
}
