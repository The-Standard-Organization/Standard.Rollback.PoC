
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Standard.Rollback.PoC.Brokers.DateTimes;
using Standard.Rollback.PoC.Brokers.Loggings;
using Standard.Rollback.PoC.Brokers.Storages;
using Standard.Rollback.PoC.Services.Foundations.ProductImages;
using Standard.Rollback.PoC.Services.Foundations.Products;
using Standard.Rollback.PoC.Services.Foundations.ProductSources;
using Standard.Rollback.PoC.Services.Orchestrations.Products;

namespace Standard.Rollback.PoC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddTransient<ILoggingBroker, LoggingBroker>();
            builder.Services.AddTransient<IDateTimeBroker, DateTimeBroker>();
            builder.Services.AddTransient<IStorageBroker, StorageBroker>();
            builder.Services.AddTransient<IProductService, ProductService>();
            builder.Services.AddTransient<IProductImageService, ProductImageService>();
            builder.Services.AddTransient<IProductSourceService, ProductSourceService>();
            builder.Services.AddTransient<IProductOrchestrationService, ProductOrchestrationService>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.MapControllers();
            app.Run();
        }
    }
}
