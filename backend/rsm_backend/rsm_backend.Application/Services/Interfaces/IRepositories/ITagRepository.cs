using rsm_backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.Services.Interfaces.IRepositories
{
    public interface ITagRepository
    {
        Task<Tag?> GetByIdAsync(int id);

        Task<Tag?> GetByNameAsync(string name);

        Task<List<Tag>> GetAllTags();

        Task AddAsync(Tag tag);
        Task BulkCreateAsync (List<Tag> tags);

        Task<bool> ExistsAsync(int id);
    }
}
