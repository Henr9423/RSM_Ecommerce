using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Application.Services.Interfaces.Infrastructure
{
    public interface IObjectStorage
    {
        public Task UploadAsync(string objectKey, Stream stream, string contentType, CancellationToken cancellationToken);

        public string GetPublicUrl(string? storageKey);
    }
}
