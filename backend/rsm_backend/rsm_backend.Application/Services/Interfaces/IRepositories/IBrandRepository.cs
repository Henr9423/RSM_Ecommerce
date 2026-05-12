using rsm_backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.Services.Interfaces.IRepositories
{
    public interface IBrandRepository
    {
        Task<Brand?> GetByIdAsync(int id);

        Task<Brand?> GetByNameAsync(string name);

        Task AddAsync(Brand brand);

        Task BulkCreateAsync(List<Brand> brands);
    }
}
