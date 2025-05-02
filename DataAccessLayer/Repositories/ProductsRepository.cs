using System.Linq.Expressions;

using eCommerce.DataAccessLayer.Context;
using eCommerce.DataAccessLayer.Entities;
using eCommerce.DataAccessLayer.RepositoryContracts;

using Microsoft.EntityFrameworkCore;

namespace eCommerce.DataAccessLayer.Repositories;

public class ProductsRepository : IProductsRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ProductsRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Product?> AddProduct(Product product)
    {
        await _dbContext.Products.AddAsync(product);
        await _dbContext.SaveChangesAsync();

        return product;
    }

    public async Task<bool> DeleteProduct(Guid productID)
    {
        Product? existingProduct = await _dbContext.Products.FirstOrDefaultAsync(p => p.ProductID == productID);

        if (existingProduct == null)
        {
            return false;
        }

        _dbContext.Products.Remove(existingProduct);
        int affectedRowsCount = await _dbContext.SaveChangesAsync();
        return affectedRowsCount > 0;
    }

    public async Task<Product?> GetProductByCondition(Expression<Func<Product, bool>> conditionExpression)
    {
        return await _dbContext.Products.FirstOrDefaultAsync(conditionExpression);
    }

    public async Task<IEnumerable<Product?>> GetProducts()
    {
        return await _dbContext.Products.ToListAsync();
    }

    public async Task<IEnumerable<Product?>> GetProductsByCondition(Expression<Func<Product, bool>> conditionExpression)
    {
        return await _dbContext.Products.Where(conditionExpression).ToListAsync();
    }

    public async Task<Product?> UpdateProduct(Product newProduct)
    {
        Product? existingProduct = await _dbContext.Products.FirstOrDefaultAsync(temp => temp.ProductID == newProduct.ProductID);

        if (existingProduct == null)
        {
            return null;
        }

        existingProduct.ProductName = newProduct.ProductName;
        existingProduct.Category = newProduct.Category;
        existingProduct.UnitPrice = newProduct.UnitPrice;
        existingProduct.QuantityInStock = newProduct.QuantityInStock;

        await _dbContext.SaveChangesAsync();
        return existingProduct;
    }
}