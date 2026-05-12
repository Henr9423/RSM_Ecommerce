using rsm_backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.Services.Interfaces.IRepositories
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(int id);

        Task<Product?> GetByNameAsync(string name);

        Task<List<Product>> GetAllProducts();

        Task AddAsync(Product product);

        Task BulkCreateAsync(List<Product> products);

        Task BulkCreateProductTagsAsync(List<ProductTag> productTags);


        Task<bool> ExistsAsync(int id);

        Task<bool> ExistProductTagAsync(int productId,int tagId);
    }
}
