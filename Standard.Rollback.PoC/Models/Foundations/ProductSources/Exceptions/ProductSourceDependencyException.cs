using Xeptions;

namespace Standard.Rollback.PoC.Models.Foundations.ProductSources.Exceptions
{
    public class ProductSourceDependencyException : Xeption
    {
        public ProductSourceDependencyException(Xeption innerException) :
            base(message: "ProductSource dependency error occurred, contact support.", innerException)
        { }
    }
}