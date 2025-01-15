using System;
using Standard.Rollback.PoC.Models.Foundations.ProductImages;
using Standard.Rollback.PoC.Models.Foundations.ProductImages.Exceptions;

namespace Standard.Rollback.PoC.Services.Foundations.ProductImages
{
    public partial class ProductImageService
    {
        private void ValidateProductImageOnAdd(ProductImage productImage)
        {
            ValidateProductImageIsNotNull(productImage);

            Validate(
                (Rule: IsInvalid(productImage.Id), Parameter: nameof(ProductImage.Id)));
        }

        private void ValidateProductImageOnModify(ProductImage productImage)
        {
            ValidateProductImageIsNotNull(productImage);

            Validate(
                (Rule: IsInvalid(productImage.Id), Parameter: nameof(ProductImage.Id)));
        }

        public void ValidateProductImageId(Guid productImageId) =>
            Validate((Rule: IsInvalid(productImageId), Parameter: nameof(ProductImage.Id)));

        private static void ValidateStorageProductImage(ProductImage maybeProductImage, Guid productImageId)
        {
            if (maybeProductImage is null)
            {
                throw new NotFoundProductImageException(productImageId);
            }
        }

        private static void ValidateProductImageIsNotNull(ProductImage productImage)
        {
            if (productImage is null)
            {
                throw new NullProductImageException();
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
            var invalidProductImageException = new InvalidProductImageException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidProductImageException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidProductImageException.ThrowIfContainsErrors();
        }
    }
}