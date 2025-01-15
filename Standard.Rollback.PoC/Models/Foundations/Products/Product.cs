using System;
using System.Collections.Generic;
using Standard.Rollback.PoC.Models.Foundations.ProductImages;
using Standard.Rollback.PoC.Models.Foundations.ProductSources;

namespace Standard.Rollback.PoC.Models.Foundations.Products
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ProductSource> ProductSources { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }
    }
}
