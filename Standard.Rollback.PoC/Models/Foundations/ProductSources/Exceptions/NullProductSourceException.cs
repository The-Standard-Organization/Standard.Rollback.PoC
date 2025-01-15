using Xeptions;

namespace Standard.Rollback.PoC.Models.Foundations.ProductSources.Exceptions
{
    public class NullProductSourceException : Xeption
    {
        public NullProductSourceException()
            : base(message: "ProductSource is null.")
        { }
    }
}