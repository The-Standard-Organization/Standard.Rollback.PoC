using System;
using Standard.Rollback.PoC.Models.Foundations.Products;
using Standard.Rollback.PoC.Models.Foundations.Products.Exceptions;

namespace SdxRollbackPoc.Services.Orchestrations.Products
{
    public partial class ProductOrchestrationService
    {
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
