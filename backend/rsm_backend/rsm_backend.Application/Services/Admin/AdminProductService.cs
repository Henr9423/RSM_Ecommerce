using rsm_backend.Application.DTO;
using rsm_backend.Application.Services.Interfaces;
using rsm_backend.Application.Services.Interfaces.IRepositories;
using rsm_backend.Domain.Entities;


namespace rsm_backend.Application.Services.Admin
{
    public class AdminProductService : IAdminProductService
    {

        private readonly IProductRepository _productRepo;
        private readonly ITagRepository _tagRepo;
        public AdminProductService(IProductRepository productRepository, ITagRepository tagRepository)
        {
            _productRepo = productRepository;
            _tagRepo = tagRepository;
        }
        public async Task<int> BulkCreateAsync(List<CreateProductDTO> products)
        {

            if (products == null)
                throw new ArgumentNullException(nameof(products));

            if (products.Count == 0)
                return 0;


            var now = DateTime.UtcNow;

            List<Product> productsToAdd = products.Select(p =>
            {
                if (p == null)
                    throw new ArgumentException("Product list contains null items.", nameof(products));

                if (string.IsNullOrWhiteSpace(p.Name))
                    throw new ArgumentException("Product name cannot be empty.", nameof(products));

                if (p.BrandId is not int brandId)
                    throw new ArgumentException("Product brandId cannot be null.", nameof(products));

                if (brandId <= 0)
                    throw new ArgumentException("Product brandId cannot be less than or equal to 0.", nameof(products));

                return new Product()
                {
                    Name = p.Name,
                    BrandId = p.BrandId.Value,
                    Description = p.Description,
                    CreatedAt = now,
                    UpdatedAt = now,

                };
            }).ToList();

            await _productRepo.BulkCreateAsync(productsToAdd);



            return productsToAdd.Count();
        }

        public async Task<int> BulkCreateProductTagsAsync(List<CreateProductTagDTO> productTagDTOs)
        {
            if(productTagDTOs == null)
                throw new ArgumentNullException(nameof(productTagDTOs));

            if(productTagDTOs.Count == 0) 
                throw new ArgumentException("List of ProductTagDTO's is empty",nameof(productTagDTOs));

            var duplicateExists = productTagDTOs
                .GroupBy(pt => new { pt.ProductId, pt.TagId })
                .Any(g => g.Count() > 1);

            if (duplicateExists)
                throw new ArgumentException("Request contains duplicate product/tag combinations.", nameof(productTagDTOs));


            var now = DateTime.UtcNow;


            var productTagsToAdd = new List<ProductTag>();

            foreach (var pt in productTagDTOs)
            {
                var productExists = await _productRepo.ExistsAsync(pt.ProductId);

                if (!productExists)
                    throw new KeyNotFoundException($"Product with ID {pt.ProductId} was not found.");

                var tagExists = await _tagRepo.ExistsAsync(pt.TagId);

                if (!tagExists)
                    throw new KeyNotFoundException($"Tag with ID {pt.TagId} was not found.");

                var productTagExists = await _productRepo.ExistProductTagAsync(pt.ProductId, pt.TagId);

                if (productTagExists)
                    throw new InvalidOperationException($"Product {pt.ProductId} already has tag {pt.TagId}.");

                productTagsToAdd.Add(new ProductTag
                {
                    ProductID = pt.ProductId,
                    TagId = pt.TagId,
                    CreatedAt = now
                });
            }


            await _productRepo.BulkCreateProductTagsAsync(productTagsToAdd);

            return productTagsToAdd.Count;

        }
    }
}
