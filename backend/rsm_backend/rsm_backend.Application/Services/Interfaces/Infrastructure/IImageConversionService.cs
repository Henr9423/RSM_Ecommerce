using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.Services.Interfaces.Infrastructure
{
    public interface IImageConversionService
    {
        Task<Stream> ConvertToWebPAsync(Stream input, CancellationToken cancellationToken = default);

    }
}
