using System;
using Xeptions;

namespace Standard.Rollback.PoC.Models.Foundations.ProductSources.Exceptions
{
    public class FailedProductSourceStorageException : Xeption
    {
        public FailedProductSourceStorageException(Exception innerException)
            : base(message: "Failed productSource storage error occurred, contact support.", innerException)
        { }
    }
}