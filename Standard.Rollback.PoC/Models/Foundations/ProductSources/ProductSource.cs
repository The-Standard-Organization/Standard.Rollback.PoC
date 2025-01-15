using System;
using Standard.Rollback.PoC.Models.Foundations.Products;

namespace Standard.Rollback.PoC.Models.Foundations.ProductSources
{
    public class ProductSource
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; }

    }
}
