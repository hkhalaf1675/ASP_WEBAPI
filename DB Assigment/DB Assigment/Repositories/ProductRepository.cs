using DB_Assigment.Contexts;
using DB_Assigment.DTOs;
using DB_Assigment.IRepositories;
using DB_Assigment.Models;

namespace DB_Assigment.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext context;

        public ProductRepository(ApplicationDbContext _context)
        {
            context = _context;
        }
        public async Task<LogicResponse> AddProduct(ProductDto product)
        {
            throw new NotImplementedException();
        }

        public async Task<LogicResponse> DeleteProduct(string code)
        {
            var product = context.Products.FirstOrDefault(P => P.Code == code);
            if (product == null)
            {
                return new LogicResponse
                {
                    Message = "There is no product with that code",
                    IsJobDone = false
                };
            }

            context.Products.Remove(product);
            try
            {
                context.SaveChanges();
                return new LogicResponse
                {
                    IsJobDone = true
                };
            }
            catch(Exception ex)
            {
                return new LogicResponse
                {
                    Message = ex.Message,
                    IsJobDone = false
                };
            }
        }

        public Task<List<ProductDto>> GetAllProducts()
        {
            throw new NotImplementedException();
        }

        public Task<ProductDto> GetProductByCode(string code)
        {
            throw new NotImplementedException();
        }

        public Task<LogicResponse> UpdateProduct(ProductDto product)
        {
            throw new NotImplementedException();
        }
    }
}
