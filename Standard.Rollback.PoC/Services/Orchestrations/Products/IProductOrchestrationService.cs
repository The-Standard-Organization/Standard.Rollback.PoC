using System;
using System.Threading.Tasks;
using Standard.Rollback.PoC.Models.Foundations.Products;

namespace Standard.Rollback.PoC.Services.Orchestrations.Products
{
    public interface IProductOrchestrationService
    {
        ValueTask<Product> RemoveProductWithRollbackAsync(Guid productId);
    }
}
