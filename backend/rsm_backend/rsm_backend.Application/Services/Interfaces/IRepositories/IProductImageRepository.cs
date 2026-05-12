using rsm_backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.Services.Interfaces.IRepositories
{
    public interface IProductImageRepository
    {


        Task<ProductImage?> GetByIdAsync(int id);

        Task<ProductImage?> GetByAltTextAsync(string altText);

        Task<List<ProductImage>> GetAllProductImages();

        Task AddAsync(ProductImage productImage);

        Task BulkCreateAsync(List<ProductImage> productImages);

        Task<bool> ExistsAsync(int productVariantId, string storageKey);

        Task DeleteAsync(int id);

        Task<int> GetNextSortOrderAsync(int productVariantId);
       

  
    }
}
