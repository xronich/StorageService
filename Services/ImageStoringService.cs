using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Abstractions.Services.Images;
using Abstractions.Services.Images.Contracts;
using ImagesAndFilesStorage.Exceptions;
using log4net;

namespace ImagesAndFilesStorage.Services
{
    public class ImageStoringService : IImageStoringService
    {
        private readonly ICloudinaryService _cloudinaryService;
        private readonly ILog _logger = LogManager.GetLogger("ImagesAndFilesLogger");

        public ImageStoringService(ICloudinaryService cloudinaryService)
        {
            _cloudinaryService = cloudinaryService;
        }


        public async Task<StoredImageResponse> Store(StoreImageRequest request)
        {
            var storedImagesData = new List<StoredImageResponse.ImageDataBase>();

            foreach (var imageData in request.Images)
            {
                try
                {
                    var uploadParams = GenerateImageUploadData(imageData, request.ImageConfiguration);

                    var response = await _cloudinaryService.UploadImage(uploadParams);

                    var transforms = ParseTransforms(response);

                    storedImagesData.Add(new StoredImageResponse.SavedImageData
                    {
                        FileName = imageData.FileName,
                        OriginalImageUrl = response.SecureUri.ToString(),
                        ImageTransforms = transforms,
                        Height = response.Height,
                        Width = response.Width
                    });
                }
                catch (Exception e)
                {
                    storedImagesData.Add(new StoredImageResponse.CorruptedImageData
                    {
                        Error = e.Message,
                        FileName = imageData.FileName
                    });

                    _logger.Warn(
                        $"Error while uploading image. Correlation id: {request.CorrelationId}, Message: {e.Message}",
                        e);
                }
            }

            return new StoredImageResponse
            {
                StoredImages = storedImagesData
            };
        }

        private ImageUploadParams GenerateImageUploadData(StoreImageRequest.ImageData data,
            StoreImageRequest.Configuration configuration)
        {
            if (data.FileStream == null || data.FileStream.Length == 0)
            {
                throw new EmptyFileStreamException();
            }

            var transforms =
                configuration.Transforms?.Select(transform =>
                    new Transformation().Width(transform.Width).Prefix(transform.TransformName)).ToList();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(data.FileName, data.FileStream),
                EagerTransforms = transforms
            };
            return uploadParams;
        }

        private Dictionary<string, string> ParseTransforms(ImageUploadResult result)
        {
            var array = result.JsonObj["eager"]?.ToArray();

            var dictionary = array?.ToDictionary(
                token => token["transformation"]?.ToString().Split(',')[0].Substring(startIndex: 2),
                token => token["url"]?.ToString());

            return dictionary;
        }

        public async Task Remove(DeleteImagesRequest deleteImagesRequest)
        {
            foreach (var originalImageId in deleteImagesRequest.ImagesIds)
            {
                try
                {
                    var deleteRequestParams = new DelResParams
                    {
                        PublicIds = new List<string>
                        {
                            originalImageId
                        }
                    };

                    await _cloudinaryService.RemoveContent(deleteRequestParams);
                }
                catch (Exception e)
                {
                    _logger.Warn(
                        $"Error while deleting image. Correlation id: {deleteImagesRequest.CorrelationId}, Message: {e.Message}",
                        e);
                }
            }
        }

        public async Task<ImagesResponse> Get(ImagesRequest request)
        {
            var imagesResponses = await _cloudinaryService.GetImages(request.ItemsOnPage, request.NextPageCursor);


            var images = imagesResponses.Resources.Select(resource => new ImagesResponse.Image
            {
                Url = resource.Uri.ToString(),
                Id = resource.PublicId
            }).ToList();

            return new ImagesResponse
            {
                Images = images,
                NextPageCursor = imagesResponses.NextCursor
            };
        }
    }
}