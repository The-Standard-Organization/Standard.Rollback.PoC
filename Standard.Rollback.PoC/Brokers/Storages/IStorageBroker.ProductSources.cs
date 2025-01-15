using System;
using System.Linq;
using System.Threading.Tasks;
using Standard.Rollback.PoC.Models.Foundations.ProductSources;

namespace Standard.Rollback.PoC.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<ProductSource> InsertProductSourceAsync(ProductSource productSource);
        IQueryable<ProductSource> SelectAllProductSources();
        ValueTask<ProductSource> SelectProductSourceByIdAsync(Guid productSourceId);
        ValueTask<ProductSource> UpdateProductSourceAsync(ProductSource productSource);
        ValueTask<ProductSource> DeleteProductSourceAsync(ProductSource productSource);
    }
}
