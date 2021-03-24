using System;
using System.IO;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Abstractions.Services.Files;

namespace ImagesAndFilesStorage.Services
{
    public class SingleFileStorage : ISingleFileStorage
    {
        private readonly ICloudinaryService _cloudinaryService;

        public SingleFileStorage(ICloudinaryService cloudinaryService)
        {
            _cloudinaryService = cloudinaryService;
        }

        public async Task<string> StoreSingle(Stream stream)
        {
            var result = await _cloudinaryService.UploadFile(new RawUploadParams
            {
                File = new FileDescription(Guid.NewGuid().ToString(), stream)
            });

            return result.SecureUri.ToString();
        }
    }
}