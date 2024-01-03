using DB_Assigment.DTOs;

namespace DB_Assigment.IRepositories
{
    public interface IProductRepository
    {
        Task<ProductDto> GetProductByCode(string code);
        Task<List<ProductDto>> GetAllProducts();
        Task<LogicResponse> AddProduct(ProductDto product);
        Task<LogicResponse> UpdateProduct(ProductDto product);
        Task<LogicResponse> DeleteProduct(string code);
    }
}
