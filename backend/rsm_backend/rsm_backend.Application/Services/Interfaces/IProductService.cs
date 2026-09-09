using rsm_backend.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.Services.Interfaces
{
    public interface IProductService
    {

        public Task<List<ProductCardDTO>> GetProductsAsync(string? search);

        public Task AddProductImageToProductVariantAsync(int productVariantId, string imageKey, AddProductImageRequest dto, CancellationToken cancellationToken);

        public Task<object> AddProductImageAsync(int productId,
                                                        int productVariantId,
                                                        Stream input,
                                                        AddProductImageRequest dto,
                                                        CancellationToken cancellationToken);


    }
}
