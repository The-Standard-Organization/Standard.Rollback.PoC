using System;
using System.Linq;
using System.Threading.Tasks;
using Standard.Rollback.PoC.Brokers.DateTimes;
using Standard.Rollback.PoC.Brokers.Loggings;
using Standard.Rollback.PoC.Brokers.Storages;
using Standard.Rollback.PoC.Models.Foundations.Products;
using Standard.Rollback.PoC.Services.Foundations.Products;

namespace Standard.Rollback.PoC.Services.Foundations.Products
{
    public partial class ProductService : IProductService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public ProductService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Product> AddProductAsync(Product product) =>
            TryCatch(async () =>
            {
                ValidateProductOnAdd(product);

                return await this.storageBroker.InsertProductAsync(product);
            });

        public IQueryable<Product> RetrieveAllProducts() =>
            TryCatch(() => this.storageBroker.SelectAllProducts());

        public ValueTask<Product> RetrieveProductByIdAsync(Guid productId) =>
            TryCatch(async () =>
            {
                ValidateProductId(productId);

                Product maybeProduct = await this.storageBroker
                    .SelectProductByIdAsync(productId);

                ValidateStorageProduct(maybeProduct, productId);

                return maybeProduct;
            });

        public ValueTask<Product> ModifyProductAsync(Product product) =>
            TryCatch(async () =>
            {
                ValidateProductOnModify(product);

                Product maybeProduct =
                    await this.storageBroker.SelectProductByIdAsync(product.Id);

                ValidateStorageProduct(maybeProduct, product.Id);

                return await this.storageBroker.UpdateProductAsync(product);
            });

        public ValueTask<Product> RemoveProductByIdAsync(Guid productId) =>
            TryCatch(async () =>
            {
                ValidateProductId(productId);

                Product maybeProduct = await this.storageBroker
                    .SelectProductByIdAsync(productId);

                ValidateStorageProduct(maybeProduct, productId);

                return await this.storageBroker.DeleteProductAsync(maybeProduct);
            });
    }
}