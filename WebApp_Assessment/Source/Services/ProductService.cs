using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApp_Assessment.Services;
using WebApp_Assessment.Source.DTOs;
using WebApp_Assessment.Source.Models;
using WebApp_Assessment.Source.Repositories;

namespace WebApp_Assessment.Source.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ServiceResult<ProductDto>> CreateAsync(ProductCreateDto dto)
        {
            var product = _mapper.Map<Product>(dto);
            var createdProduct = await _repo.AddAsync(product);

            if (createdProduct == null)
                return ServiceResult<ProductDto>.BadRequest();

            return ServiceResult<ProductDto>.Success(_mapper.Map<ProductDto>(createdProduct));
        }

        public async Task<ServiceResult<ProductDto>> GetByIdAsync(string id)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null)
                return ServiceResult<ProductDto>.NotFound();

            var dto = _mapper.Map<ProductDto>(product);
            return ServiceResult<ProductDto>.Success(dto);
        }

        public async Task<ServiceResult<List<ProductDto>>> GetAllAsync()
        {
            var products = await _repo.GetAllAsync();
            if (products == null || products.Count == 0)
                return ServiceResult<List<ProductDto>>.NotFound(); 

            var productDtos = _mapper.Map<List<ProductDto>>(products);
            return ServiceResult<List<ProductDto>>.Success(productDtos);
        }

        public async Task<StockOperationResult> DeleteAsync(string id)
        {
            var deleted = await _repo.DeleteAsync(id);
            return deleted ? StockOperationResult.Success : StockOperationResult.ProductNotFound;
        }

        public async Task<ServiceResult<ProductDto>> UpdateAsync(string id, ProductUpdateDto dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null)
                return ServiceResult<ProductDto>.NotFound();

            _mapper.Map(dto, existing);
            await _repo.UpdateAsync(existing);

            var updatedDto = _mapper.Map<ProductDto>(existing);
            return ServiceResult<ProductDto>.Success(updatedDto);
        }

        public async Task<StockOperationResult> DecrementStockAsync(string id, int quantity)
        { 
            var product = await _repo.GetByIdAsync(id);
            if (product == null)
                return StockOperationResult.ProductNotFound;

            if (product.StockAvailable < quantity)
                return StockOperationResult.BadRequest;

            product.StockAvailable -= quantity;
            await _repo.UpdateAsync(product);

            return StockOperationResult.Success;
        }

        public async Task<StockOperationResult> AddToStockAsync(string id, int quantity)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null)
                return StockOperationResult.ProductNotFound;

            product.StockAvailable += quantity;
            await _repo.UpdateAsync(product);
            return StockOperationResult.Success;
        }
    }
}
