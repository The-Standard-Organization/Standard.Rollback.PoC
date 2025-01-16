using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Standard.Rollback.PoC.Brokers.Loggings;
using Standard.Rollback.PoC.Models.Foundations.ProductImages;
using Standard.Rollback.PoC.Models.Foundations.Products;
using Standard.Rollback.PoC.Services.Foundations.ProductImages;
using Standard.Rollback.PoC.Services.Foundations.Products;

namespace Standard.Rollback.PoC.Services.Orchestrations.Products
{
    public partial class ProductOrchestrationService : IProductOrchestrationService
    {
        private readonly IProductService productService;
        private readonly IProductImageService productImageService;
        private readonly ILoggingBroker loggingBroker;

        public ProductOrchestrationService(
            IProductService productService,
            IProductImageService productImageService,
            ILoggingBroker loggingBroker)
        {
            this.productService = productService;
            this.productImageService = productImageService;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Product> ModifyOrRollbackProductAsync(Product product) =>
        TryCatchAndRollback(async () =>
        {
            ValidateProduct(product);

            var modifiedProduct =
                await this.productService.ModifyProductAsync(product);

            throw new Exception("An error occurred while processing the product.");

            return modifiedProduct;
        },
        async (Exception reasonException) =>
        {
            Product revertedProduct =
                await this.productService.UndoLastChangedProductAsync(product);

            return revertedProduct;
        });

        public ValueTask<Product> RemoveOrRollbackProductAsync(Guid productId)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<Product> RemoveProductWithRollbackAsync(Guid productId)
        {
            var maybeDeletedImages = new List<ProductImage>();
            Product maybeDeletedProduct = null;

            return await TryCatchAndRollback(async () =>
            {
                ValidateProductId(productId);

                Product product =
                    await this.productService.RetrieveProductByIdAsync(productId);

                ValidateProductExists(product, productId);

                foreach (var image in product.ProductImages)
                {
                    ProductImage deletedImage =
                         await this.productImageService.RemoveProductImageByIdAsync(image.Id);

                    maybeDeletedImages.Add(deletedImage);
                }

                maybeDeletedProduct =
                    await this.productService.RemoveProductByIdAsync(productId);

                return maybeDeletedProduct;
            },
            async (Exception reasonException) =>
            {
                List<ProductImage> rolledBackImages =
                    await RollbackDeletedImagesAsync(maybeDeletedImages);

                // we can add dome validate here like: EnsureRolledBackImages(deletedImages, rolledBackImages)

                return await RollbackDeletedProductAsync(maybeDeletedProduct);
            });
        }

        private async ValueTask<List<ProductImage>> RollbackDeletedImagesAsync(IEnumerable<ProductImage> deletedImages)
        {
            var rolledBackImages = new List<ProductImage>();

            foreach (var image in deletedImages)
            {
                ProductImage rolledBackImage =
                     await this.productImageService.AddProductImageAsync(image);

                rolledBackImages.Add(rolledBackImage);

                // maybe we can add also some logging here 
            }

            return rolledBackImages;
        }

        private async ValueTask<Product> RollbackDeletedProductAsync(Product deletedProduct)
        {
            if (deletedProduct == null)
                return null;

            return await this.productService.AddProductAsync(deletedProduct);
        }
    }

}
