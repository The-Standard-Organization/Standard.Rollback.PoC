using System;
using Xeptions;

namespace Standard.Rollback.PoC.Models.Foundations.Products.Exceptions
{
    public class ProductServiceException : Xeption
    {
        public ProductServiceException(Exception innerException)
            : base(message: "Product service error occurred, contact support.", innerException)
        { }
    }
}