using System;
using Xeptions;

namespace Standard.Rollback.PoC.Models.Foundations.Products.Exceptions
{
    public class ProductOrchestrationServiceException : Xeption
    {
        public ProductOrchestrationServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
