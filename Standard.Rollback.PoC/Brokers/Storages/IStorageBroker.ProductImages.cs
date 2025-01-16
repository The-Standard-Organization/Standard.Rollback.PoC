using System;
using System.Linq;
using System.Threading.Tasks;
using Standard.Rollback.PoC.Models.Foundations.ProductImages;

namespace Standard.Rollback.PoC.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<ProductImage> InsertProductImageAsync(ProductImage productImage);
        IQueryable<ProductImage> SelectAllProductImages();
        ValueTask<ProductImage> SelectProductImageByIdAsync(Guid productImageId);
        ValueTask<ProductImage> UpdateProductImageAsync(ProductImage productImage);
        ValueTask<ProductImage> DeleteProductImageAsync(ProductImage productImage);
        IQueryable<ProductImage> SelectProductImagesHistory();
        ValueTask<ProductImage> RevertLastProductImageChangeAsync(
            ProductImage productImage,
            ProductImage previousProductImage);
    }
}
