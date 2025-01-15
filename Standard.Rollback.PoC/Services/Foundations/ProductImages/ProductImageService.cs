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

        public ValueTask<ProductImage> LockProductImageAsync(ProductImage productImage) =>
        TryCatch(async () =>
        {
            productImage.IsLocked = true;
            productImage.LockedDate =
                this.dateTimeBroker.GetCurrentDateTimeOffset();

            return await this.storageBroker.UpdateProductImageAsync(productImage);
        });

        public ValueTask<ProductImage> UnlockProductImageAsync(ProductImage productImage) =>
        TryCatch(async () =>
        {
            productImage.IsLocked = false;

            return await this.storageBroker.UpdateProductImageAsync(productImage);
        });

        public ValueTask<ProductImage> UndoLastChangedProductImageAsync(ProductImage productImage) =>
        TryCatch(async () =>
        {
            ProductImage lastProductImageChange = await this.storageBroker
                .SelectLastProductImageChangeAsync(productImage.Id);

            return await this.storageBroker.RevertLastProductImageChangeAsync(
                productImage,
                lastProductImageChange);
        });
    }
}