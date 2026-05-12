using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.DTO
{
    public class CreateTagDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;

    }
}
