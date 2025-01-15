using Xeptions;

namespace Standard.Rollback.PoC.Models.Foundations.ProductSources.Exceptions
{
    public class ProductSourceValidationException : Xeption
    {
        public ProductSourceValidationException(Xeption innerException)
            : base(message: "ProductSource validation errors occurred, please try again.",
                  innerException)
        { }
    }
}