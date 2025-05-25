using eCommerce.BusinessLogicLayer.DTO;
using eCommerce.BusinessLogicLayer.ServiceContracts;

using FluentValidation;
using FluentValidation.Results;

namespace eCommerce.ProductsMicroService.API.APIEndpoints;

public static class ProductAPIEndpoints
{
    public static IEndpointRouteBuilder MapProductAPIEndpoints(this IEndpointRouteBuilder app)
    {
        //GET /api/products
        app.MapGet("/api/products", async (IProductsService productsService) =>
        {
            List<ProductResponse?> products = await productsService.GetProducts();

            return Results.Ok(products);
        });

        //GET /api/products/search/product-id/00000000-0000-0000-0000-000000000000
        app.MapGet("/api/products/search/product-id/{ProductID:guid}", async (
            IProductsService productsService,
            Guid ProductID) =>
        {
            //await Task.Delay(100);
            //throw new NotImplementedException("Test exception message in ProductsMicroService");

            ProductResponse? product = await productsService
                .GetProductByCondition(temp => temp.ProductID == ProductID);

            if (product == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(product);
        });

        //GET /api/products/search/product-name/abc
        app.MapGet("/api/products/search/{SearchString}", async (IProductsService productsService, string SearchString) =>
        {
            List<ProductResponse?> productsByProductName = await productsService.GetProductsByCondition(temp => temp.ProductName != null
            && temp.ProductName.Contains(
                SearchString));

            List<ProductResponse?> productsByCategory = await productsService.GetProductsByCondition(temp => temp.Category != null
            && temp.Category.Contains(
                SearchString));

            IEnumerable<ProductResponse?> products = productsByProductName.Union(productsByCategory);

            return Results.Ok(products);
        });

        //GET /api/products/
        app.MapPost("/api/products", async (
            IProductsService productsService,
            IValidator<ProductAddRequest> productAddRequestValidator,
            ProductAddRequest productAddRequest) =>
        {
            ValidationResult validationResult = productAddRequestValidator.Validate(productAddRequest);

            if (!validationResult.IsValid)
            {
                Dictionary<string, string[]> errors = validationResult.Errors.GroupBy(temp => temp.PropertyName)
                .ToDictionary(grp => grp.Key, grp => grp.Select(error => error.ErrorMessage)
                .ToArray());

                return Results.ValidationProblem(errors);
            }

            ProductResponse? addedProductResponse = await productsService.AddProduct(productAddRequest);

            if (addedProductResponse != null)
            {
                return Results.Created($"/api/products/search/product-id/{addedProductResponse.ProductID}", addedProductResponse);
            }
            else
            {
                return Results.Problem("Error in adding product");
            }
        });

        app.MapPut("/api/products", async (
            IProductsService productsService,
            IValidator<ProductUpdateRequest> productUpdateRequestValidator,
            ProductUpdateRequest productUpdateRequest) =>
        {
            ValidationResult validationResult = productUpdateRequestValidator.Validate(productUpdateRequest);

            if (!validationResult.IsValid)
            {
                Dictionary<string, string[]> errors = validationResult.Errors.GroupBy(temp => temp.PropertyName)
                .ToDictionary(grp => grp.Key, grp => grp.Select(error => error.ErrorMessage)
                .ToArray());
                return Results.ValidationProblem(errors);
            }

            ProductResponse? updatedProductResponse = await productsService.UpdateProduct(productUpdateRequest);

            if (updatedProductResponse != null)
            {
                return Results.Ok(updatedProductResponse);
            }
            else
            {
                return Results.Problem("Failed to update product");
            }
        });

        app.MapDelete("/api/products/{ProductID:guid}", async (
            IProductsService productsService,
            Guid ProductID) =>
        {
            bool result = await productsService.DeleteProduct(ProductID);

            if (!result)
            {
                return Results.Problem("Failed to delete the product");
            }
            else
            {
                return Results.Ok(true);
            }
        });

        return app;
    }
}