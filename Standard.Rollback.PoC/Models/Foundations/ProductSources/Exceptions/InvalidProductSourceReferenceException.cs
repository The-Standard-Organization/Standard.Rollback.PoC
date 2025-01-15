using System;
using Xeptions;

namespace Standard.Rollback.PoC.Models.Foundations.ProductSources.Exceptions
{
    public class InvalidProductSourceReferenceException : Xeption
    {
        public InvalidProductSourceReferenceException(Exception innerException)
            : base(message: "Invalid productSource reference error occurred.", innerException) { }
    }
}