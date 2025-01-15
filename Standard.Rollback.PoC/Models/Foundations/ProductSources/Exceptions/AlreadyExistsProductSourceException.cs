using System;
using Xeptions;

namespace Standard.Rollback.PoC.Models.Foundations.ProductSources.Exceptions
{
    public class AlreadyExistsProductSourceException : Xeption
    {
        public AlreadyExistsProductSourceException(Exception innerException)
            : base(message: "ProductSource with the same Id already exists.", innerException)
        { }
    }
}