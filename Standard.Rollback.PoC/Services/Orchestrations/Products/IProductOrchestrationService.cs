using System;
using System.Threading.Tasks;
using Standard.Rollback.PoC.Models.Foundations.Products;

namespace Standard.Rollback.PoC.Services.Orchestrations.Products
{
    public interface IProductOrchestrationService
    {
        ValueTask<Product> ModifyOrRollbackProductAsync(Product product);
        ValueTask<Product> RemoveOrRollbackProductAsync(Guid productId);
    }
}
