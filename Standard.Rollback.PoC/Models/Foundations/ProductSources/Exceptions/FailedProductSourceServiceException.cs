using System;
using Xeptions;

namespace Standard.Rollback.PoC.Models.Foundations.ProductSources.Exceptions
{
    public class FailedProductSourceServiceException : Xeption
    {
        public FailedProductSourceServiceException(Exception innerException)
            : base(message: "Failed productSource service occurred, please contact support", innerException)
        { }
    }
}