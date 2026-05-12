using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using rsm_backend.Application.Services;
using rsm_backend.Application.Services.Interfaces.IRepositories;
using rsm_backend.Domain.Entities;
using rsm_backend.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Infrastructure.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<TagRepository> _logger;

        public TagRepository(AppDbContext dbContext, ILogger<TagRepository> logger)
        {
            _context = dbContext;
            _logger = logger;
        }


        public Task AddAsync(Tag tag)
        {
            throw new NotImplementedException();
        }

        public async Task BulkCreateAsync(List<Tag> tags)
        {
            if (tags == null)
                throw new ArgumentNullException(nameof(tags));

            if (tags.Count == 0)
                throw new ArgumentException("Tag list cannot be empty.", nameof(tags));


            try
            {
               await _context.AddRangeAsync(tags);
               await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex )
            {
                _logger.LogError(ex, "Error adding bulk of Tags. Count: {Count}", tags.Count);

                throw;
            }

        }

        public async Task<bool> ExistsAsync(int id)
        {


            return await _context.Tags.AnyAsync(t => t.Id == id);
        }

        public Task<List<Tag>> GetAllTags()
        {
            throw new NotImplementedException();
        }

        public Task<Tag?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Tag?> GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }
    }
}
