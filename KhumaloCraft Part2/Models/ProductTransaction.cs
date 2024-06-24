#nullable disable
using System.Collections.Generic;
using KhumaloCraft_Part2.Models;

namespace KhumaloCraft_Part2.Models
{
    public partial class ProductTransaction
    {
        public int ProductTransactionId { get; set; }

        public int? TransactionID { get; set; }

        public int? ProductId { get; set; }

        public virtual Transaction Transaction { get; set; }

        public virtual Product Product { get; set; }
    }
}
