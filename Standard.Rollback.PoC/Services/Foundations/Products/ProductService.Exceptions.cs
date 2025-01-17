using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Standard.Rollback.PoC.Models.Foundations.Products;
using Standard.Rollback.PoC.Models.Foundations.Products.Exceptions;
using Xeptions;

namespace Standard.Rollback.PoC.Services.Foundations.Products
{
    public partial class ProductService
    {
        private delegate ValueTask<Product> ReturningProductFunction();
        private delegate IQueryable<Product> ReturningProductsFunction();

        private async ValueTask<Product> TryCatch(ReturningProductFunction returningProductFunction)
        {
            try
            {
                return await returningProductFunction();
            }
            catch (NullProductException nullProductException)
            {
                throw CreateAndLogValidationException(nullProductException);
            }
            catch (InvalidProductException invalidProductException)
            {
                throw CreateAndLogValidationException(invalidProductException);
            }
            catch (SqlException sqlException)
            {
                var failedProductStorageException =
                    new FailedProductStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedProductStorageException);
            }
            catch (NotFoundProductException notFoundProductException)
            {
                throw CreateAndLogValidationException(notFoundProductException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsProductException =
                    new AlreadyExistsProductException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsProductException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidProductReferenceException =
                    new InvalidProductReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidProductReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedProductException = new LockedProductException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedProductException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedProductStorageException =
                    new FailedProductStorageException(databaseUpdateException);

                throw CreateAndLogDependencyException(failedProductStorageException);
            }
            catch (Exception exception)
            {
                var failedProductServiceException =
                    new FailedProductServiceException(exception);

                throw CreateAndLogServiceException(failedProductServiceException);
            }
        }

        private IQueryable<Product> TryCatch(ReturningProductsFunction returningProductsFunction)
        {
            try
            {
                return returningProductsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedProductStorageException =
                    new FailedProductStorageException(sqlException);
                throw CreateAndLogCriticalDependencyException(failedProductStorageException);
            }
            catch (Exception exception)
            {
                var failedProductServiceException =
                    new FailedProductServiceException(exception);

                throw CreateAndLogServiceException(failedProductServiceException);
            }
        }

        private ProductValidationException CreateAndLogValidationException(Xeption exception)
        {
            var productValidationException =
                new ProductValidationException(exception);

            this.loggingBroker.LogError(productValidationException);

            return productValidationException;
        }

        private ProductDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var productDependencyException = new ProductDependencyException(exception);
            this.loggingBroker.LogCritical(productDependencyException);

            return productDependencyException;
        }

        private ProductDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var productDependencyValidationException =
                new ProductDependencyValidationException(exception);

            this.loggingBroker.LogError(productDependencyValidationException);

            return productDependencyValidationException;
        }

        private ProductDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var productDependencyException = new ProductDependencyException(exception);
            this.loggingBroker.LogError(productDependencyException);

            return productDependencyException;
        }

        private ProductServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var productServiceException = new ProductServiceException(exception);
            this.loggingBroker.LogError(productServiceException);

            return productServiceException;
        }
    }
}