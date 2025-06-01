using System.Buffers;

namespace WebApp_Assessment.Services
{
    public enum StockOperationResult
    {
        Success,
        ProductNotFound,
        BadRequest
    }

    public class ServiceResult<T>
    {
        public StockOperationResult Status { get; set; }
        public T? Data { get; set; }

        public static ServiceResult<T> Success(T data) => new() { Status = StockOperationResult.Success, Data = data };
        public static ServiceResult<T> NotFound() => new() { Status = StockOperationResult.ProductNotFound };
        public static ServiceResult<T> BadRequest() => new() { Status = StockOperationResult.BadRequest };
    }
}
