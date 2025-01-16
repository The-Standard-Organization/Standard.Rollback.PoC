using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Standard.Rollback.PoC.Models.Foundations.ProductImages;

namespace Standard.Rollback.PoC.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<ProductImage> ProductImages { get; set; }

        public async ValueTask<ProductImage> InsertProductImageAsync(ProductImage productImage) =>
            await InsertAsync(productImage);

        public IQueryable<ProductImage> SelectAllProductImages() =>
            SelectAll<ProductImage>();

        public async ValueTask<ProductImage> SelectProductImageByIdAsync(Guid productImageId) =>
            await SelectAsync<ProductImage>(productImageId);

        public async ValueTask<ProductImage> UpdateProductImageAsync(ProductImage productImage) =>
            await UpdateAsync(productImage);

        public async ValueTask<ProductImage> DeleteProductImageAsync(ProductImage productImage) =>
            await DeleteAsync(productImage);

        public IQueryable<ProductImage> SelectProductImagesHistory() =>
            SelectHistory<ProductImage>();

        public async ValueTask<ProductImage> RevertLastProductImageChangeAsync(
            ProductImage productImage,
            ProductImage previousProductImage)
        {
            return await RevertAsync(productImage, previousProductImage);
        }

        internal void ConfigureProductImages(EntityTypeBuilder<ProductImage> builder)
        {
            builder.HasOne(image => image.Product)
                 .WithMany(product => product.ProductImages)
                 .HasForeignKey(image => image.ProductId)
                 .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
