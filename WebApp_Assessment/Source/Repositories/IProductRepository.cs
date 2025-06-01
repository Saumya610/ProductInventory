using WebApp_Assessment.Services;
using WebApp_Assessment.Source.Models;

namespace WebApp_Assessment.Source.Repositories
{
    public interface IProductRepository
    {
        Task<Product> AddAsync(Product product);
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(string id);
        Task<Product> UpdateAsync(Product product);
        Task<bool> DeleteAsync(string id);
    }
}
