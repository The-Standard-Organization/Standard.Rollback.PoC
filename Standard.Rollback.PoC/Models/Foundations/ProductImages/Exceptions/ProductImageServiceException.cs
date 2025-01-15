using System;
using Xeptions;

namespace Standard.Rollback.PoC.Models.Foundations.ProductImages.Exceptions
{
    public class ProductImageServiceException : Xeption
    {
        public ProductImageServiceException(Exception innerException)
            : base(message: "ProductImage service error occurred, contact support.", innerException)
        { }
    }
}