using System;
using Xeptions;

namespace Standard.Rollback.PoC.Models.Foundations.Products.Exceptions
{
    public class ProductOrchestrationRollbackException : Xeption
    {
        public ProductOrchestrationRollbackException(
            string message,
            Exception reasonException,
            Exception innerException)
            : base(message, innerException)
        {
            RollbackReasonException = reasonException;
        }

        public Exception RollbackReasonException { get; private set; }
    }
}
