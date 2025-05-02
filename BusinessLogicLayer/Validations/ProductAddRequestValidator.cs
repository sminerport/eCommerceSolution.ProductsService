using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using eCommerce.BusinessLogicLayer.DTO;

using FluentValidation;

namespace eCommerce.BusinessLogicLayer.Validations;

public class ProductAddRequestValidator : AbstractValidator<ProductAddRequest>
{
    public ProductAddRequestValidator()
    {
        RuleFor(productAddRequest => productAddRequest.ProductName)
            .NotEmpty()
            .WithMessage("Product name is required.")
            .Length(2, 50)
            .WithMessage("Product name must be between 2 and 50 characters.");

        RuleFor(productAddRequest => productAddRequest.Category)
            .IsInEnum()
            .WithMessage("Invalid category.");

        RuleFor(productAddRequest => productAddRequest.UnitPrice)
            .InclusiveBetween(0, double.MaxValue)
            .WithMessage($"Unit price must be between 0 to {double.MaxValue}.");

        RuleFor(productAddRequest => productAddRequest.QuantityInStock)
            .InclusiveBetween(0, int.MaxValue)
            .WithMessage($"Quantity in stock must be between 0 to {int.MaxValue}.");
    }
}