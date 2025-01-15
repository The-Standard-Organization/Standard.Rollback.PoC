using Xeptions;

namespace Standard.Rollback.PoC.Models.Foundations.Products.Exceptions
{
    public class ProductValidationException : Xeption
    {
        public ProductValidationException(Xeption innerException)
            : base(message: "Product validation errors occurred, please try again.",
                  innerException)
        { }
    }
}