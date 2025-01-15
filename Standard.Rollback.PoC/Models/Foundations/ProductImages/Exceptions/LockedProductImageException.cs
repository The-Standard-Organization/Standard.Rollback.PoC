using System;
using Xeptions;

namespace Standard.Rollback.PoC.Models.Foundations.ProductImages.Exceptions
{
    public class LockedProductImageException : Xeption
    {
        public LockedProductImageException(Exception innerException)
            : base(message: "Locked productImage record exception, please try again later", innerException)
        {
        }
    }
}