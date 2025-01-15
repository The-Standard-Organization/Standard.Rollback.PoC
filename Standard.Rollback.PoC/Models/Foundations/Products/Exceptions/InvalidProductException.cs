using Xeptions;

namespace Standard.Rollback.PoC.Models.Foundations.Products.Exceptions
{
    public class InvalidProductException : Xeption
    {
        public InvalidProductException()
            : base(message: "Invalid product. Please correct the errors and try again.")
        { }
    }
}