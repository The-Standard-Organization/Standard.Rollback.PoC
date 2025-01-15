using Xeptions;

namespace Standard.Rollback.PoC.Models.Foundations.Products.Exceptions
{
    public class NullProductException : Xeption
    {
        public NullProductException()
            : base(message: "Product is null.")
        { }
    }
}