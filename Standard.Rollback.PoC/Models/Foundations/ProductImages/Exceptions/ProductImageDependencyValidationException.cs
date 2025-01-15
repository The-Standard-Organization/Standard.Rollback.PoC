using Xeptions;

namespace Standard.Rollback.PoC.Models.Foundations.ProductImages.Exceptions
{
    public class ProductImageDependencyValidationException : Xeption
    {
        public ProductImageDependencyValidationException(Xeption innerException)
            : base(message: "ProductImage dependency validation occurred, please try again.", innerException)
        { }
    }
}