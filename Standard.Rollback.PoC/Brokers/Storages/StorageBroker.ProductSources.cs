using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Standard.Rollback.PoC.Models.Foundations.ProductSources;

namespace Standard.Rollback.PoC.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<ProductSource> ProductSources { get; set; }

        public async ValueTask<ProductSource> InsertProductSourceAsync(ProductSource productSource) =>
            await InsertAsync(productSource);

        public IQueryable<ProductSource> SelectAllProductSources() =>
            SelectAll<ProductSource>();

        public async ValueTask<ProductSource> SelectProductSourceByIdAsync(Guid productSourceId) =>
            await SelectAsync<ProductSource>(productSourceId);

        public async ValueTask<ProductSource> UpdateProductSourceAsync(ProductSource productSource) =>
            await UpdateAsync(productSource);

        public async ValueTask<ProductSource> DeleteProductSourceAsync(ProductSource productSource) =>
            await DeleteAsync(productSource);

        internal void ConfigureProductSources(EntityTypeBuilder<ProductSource> builder)
        {
            builder.HasOne(source => source.Product)
                .WithMany(product => product.ProductSources)
                .HasForeignKey(source => source.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
