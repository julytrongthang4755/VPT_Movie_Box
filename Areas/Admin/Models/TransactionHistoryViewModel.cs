using Microsoft.AspNetCore.Mvc;

namespace VPT_Movie_Box.Models
{
        public class TransactionHistoryViewModel
    {
            public string? TransactionId { get; set; }
            public DateTime Date { get; set; }
            public decimal Amount { get; set; }
            public string? Status { get; set; }
        }
}
