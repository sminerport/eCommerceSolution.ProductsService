using System.Linq.Expressions;

using eCommerce.DataAccessLayer.Entities;

namespace eCommerce.DataAccessLayer.RepositoryContracts;

/// <summary>
/// Represents a repository for managing 'products' table in the database.
/// </summary>
public interface IProductsRepository
{
    /// <summary>
    /// Retrieves all products from the database.
    /// </summary>
    /// <returns>All products in the database.</returns>
    Task<IEnumerable<Product?>> GetProducts();

    /// <summary>
    /// Retrieves a collection of products that match the specified condition.
    /// </summary>
    /// <param name="condition">The condition to filter the product.</param>
    /// <returns>Collection of products that match the condition or null if not found.</returns>
    Task<IEnumerable<Product?>> GetProductsByCondition(Expression<Func<Product, bool>> condition);

    /// <summary>
    /// Retrieves a single product that matches the specified condition.
    /// </summary>
    /// <param name="condition">The condition to filter the product.</param>
    /// <returns>A single product that matches the condition or null if not found.</returns>
    Task<Product?> GetProductByCondition(Expression<Func<Product, bool>> condition);

    /// <summary>
    /// Adds a new product into the products table asynchronously
    /// </summary>
    /// <param name="product">The product to be added.</param>
    /// <returns>The added product or null if the operation fails.</returns>
    Task<Product?> AddProduct(Product product);

    /// <summary>
    /// Updates an existing product in the products table asynchronously.
    /// </summary>
    /// <param name="product">The product to be updated.</param>
    /// <returns>The updated product or null if the operation fails.</returns>
    Task<Product?> UpdateProduct(Product product);

    /// <summary>
    /// Deletes a product from the products table asynchronously.
    /// </summary>
    /// <param name="product">The ID of the product to be deleted.</param>
    /// <returns>A boolean indicating whether the deletion was successful.</returns>
    Task<bool> DeleteProduct(Guid ProductID);
}