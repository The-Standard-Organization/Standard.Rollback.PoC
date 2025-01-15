using System;
using Xeptions;

namespace Standard.Rollback.PoC.Models.Foundations.Products.Exceptions
{
    public class AlreadyExistsProductException : Xeption
    {
        public AlreadyExistsProductException(Exception innerException)
            : base(message: "Product with the same Id already exists.", innerException)
        { }
    }
}