using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using Standard.Rollback.PoC.Models.Foundations.Products;
using Standard.Rollback.PoC.Models.Foundations.Products.Exceptions;
using Standard.Rollback.PoC.Services.Foundations.Products;

namespace Standard.Rollback.PoC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : RESTFulController
    {
        private readonly IProductService productService;

        public ProductsController(IProductService productService) =>
            this.productService = productService;

        [HttpPost]
        public async ValueTask<ActionResult<Product>> PostProductAsync(Product product)
        {
            try
            {
                Product addedProduct =
                    await this.productService.AddProductAsync(product);

                return Created(addedProduct);
            }
            catch (ProductValidationException productValidationException)
            {
                return BadRequest(productValidationException.InnerException);
            }
            catch (ProductDependencyValidationException productValidationException)
                when (productValidationException.InnerException is InvalidProductReferenceException)
            {
                return FailedDependency(productValidationException.InnerException);
            }
            catch (ProductDependencyValidationException productDependencyValidationException)
               when (productDependencyValidationException.InnerException is AlreadyExistsProductException)
            {
                return Conflict(productDependencyValidationException.InnerException);
            }
            catch (ProductDependencyException productDependencyException)
            {
                return InternalServerError(productDependencyException);
            }
            catch (ProductServiceException productServiceException)
            {
                return InternalServerError(productServiceException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<Product>> GetAllProducts()
        {
            try
            {
                IQueryable<Product> retrievedProducts =
                    this.productService.RetrieveAllProducts();

                return Ok(retrievedProducts);
            }
            catch (ProductDependencyException productDependencyException)
            {
                return InternalServerError(productDependencyException);
            }
            catch (ProductServiceException productServiceException)
            {
                return InternalServerError(productServiceException);
            }
        }

        [HttpGet("{productId}")]
        public async ValueTask<ActionResult<Product>> GetProductByIdAsync(Guid productId)
        {
            try
            {
                Product product = await this.productService.RetrieveProductByIdAsync(productId);

                return Ok(product);
            }
            catch (ProductValidationException productValidationException)
                when (productValidationException.InnerException is NotFoundProductException)
            {
                return NotFound(productValidationException.InnerException);
            }
            catch (ProductValidationException productValidationException)
            {
                return BadRequest(productValidationException.InnerException);
            }
            catch (ProductDependencyException productDependencyException)
            {
                return InternalServerError(productDependencyException);
            }
            catch (ProductServiceException productServiceException)
            {
                return InternalServerError(productServiceException);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<Product>> PutProductAsync(Product product)
        {
            try
            {
                Product modifiedProduct =
                    await this.productService.ModifyProductAsync(product);

                return Ok(modifiedProduct);
            }
            catch (ProductValidationException productValidationException)
                when (productValidationException.InnerException is NotFoundProductException)
            {
                return NotFound(productValidationException.InnerException);
            }
            catch (ProductValidationException productValidationException)
            {
                return BadRequest(productValidationException.InnerException);
            }
            catch (ProductDependencyValidationException productValidationException)
                when (productValidationException.InnerException is InvalidProductReferenceException)
            {
                return FailedDependency(productValidationException.InnerException);
            }
            catch (ProductDependencyValidationException productDependencyValidationException)
               when (productDependencyValidationException.InnerException is AlreadyExistsProductException)
            {
                return Conflict(productDependencyValidationException.InnerException);
            }
            catch (ProductDependencyException productDependencyException)
            {
                return InternalServerError(productDependencyException);
            }
            catch (ProductServiceException productServiceException)
            {
                return InternalServerError(productServiceException);
            }
        }

        [HttpDelete("{productId}")]
        public async ValueTask<ActionResult<Product>> DeleteProductByIdAsync(Guid productId)
        {
            try
            {
                Product deletedProduct =
                    await this.productService.RemoveProductByIdAsync(productId);

                return Ok(deletedProduct);
            }
            catch (ProductValidationException productValidationException)
                when (productValidationException.InnerException is NotFoundProductException)
            {
                return NotFound(productValidationException.InnerException);
            }
            catch (ProductValidationException productValidationException)
            {
                return BadRequest(productValidationException.InnerException);
            }
            catch (ProductDependencyValidationException productDependencyValidationException)
                when (productDependencyValidationException.InnerException is LockedProductException)
            {
                return Locked(productDependencyValidationException.InnerException);
            }
            catch (ProductDependencyValidationException productDependencyValidationException)
            {
                return BadRequest(productDependencyValidationException);
            }
            catch (ProductDependencyException productDependencyException)
            {
                return InternalServerError(productDependencyException);
            }
            catch (ProductServiceException productServiceException)
            {
                return InternalServerError(productServiceException);
            }
        }
    }
}