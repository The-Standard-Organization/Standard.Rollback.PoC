using System;
using Xeptions;

namespace Standard.Rollback.PoC.Models.Foundations.ProductSources.Exceptions
{
    public class NotFoundProductSourceException : Xeption
    {
        public NotFoundProductSourceException(Guid productSourceId)
            : base(message: $"Couldn't find productSource with productSourceId: {productSourceId}.")
        { }
    }
}