#nullable disable
using System;
using System.Collections.Generic;

using KhumaloCraft_Part2.Models;

namespace KhumaloCraft_Part2.Models
{
    public partial class Transaction
    {
        public int TransactionId { get; set; }

        public int? UserId { get; set; }

        public DateTime? TransactionDate { get; set; }

        public double? TransactionTotalPrice { get; set; }

        public string TransactionPaymentMethod { get; set; }

        public string TransactionStatus { get; set; }

        public virtual User User { get; set; }
    }
}


