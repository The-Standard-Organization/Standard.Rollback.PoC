using Xeptions;

namespace Standard.Rollback.PoC.Models.Foundations.ProductImages.Exceptions
{
    public class ProductImageDependencyException : Xeption
    {
        public ProductImageDependencyException(Xeption innerException) :
            base(message: "ProductImage dependency error occurred, contact support.", innerException)
        { }
    }
}