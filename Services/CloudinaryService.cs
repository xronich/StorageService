using System.Net;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ImagesAndFilesStorage.Exceptions;
using ImagesAndFilesStorage.Factories;

namespace ImagesAndFilesStorage.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(ICloudinaryClientFactory cloudinaryClientFactory)
        {
            _cloudinary = cloudinaryClientFactory.Create();
        }

        public async Task<ImageUploadResult> UploadImage(ImageUploadParams uploadParams)
        {
            var uploadResponse = await _cloudinary.UploadAsync(parameters: uploadParams);

            if (uploadResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new CloudinaryRequestException(uploadResponse.Error.Message);
            }

            return uploadResponse;
        }

        public async Task<DelResResult> RemoveContent(DelResParams deleteParams)
        {
            var deleteResponse = await _cloudinary.DeleteResourcesAsync(deleteParams);

            if (deleteResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new CloudinaryRequestException(deleteResponse.Error.Message);
            }

            return deleteResponse;
        }

        public async Task<ListResourcesResult> GetImages(int itemsOnPage, string nextCursor)
        {
            var imagesResponse = await _cloudinary.ListResourcesAsync(new ListResourcesParams
            {
                ResourceType = ResourceType.Image,
                MaxResults = itemsOnPage,
                NextCursor = nextCursor,
                Direction = "asc"
            });

            if (imagesResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new CloudinaryRequestException(imagesResponse.Error.Message);
            }

            return imagesResponse;
        }

        public async Task<RawUploadResult> UploadFile(RawUploadParams uploadParams)
        {
            var uploadResponse = await _cloudinary.UploadAsync(parameters: uploadParams);

            if (uploadResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new CloudinaryRequestException(uploadResponse.Error.Message);
            }

            return uploadResponse;
        }

        public async Task<ListResourcesResult> GetFiles(int itemsOnPage, string nextPageCursor)
        {
            var filesResponse = await _cloudinary.ListResourcesAsync(new ListResourcesParams
            {
                ResourceType = ResourceType.Raw,
                MaxResults = itemsOnPage,
                NextCursor = nextPageCursor,
                Direction = "asc",
            });

            if (filesResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new CloudinaryRequestException(filesResponse.Error.Message);
            }

            return filesResponse;
        }
    }
}