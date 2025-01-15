using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using Standard.Rollback.PoC.Models.Foundations.ProductSources;
using Standard.Rollback.PoC.Models.Foundations.ProductSources.Exceptions;
using Standard.Rollback.PoC.Services.Foundations.ProductSources;

namespace Standard.Rollback.PoC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductSourcesController : RESTFulController
    {
        private readonly IProductSourceService productSourceService;

        public ProductSourcesController(IProductSourceService productSourceService) =>
            this.productSourceService = productSourceService;

        [HttpPost]
        public async ValueTask<ActionResult<ProductSource>> PostProductSourceAsync(ProductSource productSource)
        {
            try
            {
                ProductSource addedProductSource =
                    await this.productSourceService.AddProductSourceAsync(productSource);

                return Created(addedProductSource);
            }
            catch (ProductSourceValidationException productSourceValidationException)
            {
                return BadRequest(productSourceValidationException.InnerException);
            }
            catch (ProductSourceDependencyValidationException productSourceValidationException)
                when (productSourceValidationException.InnerException is InvalidProductSourceReferenceException)
            {
                return FailedDependency(productSourceValidationException.InnerException);
            }
            catch (ProductSourceDependencyValidationException productSourceDependencyValidationException)
               when (productSourceDependencyValidationException.InnerException is AlreadyExistsProductSourceException)
            {
                return Conflict(productSourceDependencyValidationException.InnerException);
            }
            catch (ProductSourceDependencyException productSourceDependencyException)
            {
                return InternalServerError(productSourceDependencyException);
            }
            catch (ProductSourceServiceException productSourceServiceException)
            {
                return InternalServerError(productSourceServiceException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<ProductSource>> GetAllProductSources()
        {
            try
            {
                IQueryable<ProductSource> retrievedProductSources =
                    this.productSourceService.RetrieveAllProductSources();

                return Ok(retrievedProductSources);
            }
            catch (ProductSourceDependencyException productSourceDependencyException)
            {
                return InternalServerError(productSourceDependencyException);
            }
            catch (ProductSourceServiceException productSourceServiceException)
            {
                return InternalServerError(productSourceServiceException);
            }
        }

        [HttpGet("{productSourceId}")]
        public async ValueTask<ActionResult<ProductSource>> GetProductSourceByIdAsync(Guid productSourceId)
        {
            try
            {
                ProductSource productSource = await this.productSourceService.RetrieveProductSourceByIdAsync(productSourceId);

                return Ok(productSource);
            }
            catch (ProductSourceValidationException productSourceValidationException)
                when (productSourceValidationException.InnerException is NotFoundProductSourceException)
            {
                return NotFound(productSourceValidationException.InnerException);
            }
            catch (ProductSourceValidationException productSourceValidationException)
            {
                return BadRequest(productSourceValidationException.InnerException);
            }
            catch (ProductSourceDependencyException productSourceDependencyException)
            {
                return InternalServerError(productSourceDependencyException);
            }
            catch (ProductSourceServiceException productSourceServiceException)
            {
                return InternalServerError(productSourceServiceException);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<ProductSource>> PutProductSourceAsync(ProductSource productSource)
        {
            try
            {
                ProductSource modifiedProductSource =
                    await this.productSourceService.ModifyProductSourceAsync(productSource);

                return Ok(modifiedProductSource);
            }
            catch (ProductSourceValidationException productSourceValidationException)
                when (productSourceValidationException.InnerException is NotFoundProductSourceException)
            {
                return NotFound(productSourceValidationException.InnerException);
            }
            catch (ProductSourceValidationException productSourceValidationException)
            {
                return BadRequest(productSourceValidationException.InnerException);
            }
            catch (ProductSourceDependencyValidationException productSourceValidationException)
                when (productSourceValidationException.InnerException is InvalidProductSourceReferenceException)
            {
                return FailedDependency(productSourceValidationException.InnerException);
            }
            catch (ProductSourceDependencyValidationException productSourceDependencyValidationException)
               when (productSourceDependencyValidationException.InnerException is AlreadyExistsProductSourceException)
            {
                return Conflict(productSourceDependencyValidationException.InnerException);
            }
            catch (ProductSourceDependencyException productSourceDependencyException)
            {
                return InternalServerError(productSourceDependencyException);
            }
            catch (ProductSourceServiceException productSourceServiceException)
            {
                return InternalServerError(productSourceServiceException);
            }
        }

        [HttpDelete("{productSourceId}")]
        public async ValueTask<ActionResult<ProductSource>> DeleteProductSourceByIdAsync(Guid productSourceId)
        {
            try
            {
                ProductSource deletedProductSource =
                    await this.productSourceService.RemoveProductSourceByIdAsync(productSourceId);

                return Ok(deletedProductSource);
            }
            catch (ProductSourceValidationException productSourceValidationException)
                when (productSourceValidationException.InnerException is NotFoundProductSourceException)
            {
                return NotFound(productSourceValidationException.InnerException);
            }
            catch (ProductSourceValidationException productSourceValidationException)
            {
                return BadRequest(productSourceValidationException.InnerException);
            }
            catch (ProductSourceDependencyValidationException productSourceDependencyValidationException)
                when (productSourceDependencyValidationException.InnerException is LockedProductSourceException)
            {
                return Locked(productSourceDependencyValidationException.InnerException);
            }
            catch (ProductSourceDependencyValidationException productSourceDependencyValidationException)
            {
                return BadRequest(productSourceDependencyValidationException);
            }
            catch (ProductSourceDependencyException productSourceDependencyException)
            {
                return InternalServerError(productSourceDependencyException);
            }
            catch (ProductSourceServiceException productSourceServiceException)
            {
                return InternalServerError(productSourceServiceException);
            }
        }
    }
}