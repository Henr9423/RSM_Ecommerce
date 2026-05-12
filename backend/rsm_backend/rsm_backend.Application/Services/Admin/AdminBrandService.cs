using rsm_backend.Application.DTO;
using rsm_backend.Application.Services.Admin;
using rsm_backend.Application.Services.Interfaces;
using rsm_backend.Application.Services.Interfaces.IRepositories;
using rsm_backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace rsm_backend.Application.Services.Admin
{
    public class AdminBrandService : IAdminBrandService
    {
        private readonly IBrandRepository _brandRepository;
        public AdminBrandService(IBrandRepository brandRepository) 
        { 
            _brandRepository = brandRepository;
        }

        public async Task<int> BulkCreateAsync(List<CreateBrandDTO> brands)
        {
           if (brands==null)
                throw new ArgumentNullException(nameof(brands));

            if (brands.Count == 0)
                return 0;
           
            if (brands.Any(b => b == null))
                throw new ArgumentException("Brand list contains null items.", nameof(brands));

            if (brands.Any(b => string.IsNullOrWhiteSpace(b.Name)))
                throw new ArgumentException("Brand name cannot be empty",nameof(brands));

            var now= DateTime.UtcNow;

            var entities = brands.Select(item => new Brand
            {
                Name = item.Name.Trim(),
                CreatedAt = now
            }).ToList();

            await _brandRepository.BulkCreateAsync(entities);
            

            return entities.Count;
        }
    }
}
