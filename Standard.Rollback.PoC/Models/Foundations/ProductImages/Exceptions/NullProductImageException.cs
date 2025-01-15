using Xeptions;

namespace Standard.Rollback.PoC.Models.Foundations.ProductImages.Exceptions
{
    public class NullProductImageException : Xeption
    {
        public NullProductImageException()
            : base(message: "ProductImage is null.")
        { }
    }
}