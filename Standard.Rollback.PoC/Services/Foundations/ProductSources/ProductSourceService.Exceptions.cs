using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Standard.Rollback.PoC.Models.Foundations.ProductSources;
using Standard.Rollback.PoC.Models.Foundations.ProductSources.Exceptions;
using Xeptions;

namespace Standard.Rollback.PoC.Services.Foundations.ProductSources
{
    public partial class ProductSourceService
    {
        private delegate ValueTask<ProductSource> ReturningProductSourceFunction();
        private delegate IQueryable<ProductSource> ReturningProductSourcesFunction();

        private async ValueTask<ProductSource> TryCatch(ReturningProductSourceFunction returningProductSourceFunction)
        {
            try
            {
                return await returningProductSourceFunction();
            }
            catch (NullProductSourceException nullProductSourceException)
            {
                throw CreateAndLogValidationException(nullProductSourceException);
            }
            catch (InvalidProductSourceException invalidProductSourceException)
            {
                throw CreateAndLogValidationException(invalidProductSourceException);
            }
            catch (SqlException sqlException)
            {
                var failedProductSourceStorageException =
                    new FailedProductSourceStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedProductSourceStorageException);
            }
            catch (NotFoundProductSourceException notFoundProductSourceException)
            {
                throw CreateAndLogValidationException(notFoundProductSourceException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsProductSourceException =
                    new AlreadyExistsProductSourceException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsProductSourceException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidProductSourceReferenceException =
                    new InvalidProductSourceReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidProductSourceReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedProductSourceException = new LockedProductSourceException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedProductSourceException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedProductSourceStorageException =
                    new FailedProductSourceStorageException(databaseUpdateException);

                throw CreateAndLogDependencyException(failedProductSourceStorageException);
            }
            catch (Exception exception)
            {
                var failedProductSourceServiceException =
                    new FailedProductSourceServiceException(exception);

                throw CreateAndLogServiceException(failedProductSourceServiceException);
            }
        }

        private IQueryable<ProductSource> TryCatch(ReturningProductSourcesFunction returningProductSourcesFunction)
        {
            try
            {
                return returningProductSourcesFunction();
            }
            catch (SqlException sqlException)
            {
                var failedProductSourceStorageException =
                    new FailedProductSourceStorageException(sqlException);
                throw CreateAndLogCriticalDependencyException(failedProductSourceStorageException);
            }
            catch (Exception exception)
            {
                var failedProductSourceServiceException =
                    new FailedProductSourceServiceException(exception);

                throw CreateAndLogServiceException(failedProductSourceServiceException);
            }
        }

        private ProductSourceValidationException CreateAndLogValidationException(Xeption exception)
        {
            var productSourceValidationException =
                new ProductSourceValidationException(exception);

            this.loggingBroker.LogError(productSourceValidationException);

            return productSourceValidationException;
        }

        private ProductSourceDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var productSourceDependencyException = new ProductSourceDependencyException(exception);
            this.loggingBroker.LogCritical(productSourceDependencyException);

            return productSourceDependencyException;
        }

        private ProductSourceDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var productSourceDependencyValidationException =
                new ProductSourceDependencyValidationException(exception);

            this.loggingBroker.LogError(productSourceDependencyValidationException);

            return productSourceDependencyValidationException;
        }

        private ProductSourceDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var productSourceDependencyException = new ProductSourceDependencyException(exception);
            this.loggingBroker.LogError(productSourceDependencyException);

            return productSourceDependencyException;
        }

        private ProductSourceServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var productSourceServiceException = new ProductSourceServiceException(exception);
            this.loggingBroker.LogError(productSourceServiceException);

            return productSourceServiceException;
        }
    }
}