using System;
using Xeptions;

namespace Standard.Rollback.PoC.Models.Foundations.ProductSources.Exceptions
{
    public class ProductSourceServiceException : Xeption
    {
        public ProductSourceServiceException(Exception innerException)
            : base(message: "ProductSource service error occurred, contact support.", innerException)
        { }
    }
}