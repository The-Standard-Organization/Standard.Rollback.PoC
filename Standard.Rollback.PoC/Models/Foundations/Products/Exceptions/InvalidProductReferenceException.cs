using System;
using Xeptions;

namespace Standard.Rollback.PoC.Models.Foundations.Products.Exceptions
{
    public class InvalidProductReferenceException : Xeption
    {
        public InvalidProductReferenceException(Exception innerException)
            : base(message: "Invalid product reference error occurred.", innerException) { }
    }
}