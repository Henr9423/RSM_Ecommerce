using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageMagick;
using rsm_backend.Application.Services.Interfaces.Infrastructure;

namespace rsm_backend.Infrastructure
{
    public class ImageConversionService : IImageConversionService
    {
        public async Task<Stream> ConvertToWebPAsync(Stream input, CancellationToken cancellationToken = default)
        {
            using var image = new MagickImage(input);

            image.AutoOrient();

            if(image.Width>2000 || image.Height>2000)
            {
                image.Resize(new MagickGeometry(2000, 2000)
                {
                    IgnoreAspectRatio = false
                });

            }

            image.Format = MagickFormat.WebP;
            image.Quality = 82;

            var output = new MemoryStream();

            await image.WriteAsync(output, cancellationToken);
            output.Position = 0;

            return output;


        }
    }
}
