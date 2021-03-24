using CloudinaryDotNet;

namespace ImagesAndFilesStorage.Factories
{
    public interface ICloudinaryClientFactory
    {
        Cloudinary Create();
    }
}