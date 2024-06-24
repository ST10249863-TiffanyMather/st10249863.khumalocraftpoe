#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KhumaloCraft_Part2.Models

{
    public partial class Product
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        public double? ProductPrice { get; set; }

        public string ProductCategory { get; set; }

        public bool? ProductAvailable { get; set; }

        public string ProductImage { get; set; }

        public string ProductArtisan { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}



