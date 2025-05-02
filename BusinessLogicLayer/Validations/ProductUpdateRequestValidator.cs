using System;

using eCommerce.BusinessLogicLayer.DTO;

using FluentValidation;

namespace eCommerce.BusinessLogicLayer.Validations;

public class ProductUpdateRequestValidator : AbstractValidator<ProductUpdateRequest>
{
    public ProductUpdateRequestValidator()
    {
        RuleFor(productUpdateRequest => productUpdateRequest.ProductID)
            .NotEmpty()
            .WithMessage("Product ID is required.");
        RuleFor(productUpdateRequest => productUpdateRequest.ProductName)
            .NotEmpty()
            .WithMessage("Product name is required.")
            .Length(2, 50)
            .WithMessage("Product name must be between 2 and 50 characters.");
        RuleFor(productUpdateRequest => productUpdateRequest.Category)
            .IsInEnum()
            .WithMessage("Invalid category.");
        RuleFor(productUpdateRequest => productUpdateRequest.UnitPrice)
            .InclusiveBetween(0, double.MaxValue)
            .WithMessage($"Unit price must be between 0 to {double.MaxValue}.");
        RuleFor(productUpdateRequest => productUpdateRequest.QuantityInStock)
            .InclusiveBetween(0, int.MaxValue)
            .WithMessage($"Quantity in stock must be between 0 to {int.MaxValue}.");
    }
}