using rsm_backend.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.Services.Interfaces
{
    public interface IAdminBrandService
    {
        public Task<int> BulkCreateAsync(List<CreateBrandDTO> brands);

    }
}
