using Xeptions;

namespace Standard.Rollback.PoC.Models.Foundations.ProductSources.Exceptions
{
    public class InvalidProductSourceException : Xeption
    {
        public InvalidProductSourceException()
            : base(message: "Invalid productSource. Please correct the errors and try again.")
        { }
    }
}