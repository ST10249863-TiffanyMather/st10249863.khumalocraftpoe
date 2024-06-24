#nullable disable
using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;

namespace KhumaloCraft_Part2.Models
{
    public partial class User
    {
        public int UserId { get; set; }

        public string UserEmail { get; set; }

        public virtual ICollection<ProductTransaction> Products { get; set; } = new List<ProductTransaction>();

        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}


