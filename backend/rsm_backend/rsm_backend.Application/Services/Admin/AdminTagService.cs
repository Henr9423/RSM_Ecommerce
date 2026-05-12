using rsm_backend.Application.DTO;
using rsm_backend.Application.Services.Interfaces;
using rsm_backend.Application.Services.Interfaces.IRepositories;
using rsm_backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.Services.Admin
{
    public class AdminTagService : IAdminTagService
    {
        private readonly ITagRepository _tagRepo;
        public AdminTagService(ITagRepository tagRepository)
        {
            _tagRepo = tagRepository;
        }
        public async Task<int> BulkCreateTagsAsync(List<CreateTagDTO> tagDTOs)
        {

            if (tagDTOs == null)
                throw new ArgumentNullException(nameof(tagDTOs));

            if (tagDTOs.Count == 0)
                throw new ArgumentException("Tag DTO list can't be empty",nameof(tagDTOs));


            var now = DateTime.UtcNow;

            List<Tag> tagsToAdd = tagDTOs.Select(t =>
            {
                if (t == null)
                    throw new ArgumentException("Tag list contains null items.", nameof(tagDTOs));

                if (string.IsNullOrWhiteSpace(t.Name))
                    throw new ArgumentException("Tag name cannot be empty.", nameof(tagDTOs));

                return new Tag()
                {
                    Name = t.Name,
                    CreatedAt = now,
                   
                };
            }).ToList();

            await _tagRepo.BulkCreateAsync(tagsToAdd);



            return tagsToAdd.Count();

            
        }
    }
}
