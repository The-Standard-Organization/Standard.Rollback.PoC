using Xeptions;

namespace Standard.Rollback.PoC.Models.Foundations.ProductSources.Exceptions
{
    public class ProductSourceDependencyValidationException : Xeption
    {
        public ProductSourceDependencyValidationException(Xeption innerException)
            : base(message: "ProductSource dependency validation occurred, please try again.", innerException)
        { }
    }
}