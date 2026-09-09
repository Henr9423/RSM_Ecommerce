using Microsoft.EntityFrameworkCore.Metadata.Internal;
using rsm_backend.Application.Services.Interfaces.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsm_backend.Infrastructure.FakeServicesForTests
{
    public class FakeObjectStorage : IObjectStorage
    {
        private readonly Dictionary<string, byte[]> _files = new();

        public string GetPublicUrl(string? storageKey)
        {
            if (storageKey == null)
            {
                throw new ArgumentNullException("Storage key is null");
            }

            return $"https://fake-storage.test/{storageKey}";
        }

        public async Task UploadAsync(string objectKey, Stream stream, string contentType, CancellationToken cancellationToken)
        {
            using var memoryStream = new MemoryStream();

            await stream.CopyToAsync(memoryStream);

            _files[objectKey] = memoryStream.ToArray();
        }


        // These aren't part of IObjectStorage.
        // They're helpers specifically for integration tests.

        public bool Contains(string storageKey)
        {
            return _files.ContainsKey(storageKey);
        }

        public byte[]? GetFile(string storageKey)
        {
            return _files.GetValueOrDefault(storageKey);
        }

        public void Clear()
        {
            _files.Clear();
        }
    }
}
