using System;
using System.Threading.Tasks;
using Standard.Rollback.PoC.Models.Foundations.Products;
using Standard.Rollback.PoC.Models.Foundations.Products.Exceptions;

namespace Standard.Rollback.PoC.Services.Orchestrations.Products
{
    public partial class ProductOrchestrationService
    {
        private delegate ValueTask<Product> ReturningProductFunction();
        private delegate ValueTask RollingbackProductFunction(Exception reasonException);

        private async ValueTask<Product> TryCatchAndRollback(
            ReturningProductFunction returningProductFunction,
            RollingbackProductFunction rollingbackProductFunction)
        {
            try
            {
                return await TryCatch(returningProductFunction);
            }
            catch (Exception exception)
            {
                await TryRollback(exception, rollingbackProductFunction);

                throw;
            }
        }

        private async ValueTask<Product> TryCatch(ReturningProductFunction returningProductFunction)
        {
            try
            {
                return await returningProductFunction();
            }
            catch (Exception ex)
            {
                throw new ProductOrchestrationServiceException(
                    "An error occurred while processing the product.", ex);
            }
        }

        private async ValueTask TryRollback(
            Exception reasonException, // or mainException 
            RollingbackProductFunction rollingbackProductFunction)
        {
            try
            {
                await rollingbackProductFunction(reasonException);
            }
            catch (Exception exception)
            {
                throw new ProductOrchestrationRollbackException(
                    message: "An error occurred while processing the rollback product.",
                    reasonException: reasonException,
                    innerException: exception);
            }
        }
    }
}