using Xeptions;

namespace Standard.Rollback.PoC.Models.Foundations.ProductImages.Exceptions
{
    public class InvalidProductImageException : Xeption
    {
        public InvalidProductImageException()
            : base(message: "Invalid productImage. Please correct the errors and try again.")
        { }
    }
}