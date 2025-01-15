using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Standard.Rollback.PoC.Brokers.DateTimes;
using Standard.Rollback.PoC.Brokers.Loggings;
using Standard.Rollback.PoC.Brokers.Storages;
using Standard.Rollback.PoC.Models.Foundations.Products;
using Standard.Rollback.PoC.Services.Foundations.ProductImages;
using Standard.Rollback.PoC.Services.Foundations.Products;
using Standard.Rollback.PoC.Services.Orchestrations.Products;

namespace Standard.Rollback.PoC.Tests.Manual
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var serviceProvider = ConfigureSerivces();

            // Resolve services
            var productService = serviceProvider.GetRequiredService<IProductService>();
            var productImageService = serviceProvider.GetRequiredService<IProductImageService>();
            var orchestrationService = serviceProvider.GetRequiredService<IProductOrchestrationService>();

            // Create a product
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Product 1"
            };

            // Add the product
            var addedProduct = await productService.AddProductAsync(product);

            // Modify the product
            addedProduct.Name = "Product 1 Modified";

            var modifiedProduct = await orchestrationService.ModifyOrRollbackProductAsync(addedProduct);

            Console.WriteLine($"Product modified: {modifiedProduct.Name}");
        }

        private static IServiceProvider ConfigureSerivces()
        {
            return new ServiceCollection()
                .AddSingleton<IDateTimeBroker, DateTimeBroker>()
                .AddSingleton<ILoggingBroker, LoggingBroker>()
                .AddSingleton<IStorageBroker, StorageBroker>()
                .AddSingleton<IProductService, ProductService>()
                .AddSingleton<IProductImageService, ProductImageService>()
                .AddSingleton<IProductOrchestrationService, ProductOrchestrationService>()
                .BuildServiceProvider();
        }
    }
}
