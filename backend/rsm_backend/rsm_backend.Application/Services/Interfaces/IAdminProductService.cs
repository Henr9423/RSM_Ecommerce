using rsm_backend.Application;
using rsm_backend.Application.DTO;

namespace rsm_backend.Application.Services.Interfaces
{
    public interface IAdminProductService
    {

        public Task<int> BulkCreateAsync(List<CreateProductDTO> products);

        public Task<int> BulkCreateProductTagsAsync(List<CreateProductTagDTO> productTagDTOs);
    }
}
