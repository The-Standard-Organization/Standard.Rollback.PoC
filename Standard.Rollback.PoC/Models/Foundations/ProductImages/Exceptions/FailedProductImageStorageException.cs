using System;
using Xeptions;

namespace Standard.Rollback.PoC.Models.Foundations.ProductImages.Exceptions
{
    public class FailedProductImageStorageException : Xeption
    {
        public FailedProductImageStorageException(Exception innerException)
            : base(message: "Failed productImage storage error occurred, contact support.", innerException)
        { }
    }
}