using System;
using System.Linq;
using System.Threading.Tasks;
using Standard.Rollback.PoC.Brokers.DateTimes;
using Standard.Rollback.PoC.Brokers.Loggings;
using Standard.Rollback.PoC.Brokers.Storages;
using Standard.Rollback.PoC.Models.Foundations.ProductSources;

namespace Standard.Rollback.PoC.Services.Foundations.ProductSources
{
    public partial class ProductSourceService : IProductSourceService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public ProductSourceService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<ProductSource> AddProductSourceAsync(ProductSource productSource) =>
            TryCatch(async () =>
            {
                ValidateProductSourceOnAdd(productSource);

                return await this.storageBroker.InsertProductSourceAsync(productSource);
            });

        public IQueryable<ProductSource> RetrieveAllProductSources() =>
            TryCatch(() => this.storageBroker.SelectAllProductSources());

        public ValueTask<ProductSource> RetrieveProductSourceByIdAsync(Guid productSourceId) =>
            TryCatch(async () =>
            {
                ValidateProductSourceId(productSourceId);

                ProductSource maybeProductSource = await this.storageBroker
                    .SelectProductSourceByIdAsync(productSourceId);

                ValidateStorageProductSource(maybeProductSource, productSourceId);

                return maybeProductSource;
            });

        public ValueTask<ProductSource> ModifyProductSourceAsync(ProductSource productSource) =>
            TryCatch(async () =>
            {
                ValidateProductSourceOnModify(productSource);

                ProductSource maybeProductSource =
                    await this.storageBroker.SelectProductSourceByIdAsync(productSource.Id);

                ValidateStorageProductSource(maybeProductSource, productSource.Id);

                return await this.storageBroker.UpdateProductSourceAsync(productSource);
            });

        public ValueTask<ProductSource> RemoveProductSourceByIdAsync(Guid productSourceId) =>
            TryCatch(async () =>
            {
                ValidateProductSourceId(productSourceId);

                ProductSource maybeProductSource = await this.storageBroker
                    .SelectProductSourceByIdAsync(productSourceId);

                ValidateStorageProductSource(maybeProductSource, productSourceId);

                return await this.storageBroker.DeleteProductSourceAsync(maybeProductSource);
            });
    }
}