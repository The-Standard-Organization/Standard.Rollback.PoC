using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Standard.Rollback.PoC.Models.Foundations.ProductImages;
using Standard.Rollback.PoC.Models.Foundations.Products;
using Standard.Rollback.PoC.Models.Foundations.ProductSources;

namespace Standard.Rollback.PoC.Brokers.Storages
{
    public partial class StorageBroker : EFxceptionsContext, IStorageBroker
    {
        private readonly IConfiguration configuration;

        public StorageBroker(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureProductSources(modelBuilder.Entity<ProductSource>());
            ConfigureProductImages(modelBuilder.Entity<ProductImage>());

            CreateTemporalTables(modelBuilder);
        }

        private static void CreateTemporalTables(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .ToTable("Products", productsTable => productsTable.IsTemporal());

            modelBuilder.Entity<ProductSource>()
                .ToTable("ProductSources", productSourcesTable => productSourcesTable.IsTemporal());

            modelBuilder.Entity<ProductImage>()
                .ToTable("ProductImages", productImagesTable => productImagesTable.IsTemporal());
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            string connectionString = this.configuration
                .GetConnectionString(name: "DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);
        }

        private async ValueTask<T> InsertAsync<T>(T @object)
        {
            var broker = new StorageBroker(this.configuration);
            broker.Entry(@object).State = EntityState.Added;
            await broker.SaveChangesAsync();

            return @object;
        }

        private IQueryable<T> SelectAll<T>() where T : class
        {
            var broker = new StorageBroker(this.configuration);

            return broker.Set<T>();
        }

        private async ValueTask<T> SelectAsync<T>(params object[] objectIds) where T : class
        {
            var broker = new StorageBroker(this.configuration);

            return await broker.FindAsync<T>(objectIds);
        }

        private async ValueTask<T> UpdateAsync<T>(T @object)
        {
            var broker = new StorageBroker(this.configuration);
            broker.Entry(@object).State |= EntityState.Modified;
            await broker.SaveChangesAsync();

            return @object;
        }

        private async ValueTask<T> DeleteAsync<T>(T @object)
        {
            var broker = new StorageBroker(this.configuration);
            broker.Entry(@object).State = EntityState.Deleted;
            await broker.SaveChangesAsync();

            return @object;
        }

        private async ValueTask<T> RevertAsync<T>(T @object, T previousObject)
        {
            var broker = new StorageBroker(this.configuration);
            broker.Entry(@object).CurrentValues.SetValues(previousObject);
            await broker.SaveChangesAsync();

            return previousObject;
        }

        private async ValueTask<T> GetPreviousVersionAsync<T>(Guid objectId, string tableName) where T : class
        {
            var broker = new StorageBroker(this.configuration);

            return await broker.Set<T>()
                .FromSqlInterpolated($@"
                    SELECT TOP 1 * 
                    FROM {tableName} FOR SYSTEM_TIME ALL 
                    WHERE Id = '{objectId}' AND SysEndTime < SYSUTCDATETIME()
                    ORDER BY SysEndTime DESC")
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }


        public override void Dispose() { }
    }
}