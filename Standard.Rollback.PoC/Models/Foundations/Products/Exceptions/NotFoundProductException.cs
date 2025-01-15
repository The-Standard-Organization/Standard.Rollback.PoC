using System;
using Xeptions;

namespace Standard.Rollback.PoC.Models.Foundations.Products.Exceptions
{
    public class NotFoundProductException : Xeption
    {
        public NotFoundProductException(Guid productId)
            : base(message: $"Couldn't find product with productId: {productId}.")
        { }
    }
}