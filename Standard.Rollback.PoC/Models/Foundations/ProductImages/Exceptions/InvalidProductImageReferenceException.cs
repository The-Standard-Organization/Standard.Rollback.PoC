using System;
using Xeptions;

namespace Standard.Rollback.PoC.Models.Foundations.ProductImages.Exceptions
{
    public class InvalidProductImageReferenceException : Xeption
    {
        public InvalidProductImageReferenceException(Exception innerException)
            : base(message: "Invalid productImage reference error occurred.", innerException) { }
    }
}