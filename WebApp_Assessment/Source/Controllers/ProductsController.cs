using System.Buffers;
using Microsoft.AspNetCore.Mvc;
using WebApp_Assessment.Services;
using WebApp_Assessment.Source.DTOs;
using WebApp_Assessment.Source.Services;

namespace WebApp_Assessment.Source.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        // POST: /api/products
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);

            return result.Status switch
            {
                StockOperationResult.Success => CreatedAtAction(nameof(GetById), new { id = result.Data!.ProductId }, result.Data),
                StockOperationResult.BadRequest => BadRequest("Product creation failed."),
                _ => StatusCode(500, "Unexpected error.")
            };
        }

        // GET: /api/products
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();

            return result.Status switch
            {
                StockOperationResult.Success => Ok(result.Data),
                StockOperationResult.ProductNotFound => NotFound("No products available.")
            };
        }

        // GET: /api/products/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _service.GetByIdAsync(id);

            return result.Status switch
            {
                StockOperationResult.Success => Ok(result.Data),
                StockOperationResult.ProductNotFound => NotFound($"Product with ID '{id}' not found.")
            };
        }

        // PUT: /api/products/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, ProductUpdateDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);

            return result.Status switch
            {
                StockOperationResult.Success => Ok(result.Data),
                StockOperationResult.ProductNotFound => NotFound($"Product with ID '{id}' not found.")
            };
        }

        // DELETE: /api/products/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _service.DeleteAsync(id);
            return result switch
            {
                StockOperationResult.Success => Ok("Product deleted successfully."),
                StockOperationResult.ProductNotFound => NotFound($"Product with ID '{id}' not found.")
            };
        }

        // PUT: /api/products/decrement-stock/{id}/{quantity}
        [HttpPut("decrement-stock/{id}/{quantity}")]
        public async Task<IActionResult> DecrementStock(string id, int quantity)
        {
            var result = await _service.DecrementStockAsync(id, quantity);
            return result switch
            {
                StockOperationResult.Success => Ok("Stock decremented successfully."),
                StockOperationResult.ProductNotFound => NotFound($"Product with ID '{id}' not found."),
                StockOperationResult.BadRequest => BadRequest("Insufficient stock available.")
            };
        }

        // PUT: /api/products/add-to-stock/{id}/{quantity}
        [HttpPut("add-to-stock/{id}/{quantity}")]
        public async Task<IActionResult> AddStock(string id, int quantity)
        {
            var result = await _service.AddToStockAsync(id, quantity);
            return result switch
            {
                StockOperationResult.Success => Ok("Stock incremented successfully."),
                StockOperationResult.ProductNotFound => NotFound($"Product with ID '{id}' not found.")
            };
        }
    }
}
