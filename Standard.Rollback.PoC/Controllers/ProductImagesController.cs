using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using Standard.Rollback.PoC.Models.Foundations.ProductImages;
using Standard.Rollback.PoC.Models.Foundations.ProductImages.Exceptions;
using Standard.Rollback.PoC.Services.Foundations.ProductImages;

namespace Standard.Rollback.PoC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductImagesController : RESTFulController
    {
        private readonly IProductImageService productImageService;

        public ProductImagesController(IProductImageService productImageService) =>
            this.productImageService = productImageService;

        [HttpPost]
        public async ValueTask<ActionResult<ProductImage>> PostProductImageAsync(ProductImage productImage)
        {
            try
            {
                ProductImage addedProductImage =
                    await this.productImageService.AddProductImageAsync(productImage);

                return Created(addedProductImage);
            }
            catch (ProductImageValidationException productImageValidationException)
            {
                return BadRequest(productImageValidationException.InnerException);
            }
            catch (ProductImageDependencyValidationException productImageValidationException)
                when (productImageValidationException.InnerException is InvalidProductImageReferenceException)
            {
                return FailedDependency(productImageValidationException.InnerException);
            }
            catch (ProductImageDependencyValidationException productImageDependencyValidationException)
               when (productImageDependencyValidationException.InnerException is AlreadyExistsProductImageException)
            {
                return Conflict(productImageDependencyValidationException.InnerException);
            }
            catch (ProductImageDependencyException productImageDependencyException)
            {
                return InternalServerError(productImageDependencyException);
            }
            catch (ProductImageServiceException productImageServiceException)
            {
                return InternalServerError(productImageServiceException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<ProductImage>> GetAllProductImages()
        {
            try
            {
                IQueryable<ProductImage> retrievedProductImages =
                    this.productImageService.RetrieveAllProductImages();

                return Ok(retrievedProductImages);
            }
            catch (ProductImageDependencyException productImageDependencyException)
            {
                return InternalServerError(productImageDependencyException);
            }
            catch (ProductImageServiceException productImageServiceException)
            {
                return InternalServerError(productImageServiceException);
            }
        }

        [HttpGet("{productImageId}")]
        public async ValueTask<ActionResult<ProductImage>> GetProductImageByIdAsync(Guid productImageId)
        {
            try
            {
                ProductImage productImage = await this.productImageService.RetrieveProductImageByIdAsync(productImageId);

                return Ok(productImage);
            }
            catch (ProductImageValidationException productImageValidationException)
                when (productImageValidationException.InnerException is NotFoundProductImageException)
            {
                return NotFound(productImageValidationException.InnerException);
            }
            catch (ProductImageValidationException productImageValidationException)
            {
                return BadRequest(productImageValidationException.InnerException);
            }
            catch (ProductImageDependencyException productImageDependencyException)
            {
                return InternalServerError(productImageDependencyException);
            }
            catch (ProductImageServiceException productImageServiceException)
            {
                return InternalServerError(productImageServiceException);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<ProductImage>> PutProductImageAsync(ProductImage productImage)
        {
            try
            {
                ProductImage modifiedProductImage =
                    await this.productImageService.ModifyProductImageAsync(productImage);

                return Ok(modifiedProductImage);
            }
            catch (ProductImageValidationException productImageValidationException)
                when (productImageValidationException.InnerException is NotFoundProductImageException)
            {
                return NotFound(productImageValidationException.InnerException);
            }
            catch (ProductImageValidationException productImageValidationException)
            {
                return BadRequest(productImageValidationException.InnerException);
            }
            catch (ProductImageDependencyValidationException productImageValidationException)
                when (productImageValidationException.InnerException is InvalidProductImageReferenceException)
            {
                return FailedDependency(productImageValidationException.InnerException);
            }
            catch (ProductImageDependencyValidationException productImageDependencyValidationException)
               when (productImageDependencyValidationException.InnerException is AlreadyExistsProductImageException)
            {
                return Conflict(productImageDependencyValidationException.InnerException);
            }
            catch (ProductImageDependencyException productImageDependencyException)
            {
                return InternalServerError(productImageDependencyException);
            }
            catch (ProductImageServiceException productImageServiceException)
            {
                return InternalServerError(productImageServiceException);
            }
        }

        [HttpDelete("{productImageId}")]
        public async ValueTask<ActionResult<ProductImage>> DeleteProductImageByIdAsync(Guid productImageId)
        {
            try
            {
                ProductImage deletedProductImage =
                    await this.productImageService.RemoveProductImageByIdAsync(productImageId);

                return Ok(deletedProductImage);
            }
            catch (ProductImageValidationException productImageValidationException)
                when (productImageValidationException.InnerException is NotFoundProductImageException)
            {
                return NotFound(productImageValidationException.InnerException);
            }
            catch (ProductImageValidationException productImageValidationException)
            {
                return BadRequest(productImageValidationException.InnerException);
            }
            catch (ProductImageDependencyValidationException productImageDependencyValidationException)
                when (productImageDependencyValidationException.InnerException is LockedProductImageException)
            {
                return Locked(productImageDependencyValidationException.InnerException);
            }
            catch (ProductImageDependencyValidationException productImageDependencyValidationException)
            {
                return BadRequest(productImageDependencyValidationException);
            }
            catch (ProductImageDependencyException productImageDependencyException)
            {
                return InternalServerError(productImageDependencyException);
            }
            catch (ProductImageServiceException productImageServiceException)
            {
                return InternalServerError(productImageServiceException);
            }
        }
    }
}