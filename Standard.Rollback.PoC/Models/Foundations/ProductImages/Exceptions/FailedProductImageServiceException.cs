using System;
using Xeptions;

namespace Standard.Rollback.PoC.Models.Foundations.ProductImages.Exceptions
{
    public class FailedProductImageServiceException : Xeption
    {
        public FailedProductImageServiceException(Exception innerException)
            : base(message: "Failed productImage service occurred, please contact support", innerException)
        { }
    }
}