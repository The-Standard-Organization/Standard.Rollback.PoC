using Xeptions;

namespace Standard.Rollback.PoC.Models.Foundations.Products.Exceptions
{
    public class ProductDependencyValidationException : Xeption
    {
        public ProductDependencyValidationException(Xeption innerException)
            : base(message: "Product dependency validation occurred, please try again.", innerException)
        { }
    }
}