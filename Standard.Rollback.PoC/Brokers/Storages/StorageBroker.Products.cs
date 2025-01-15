using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Standard.Rollback.PoC.Models.Foundations.Products;

namespace Standard.Rollback.PoC.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Product> Products { get; set; }

        public async ValueTask<Product> InsertProductAsync(Product product) =>
            await InsertAsync(product);

        public IQueryable<Product> SelectAllProducts() =>
            SelectAll<Product>();

        public async ValueTask<Product> SelectProductByIdAsync(Guid productId) =>
            await SelectAsync<Product>(productId);

        public async ValueTask<Product> UpdateProductAsync(Product product) =>
            await UpdateAsync(product);

        public async ValueTask<Product> DeleteProductAsync(Product product) =>
            await DeleteAsync(product);

        internal void ConfigureProducts(EntityTypeBuilder<Product> builder)
        {
            // TO DO: Configure the Product entity
        }
    }
}
