using System.Linq.Expressions;

using AutoMapper;

using eCommerce.BusinessLogicLayer.DTO;
using eCommerce.BusinessLogicLayer.RabbitMQ;
using eCommerce.BusinessLogicLayer.ServiceContracts;
using eCommerce.DataAccessLayer.Entities;
using eCommerce.DataAccessLayer.RepositoryContracts;

using FluentValidation;
using FluentValidation.Results;

namespace eCommerce.BusinessLogicLayer.Services;

public class ProductsService : IProductsService
{
    private readonly IProductsRepository _productsRepository;
    private readonly IValidator<ProductAddRequest> _productAddRequestValidator;
    private readonly IValidator<ProductUpdateRequest> _productUpdateRequestValidator;
    private readonly IMapper _mapper;
    private readonly IRabbitMQPublisher _rabbitMQPublisher;

    public ProductsService(
        IProductsRepository productsRepository,
        IMapper mapper,
        IValidator<ProductAddRequest> productAddRequestValidator,
        IValidator<ProductUpdateRequest> productUpdateRequestValidator,
        IRabbitMQPublisher rabbitMQPublisher)
    {
        _productsRepository = productsRepository;
        _mapper = mapper;
        _productAddRequestValidator = productAddRequestValidator;
        _productUpdateRequestValidator = productUpdateRequestValidator;
        _rabbitMQPublisher = rabbitMQPublisher;
    }

    public async Task<ProductResponse?> AddProduct(ProductAddRequest productAddRequest)
    {
        if (productAddRequest == null)
        {
            throw new ArgumentNullException(nameof(productAddRequest));
        }

        ValidationResult validationResult = await _productAddRequestValidator.ValidateAsync(productAddRequest);

        if (!validationResult.IsValid)
        {
            string? errors = string.Join(", ", validationResult.Errors.Select(temp => temp.ErrorMessage));
            throw new ArgumentException($"Validation failed: {errors}");
        }

        Product productInput = _mapper.Map<Product>(productAddRequest);

        Product? addedProduct = await _productsRepository.AddProduct(productInput);

        if (addedProduct == null)
        {
            return null;
        }

        ProductResponse addedProductResponse = _mapper.Map<ProductResponse>(addedProduct);

        return addedProductResponse;
    }

    public async Task<ProductResponse?> UpdateProduct(ProductUpdateRequest productUpdateRequest)
    {
        Product? existingProduct = await _productsRepository.GetProductByCondition(temp => temp.ProductID == productUpdateRequest.ProductID);

        if (existingProduct == null)
        {
            throw new ArgumentException($"Product with ID {productUpdateRequest.ProductID} not found.");
        }

        ValidationResult validationResult = await _productUpdateRequestValidator.ValidateAsync(productUpdateRequest);

        if (!validationResult.IsValid)
        {
            string? errors = string.Join(", ", validationResult.Errors.Select(temp => temp.ErrorMessage));
            throw new ArgumentException($"Validation failed: {errors}");
        }

        Product productToUpdate = _mapper.Map<Product>(productUpdateRequest);

        Product? updatedProduct = await _productsRepository.UpdateProduct(productToUpdate);

        Dictionary<string, object?> headers = new Dictionary<string, object?>()
        {
            { "event", "product.update" },
            { "row-count", 1 }
        };

        await _rabbitMQPublisher.Publish<Product>(headers, productToUpdate);

        if (updatedProduct == null)
        {
            return null;
        }

        ProductResponse productUpdatedResponse = _mapper.Map<ProductResponse>(updatedProduct);

        return productUpdatedResponse;
    }

    public async Task<ProductResponse?> GetProductByCondition(Expression<Func<Product, bool>> conditionExpression)
    {
        if (conditionExpression == null)
        {
            throw new ArgumentNullException(nameof(conditionExpression));
        }

        Product? product = await _productsRepository.GetProductByCondition(conditionExpression);

        if (product == null)
        {
            return null;
        }

        ProductResponse productResponse = _mapper.Map<ProductResponse>(product);

        return productResponse;
    }

    public async Task<List<ProductResponse?>> GetProducts()
    {
        IEnumerable<Product?> products = await _productsRepository.GetProducts();

        if (products == null)
        {
            return new List<ProductResponse?>();
        }

        IEnumerable<ProductResponse?> productResponses = _mapper.Map<IEnumerable<ProductResponse>>(products);

        return productResponses.ToList();
    }

    public async Task<List<ProductResponse?>> GetProductsByCondition(Expression<Func<Product, bool>> conditionExpression)
    {
        if (conditionExpression == null)
        {
            throw new ArgumentNullException(nameof(conditionExpression));
        }

        IEnumerable<Product?> products = await _productsRepository.GetProductsByCondition(conditionExpression);

        if (products == null)
        {
            return new List<ProductResponse?>();
        }

        List<ProductResponse?> productResponses = _mapper.Map<List<ProductResponse?>>(products);

        return productResponses;
    }

    public async Task<bool> DeleteProduct(Guid productID)
    {
        Product? existingProduct = await _productsRepository.GetProductByCondition(temp => temp.ProductID == productID);

        if (existingProduct == null)
            return false;

        bool isDeleted = await _productsRepository.DeleteProduct(productID);

        // TODO: Add code for posting a message to the message queue that accounces to the consumers about the deleted product details

        if (isDeleted)
        {
            ProductDeleteMessage message = new ProductDeleteMessage(existingProduct.ProductID, existingProduct.ProductName);
            Dictionary<string, object?> headers = new Dictionary<string, object?>()
            {
                { "event", "product.delete" },
                { "row-count", 1 }
            };
            await _rabbitMQPublisher.Publish<ProductDeleteMessage>(headers, message);
        }

        return isDeleted;
    }
}