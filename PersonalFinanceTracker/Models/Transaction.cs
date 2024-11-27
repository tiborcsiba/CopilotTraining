using System;
using System.Collections.Generic;

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
}
