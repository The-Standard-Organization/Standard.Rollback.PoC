using System;
using Standard.Rollback.PoC.Models.Foundations.ProductSources;
using Standard.Rollback.PoC.Models.Foundations.ProductSources.Exceptions;

namespace Standard.Rollback.PoC.Services.Foundations.ProductSources
{
    public partial class ProductSourceService
    {
        private void ValidateProductSourceOnAdd(ProductSource productSource)
        {
            ValidateProductSourceIsNotNull(productSource);

            Validate(
                (Rule: IsInvalid(productSource.Id), Parameter: nameof(ProductSource.Id)));
        }

        private void ValidateProductSourceOnModify(ProductSource productSource)
        {
            ValidateProductSourceIsNotNull(productSource);

            Validate(
                (Rule: IsInvalid(productSource.Id), Parameter: nameof(ProductSource.Id)));
        }

        public void ValidateProductSourceId(Guid productSourceId) =>
            Validate((Rule: IsInvalid(productSourceId), Parameter: nameof(ProductSource.Id)));

        private static void ValidateStorageProductSource(ProductSource maybeProductSource, Guid productSourceId)
        {
            if (maybeProductSource is null)
            {
                throw new NotFoundProductSourceException(productSourceId);
            }
        }

        private static void ValidateProductSourceIsNotNull(ProductSource productSource)
        {
            if (productSource is null)
            {
                throw new NullProductSourceException();
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static dynamic IsSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate == secondDate,
                Message = $"Date is the same as {secondDateName}"
            };

        private static dynamic IsNotSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not the same as {secondDateName}"
            };

        private static dynamic IsNotSame(
            Guid firstId,
            Guid secondId,
            string secondIdName) => new
            {
                Condition = firstId != secondId,
                Message = $"Id is not the same as {secondIdName}"
            };

        private dynamic IsNotRecent(DateTimeOffset date) => new
        {
            Condition = IsDateNotRecent(date),
            Message = "Date is not recent"
        };

        private bool IsDateNotRecent(DateTimeOffset date)
        {
            DateTimeOffset currentDateTime =
                this.dateTimeBroker.GetCurrentDateTimeOffset();

            TimeSpan timeDifference = currentDateTime.Subtract(date);
            TimeSpan oneMinute = TimeSpan.FromMinutes(1);

            return timeDifference.Duration() > oneMinute;
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidProductSourceException = new InvalidProductSourceException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidProductSourceException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidProductSourceException.ThrowIfContainsErrors();
        }
    }
}