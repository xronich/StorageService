using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Abstractions.Services.Images;

namespace ImagesAndFilesStorage.Services
{
    public class SingleImageStorage : ISingleImageStorage
    {
        private readonly ICloudinaryService _cloudinaryService;

        public SingleImageStorage(ICloudinaryService cloudinaryService)
        {
            _cloudinaryService = cloudinaryService;
        }

        public async Task<string> StoreSingle(string base64)
        {
            var result = await _cloudinaryService.UploadImage(new ImageUploadParams
            {
                File = new FileDescription(base64)
            });

            return result.SecureUri.ToString();
        }
    }
}