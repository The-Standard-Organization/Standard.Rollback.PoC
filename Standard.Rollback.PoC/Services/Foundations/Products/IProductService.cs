using System;
using System.Linq;
using System.Threading.Tasks;
using Standard.Rollback.PoC.Models.Foundations.Products;

namespace Standard.Rollback.PoC.Services.Foundations.Products
{
    public interface IProductService
    {
        ValueTask<Product> AddProductAsync(Product product);
        IQueryable<Product> RetrieveAllProducts();
        ValueTask<Product> RetrieveProductByIdAsync(Guid productId);
        ValueTask<Product> ModifyProductAsync(Product product);
        ValueTask<Product> RemoveProductByIdAsync(Guid productId);
        ValueTask<Product> LockProductAsync(Product product);
        ValueTask<Product> UnlockProductAsync(Product product);
        ValueTask<Product> UndoLastChangedProductAsync(Product product);
    }
}