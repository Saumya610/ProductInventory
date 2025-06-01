using System.Data;
using Microsoft.EntityFrameworkCore;
using WebApp_Assessment.Source.Data;
using WebApp_Assessment.Source.Models;

namespace WebApp_Assessment.Source.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _context;

        public ProductRepository(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<Product> AddAsync(Product product)
        {
            var connection = _context.Database.GetDbConnection();

            try
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = "SELECT NEXT VALUE FOR ProductSequence";
                var result = await command.ExecuteScalarAsync();

                product.ProductId = ((long)result).ToString("D6");

                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return product;
            }

            catch (Exception ex)
            {
                throw new Exception("Failed to generate ProductId or save product", ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    await connection.CloseAsync();
                }
            }
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetByIdAsync(string id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
