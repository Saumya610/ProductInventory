using WebApp_Assessment.Services;
using WebApp_Assessment.Source.DTOs;

namespace WebApp_Assessment.Source.Services
{
    public interface IProductService
    {
        Task<ServiceResult<ProductDto>> CreateAsync(ProductCreateDto dto);
        Task<ServiceResult<ProductDto>> GetByIdAsync(string id);
        Task<ServiceResult<List<ProductDto>>> GetAllAsync();
        Task<StockOperationResult> DeleteAsync(string id);
        Task<ServiceResult<ProductDto>> UpdateAsync(string id, ProductUpdateDto dto);
        Task<StockOperationResult> DecrementStockAsync(string id, int quantity);
        Task<StockOperationResult> AddToStockAsync(string id, int quantity);
    }
}
