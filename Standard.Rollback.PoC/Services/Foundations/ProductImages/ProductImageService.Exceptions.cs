using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Standard.Rollback.PoC.Models.Foundations.ProductImages;
using Standard.Rollback.PoC.Models.Foundations.ProductImages.Exceptions;
using Xeptions;

namespace Standard.Rollback.PoC.Services.Foundations.ProductImages
{
    public partial class ProductImageService
    {
        private delegate ValueTask<ProductImage> ReturningProductImageFunction();
        private delegate IQueryable<ProductImage> ReturningProductImagesFunction();

        private async ValueTask<ProductImage> TryCatch(ReturningProductImageFunction returningProductImageFunction)
        {
            try
            {
                return await returningProductImageFunction();
            }
            catch (NullProductImageException nullProductImageException)
            {
                throw CreateAndLogValidationException(nullProductImageException);
            }
            catch (InvalidProductImageException invalidProductImageException)
            {
                throw CreateAndLogValidationException(invalidProductImageException);
            }
            catch (SqlException sqlException)
            {
                var failedProductImageStorageException =
                    new FailedProductImageStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedProductImageStorageException);
            }
            catch (NotFoundProductImageException notFoundProductImageException)
            {
                throw CreateAndLogValidationException(notFoundProductImageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsProductImageException =
                    new AlreadyExistsProductImageException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsProductImageException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidProductImageReferenceException =
                    new InvalidProductImageReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidProductImageReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedProductImageException = new LockedProductImageException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedProductImageException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedProductImageStorageException =
                    new FailedProductImageStorageException(databaseUpdateException);

                throw CreateAndLogDependencyException(failedProductImageStorageException);
            }
            catch (Exception exception)
            {
                var failedProductImageServiceException =
                    new FailedProductImageServiceException(exception);

                throw CreateAndLogServiceException(failedProductImageServiceException);
            }
        }

        private IQueryable<ProductImage> TryCatch(ReturningProductImagesFunction returningProductImagesFunction)
        {
            try
            {
                return returningProductImagesFunction();
            }
            catch (SqlException sqlException)
            {
                var failedProductImageStorageException =
                    new FailedProductImageStorageException(sqlException);
                throw CreateAndLogCriticalDependencyException(failedProductImageStorageException);
            }
            catch (Exception exception)
            {
                var failedProductImageServiceException =
                    new FailedProductImageServiceException(exception);

                throw CreateAndLogServiceException(failedProductImageServiceException);
            }
        }

        private ProductImageValidationException CreateAndLogValidationException(Xeption exception)
        {
            var productImageValidationException =
                new ProductImageValidationException(exception);

            this.loggingBroker.LogError(productImageValidationException);

            return productImageValidationException;
        }

        private ProductImageDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var productImageDependencyException = new ProductImageDependencyException(exception);
            this.loggingBroker.LogCritical(productImageDependencyException);

            return productImageDependencyException;
        }

        private ProductImageDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var productImageDependencyValidationException =
                new ProductImageDependencyValidationException(exception);

            this.loggingBroker.LogError(productImageDependencyValidationException);

            return productImageDependencyValidationException;
        }

        private ProductImageDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var productImageDependencyException = new ProductImageDependencyException(exception);
            this.loggingBroker.LogError(productImageDependencyException);

            return productImageDependencyException;
        }

        private ProductImageServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var productImageServiceException = new ProductImageServiceException(exception);
            this.loggingBroker.LogError(productImageServiceException);

            return productImageServiceException;
        }
    }
}