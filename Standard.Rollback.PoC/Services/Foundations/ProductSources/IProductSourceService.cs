using System;
using System.Linq;
using System.Threading.Tasks;
using Standard.Rollback.PoC.Models.Foundations.ProductSources;

namespace Standard.Rollback.PoC.Services.Foundations.ProductSources
{
    public interface IProductSourceService
    {
        ValueTask<ProductSource> AddProductSourceAsync(ProductSource productSource);
        IQueryable<ProductSource> RetrieveAllProductSources();
        ValueTask<ProductSource> RetrieveProductSourceByIdAsync(Guid productSourceId);
        ValueTask<ProductSource> ModifyProductSourceAsync(ProductSource productSource);
        ValueTask<ProductSource> RemoveProductSourceByIdAsync(Guid productSourceId);
    }
}