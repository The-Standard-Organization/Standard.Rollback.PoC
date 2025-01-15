using Xeptions;

namespace Standard.Rollback.PoC.Models.Foundations.ProductImages.Exceptions
{
    public class ProductImageValidationException : Xeption
    {
        public ProductImageValidationException(Xeption innerException)
            : base(message: "ProductImage validation errors occurred, please try again.",
                  innerException)
        { }
    }
}