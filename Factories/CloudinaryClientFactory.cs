using CloudinaryDotNet;
using ImagesAndFilesStorage.Configuration;

namespace ImagesAndFilesStorage.Factories
{
    public class CloudinaryClientFactory : ICloudinaryClientFactory
    {
        private readonly ICloudinaryConfiguration _cloudinaryConfiguration;

        public CloudinaryClientFactory(ICloudinaryConfiguration cloudinaryConfiguration)
        {
            _cloudinaryConfiguration = cloudinaryConfiguration;
        }

        public Cloudinary Create()
        {
            var instance = new Cloudinary(new Account
            {
                ApiSecret = _cloudinaryConfiguration.ApiSecret,
                ApiKey = _cloudinaryConfiguration.ApiKey,
                Cloud = _cloudinaryConfiguration.CloudName
            });

            return instance;
        }
    }
}