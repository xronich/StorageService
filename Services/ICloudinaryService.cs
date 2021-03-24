using System.Threading.Tasks;
using CloudinaryDotNet.Actions;

namespace ImagesAndFilesStorage.Services
{
    public interface ICloudinaryService
    {
        Task<ImageUploadResult> UploadImage(ImageUploadParams uploadParams);
        Task<DelResResult> RemoveContent(DelResParams deleteParams);
        Task<ListResourcesResult> GetImages(int itemsOnPage, string nextPageCursor);
        Task<RawUploadResult> UploadFile(RawUploadParams uploadParams);
        Task<ListResourcesResult> GetFiles(int itemsOnPage, string nextPageCursor);
    }
}