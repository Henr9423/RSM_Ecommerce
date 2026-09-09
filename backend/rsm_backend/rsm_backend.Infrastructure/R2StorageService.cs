using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using rsm_backend.Application.Services.Interfaces.Infrastructure;

namespace rsm_backend.Infrastructure
{
    public class R2StorageService:IObjectStorage
    {
        private readonly AmazonS3Client _client;
        private readonly R2Settings _settings;

        public R2StorageService(IOptions<R2Settings> options)
        {
            _settings = options.Value;

            var config = new AmazonS3Config
            {
                ServiceURL = $"https://{_settings.AccountId}.r2.cloudflarestorage.com",
                ForcePathStyle = true ,
            };

            _client = new AmazonS3Client(
                _settings.AccessKeyId,
                _settings.SecretAccessKey,
                config);

        }

        public string GetPublicUrl(string? storageKey)
        {
            if (storageKey == null) 
                throw new ArgumentNullException("Storage key is null");

            return $"{_settings.PublicUrl.TrimEnd('/')}/{storageKey.TrimStart('/')}";
        }

        public async Task UploadAsync(string objectKey, Stream stream, string contentType, CancellationToken cancellationToken)
        {
            var request = new PutObjectRequest
            {
                BucketName = _settings.BucketName,
                Key = objectKey,
                InputStream = stream,
                ContentType = contentType,

                DisablePayloadSigning = true,
                DisableDefaultChecksumValidation = true


            };

            await _client.PutObjectAsync(request, cancellationToken);
        }

    }
}
