using System;
using Xeptions;

namespace Standard.Rollback.PoC.Models.Foundations.ProductImages.Exceptions
{
    public class AlreadyExistsProductImageException : Xeption
    {
        public AlreadyExistsProductImageException(Exception innerException)
            : base(message: "ProductImage with the same Id already exists.", innerException)
        { }
    }
}