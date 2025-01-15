using System;
using Standard.Rollback.PoC.Models.Foundations.Products;
using Standard.Rollback.PoC.Models.Foundations.Products.Exceptions;

namespace Standard.Rollback.PoC.Services.Orchestrations.Products
{
    public partial class ProductOrchestrationService
    {
        private void ValidateProduct(Product product)
        {
            if (product == null)
                throw new InvalidProductException();
        }

        private void ValidateProductExists(Product product, Guid productId)
        {
            if (product == null)
                throw new NotFoundProductException(productId);
        }

        private static void ValidateProductId(Guid productId)
        {
            if (productId == Guid.Empty)
            {
                throw new InvalidProductException();
            }
        }
    }
}
