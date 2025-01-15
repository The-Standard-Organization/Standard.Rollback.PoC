using System;
using Xeptions;

namespace Standard.Rollback.PoC.Models.Foundations.ProductSources.Exceptions
{
    public class LockedProductSourceException : Xeption
    {
        public LockedProductSourceException(Exception innerException)
            : base(message: "Locked productSource record exception, please try again later", innerException)
        {
        }
    }
}