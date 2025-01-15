using System;
using Xeptions;

namespace Standard.Rollback.PoC.Models.Foundations.ProductImages.Exceptions
{
    public class NotFoundProductImageException : Xeption
    {
        public NotFoundProductImageException(Guid productImageId)
            : base(message: $"Couldn't find productImage with productImageId: {productImageId}.")
        { }
    }
}