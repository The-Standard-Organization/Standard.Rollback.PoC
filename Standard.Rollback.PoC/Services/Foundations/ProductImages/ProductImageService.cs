using System;
using System.Linq;
using System.Threading.Tasks;
using Standard.Rollback.PoC.Brokers.DateTimes;
using Standard.Rollback.PoC.Brokers.Loggings;
using Standard.Rollback.PoC.Brokers.Storages;
using Standard.Rollback.PoC.Models.Foundations.ProductImages;

namespace Standard.Rollback.PoC.Services.Foundations.ProductImages
{
    public partial class ProductImageService : IProductImageService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public ProductImageService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<ProductImage> AddProductImageAsync(ProductImage productImage) =>
            TryCatch(async () =>
            {
                ValidateProductImageOnAdd(productImage);

                return await this.storageBroker.InsertProductImageAsync(productImage);
            });

        public IQueryable<ProductImage> RetrieveAllProductImages() =>
            TryCatch(() => this.storageBroker.SelectAllProductImages());

        public ValueTask<ProductImage> RetrieveProductImageByIdAsync(Guid productImageId) =>
            TryCatch(async () =>
            {
                ValidateProductImageId(productImageId);

                ProductImage maybeProductImage = await this.storageBroker
                    .SelectProductImageByIdAsync(productImageId);

                ValidateStorageProductImage(maybeProductImage, productImageId);

                return maybeProductImage;
            });

        public ValueTask<ProductImage> ModifyProductImageAsync(ProductImage productImage) =>
            TryCatch(async () =>
            {
                ValidateProductImageOnModify(productImage);

                ProductImage maybeProductImage =
                    await this.storageBroker.SelectProductImageByIdAsync(productImage.Id);

                ValidateStorageProductImage(maybeProductImage, productImage.Id);

                return await this.storageBroker.UpdateProductImageAsync(productImage);
            });

        public ValueTask<ProductImage> RemoveProductImageByIdAsync(Guid productImageId) =>
            TryCatch(async () =>
            {
                ValidateProductImageId(productImageId);

                ProductImage maybeProductImage = await this.storageBroker
                    .SelectProductImageByIdAsync(productImageId);

                ValidateStorageProductImage(maybeProductImage, productImageId);

                return await this.storageBroker.DeleteProductImageAsync(maybeProductImage);
            });

        public ValueTask<ProductImage> UndoLastChangedProductImageAsync(Guid productImageId) =>
        TryCatch(async () =>
        {
            ValidateProductImageId(productImageId);

            ProductImage maybeProductImage = await this.storageBroker
                .SelectProductImageByIdAsync(productImageId);

            ValidateStorageProductImage(maybeProductImage, productImageId);

            ProductImage lastProductImageChange = await this.storageBroker
                .SelectLastProductImageChangeAsync(productImageId);

            ValidateStorageProductImage(maybeProductImage, productImageId);

            return await this.storageBroker.RevertLastProductImageChangeAsync(
                maybeProductImage,
                lastProductImageChange);
        });
    }
}