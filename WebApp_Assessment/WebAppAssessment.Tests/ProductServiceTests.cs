using NUnit.Framework;
using Moq;
using AutoMapper;
using WebApp_Assessment.Source.Services;
using WebApp_Assessment.Source.Repositories;
using WebApp_Assessment.Source.Models;
using WebApp_Assessment.Source;
using WebApp_Assessment.Source.DTOs;
using WebApp_Assessment.Services;

namespace WebAppAssessment.Tests;

[TestFixture]
public class ProductServiceTests
{
    private Mock<IProductRepository> _repoMock;
    private IProductService _service;
    private IMapper _mapper;

    [SetUp]
    public void Setup()
    {
        _repoMock = new Mock<IProductRepository>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new ProductMappingProfile());
        });

        _mapper = config.CreateMapper();
        _service = new ProductService(_repoMock.Object, _mapper);
    }

    [Test]
    public async Task CreateAsync_ShouldReturnProductDto_WhenProductIsCreated()
    {
        var createDto = new ProductCreateDto
        {
            Name = "Test Product",
            Description = "Description",
            StockAvailable = 10
        };

        var createdProduct = new Product
        {
            ProductId = "P00001",
            Name = "Test Product",
            Description = "Description",
            StockAvailable = 10
        };

        _repoMock.Setup(r => r.AddAsync(It.IsAny<Product>()))
                 .ReturnsAsync(createdProduct);

        var result = await _service.CreateAsync(createDto);

        Assert.IsNotNull(result);
        Assert.AreEqual("P00001", result.Data.ProductId); 
        Assert.AreEqual("Test Product", result.Data.Name);
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnProductDto_WhenProductExists()
    {
        var productId = "P00001";
        var product = new Product
        {
            ProductId = productId,
            Name = "Test Product",
            Description = "Description",
            StockAvailable = 10
        };

        _repoMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(product);

        var result = await _service.GetByIdAsync(productId);

        Assert.IsNotNull(result);
        Assert.AreEqual(productId, result.Data.ProductId); // Fixed: Accessing ProductId through the Data property  
        Assert.AreEqual("Test Product", result.Data.Name); // Fixed: Accessing Name through the Data property  
    }

    [Test]
    public async Task GetAllAsync_ShouldReturnAllProducts()
    {
        var products = new List<Product>
    {
        new Product { ProductId = "P00001", Name = "Product 1", Description = "Desc 1", StockAvailable = 5 },
        new Product { ProductId = "P00002", Name = "Product 2", Description = "Desc 2", StockAvailable = 8 }
    };

        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(products);

        var result = await _service.GetAllAsync();

        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Data.Count());
    }

    [Test]
    public async Task DeleteAsync_ShouldReturnTrue_WhenProductIsDeleted()
    {
        var productId = "P00001";
        _repoMock.Setup(r => r.DeleteAsync(productId)).ReturnsAsync(true);

        var result = await _service.DeleteAsync(productId);

        Assert.AreEqual(StockOperationResult.Success, result);
    }

    [Test]
    public async Task UpdateAsync_ShouldReturnUpdatedProductDto_WhenUpdateIsSuccessful()
    {
        var productId = "P00001";
        var updateDto = new ProductUpdateDto
        {
            Name = "Updated Product",
            Description = "Updated Description",
            StockAvailable = 15
        };

        var updatedProduct = new Product
        {
            ProductId = productId,
            Name = "Updated Product",
            Description = "Updated Description",
            StockAvailable = 15
        };

        _repoMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(updatedProduct);
        _repoMock.Setup(r => r.UpdateAsync(It.IsAny<Product>())).ReturnsAsync(updatedProduct);

        var result = await _service.UpdateAsync(productId, updateDto);

        Assert.IsNotNull(result);
        Assert.AreEqual("Updated Product", result.Data.Name);
        Assert.AreEqual(15, result.Data.StockAvailable);
    }

    [Test]
    public async Task DecrementStockAsync_ShouldReturnUpdatedProduct_WhenStockIsSufficient()
    {
        var productId = "P00001";
        var existingProduct = new Product
        {
            ProductId = productId,
            Name = "Test Product",
            Description = "Description",
            StockAvailable = 10
        };

        _repoMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(existingProduct);
        _repoMock.Setup(r => r.UpdateAsync(It.IsAny<Product>()))
                  .ReturnsAsync((Product p) => p);

        var result = await _service.DecrementStockAsync(productId, 3);

        Assert.AreEqual(StockOperationResult.Success, result); 
        Assert.AreEqual(productId, existingProduct.ProductId);
        Assert.AreEqual("Test Product", existingProduct.Name);
        Assert.AreEqual(7, existingProduct.StockAvailable);
    }

    [Test]
    public async Task AddToStockAsync_ShouldReturnTrue_AndIncreaseStock_WhenProductExists()
    {
        var productId = "P00001";
        var existingProduct = new Product
        {
            ProductId = productId,
            Name = "Test Product",
            Description = "Description",
            StockAvailable = 5
        };

        _repoMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(existingProduct);
        _repoMock.Setup(r => r.UpdateAsync(It.IsAny<Product>()))
                 .ReturnsAsync((Product p) => p);

        var result = await _service.AddToStockAsync(productId, 3);

        Assert.AreEqual(StockOperationResult.Success, result); 
        Assert.AreEqual(8, existingProduct.StockAvailable);
    }

    [Test]
    public async Task AddToStockAsync_ShouldReturnFalse_WhenProductDoesNotExist()
    {
        var productId = "P00002";

        _repoMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync((Product)null);

        var result = await _service.AddToStockAsync(productId, 5);

        Assert.AreEqual(StockOperationResult.ProductNotFound, result); 
    }

}