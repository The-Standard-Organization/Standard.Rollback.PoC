using System;
using System.Linq;
using System.Threading.Tasks;
using Standard.Rollback.PoC.Models.Foundations.ProductImages;

namespace Standard.Rollback.PoC.Services.Foundations.ProductImages
{
    public interface IProductImageService
    {
        ValueTask<ProductImage> AddProductImageAsync(ProductImage productImage);
        IQueryable<ProductImage> RetrieveAllProductImages();
        ValueTask<ProductImage> RetrieveProductImageByIdAsync(Guid productImageId);
        ValueTask<ProductImage> ModifyProductImageAsync(ProductImage productImage);
        ValueTask<ProductImage> RemoveProductImageByIdAsync(Guid productImageId);
        ValueTask<ProductImage> LockProductImageAsync(ProductImage productImage);
        ValueTask<ProductImage> UnlockProductImageAsync(ProductImage productImage);
        ValueTask<ProductImage> UndoLastChangedProductImageAsync(ProductImage productImage);
    }
}