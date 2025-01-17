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

        public bool IsLocked { get; set; }
        public DateTimeOffset LockedDate { get; set; }

        public DateTime SysEndTime { get; set; }
        public DateTime SysStartTime { get; set; }

        public virtual ICollection<ProductSource> ProductSources { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }
    }
}
