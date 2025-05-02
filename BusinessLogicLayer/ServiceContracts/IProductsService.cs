using System.Linq.Expressions;

using eCommerce.BusinessLogicLayer.DTO;

using eCommerce.DataAccessLayer.Entities;

namespace eCommerce.BusinessLogicLayer.ServiceContracts;

public interface IProductsService
{
    /// <summary>
    /// Retrieves all products from the products repository.
    /// </summary>
    /// <returns>Returns list of ProductResponse objects.</returns>
    Task<List<ProductResponse?>> GetProducts();

    /// <summary>
    /// Retrieves products from the products repository based on a condition.
    /// </summary>
    /// <param name="conditionExpression">The condition expression to filter products.</param>
    /// <returns>Returns list of ProductResponse objects.</returns>
    Task<List<ProductResponse?>> GetProductsByCondition(Expression<Func<Product, bool>> conditionExpression);

    /// <summary>
    /// Retrieves a single product from the products repository based on a condition.
    /// </summary>
    /// <param name="conditionExpression">The condition expression to filter products.</param>
    /// <returns>Returns a ProductResponse object or null if not found.</returns>
    Task<ProductResponse?> GetProductByCondition(Expression<Func<Product, bool>> conditionExpression);

    /// <summary>
    /// Adds a new product to the products repository.
    /// </summary>
    /// <param name="productAddRequest">productAddRequest object containing product details.</param>
    /// <returns>Returns a ProductResponse object or null if the product could not be added.</returns>
    Task<ProductResponse?> AddProduct(ProductAddRequest productAddRequest);

    /// <summary>
    /// Updates an existing product in the products repository.
    /// </summary>
    /// <param name="productUpdateRequest">The productUpdateRequest object containing updated product details.</param>
    /// <returns>Returns a ProductResponse object or null if the product could not be updated.</returns>
    Task<ProductResponse?> UpdateProduct(ProductUpdateRequest productUpdateRequest);

    /// <summary>
    /// Deletes a product from the products repository based on its ID.
    /// </summary>
    /// <param name="productId">The ID of the product to be deleted.</param>
    /// <returns>Returns true if the product was successfully deleted, false otherwise.</returns>
    Task<bool> DeleteProduct(Guid productId);
}