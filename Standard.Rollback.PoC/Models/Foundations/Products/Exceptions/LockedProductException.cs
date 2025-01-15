using System;
using Xeptions;

namespace Standard.Rollback.PoC.Models.Foundations.Products.Exceptions
{
    public class LockedProductException : Xeption
    {
        public LockedProductException(Exception innerException)
            : base(message: "Locked product record exception, please try again later", innerException)
        {
        }
    }
}