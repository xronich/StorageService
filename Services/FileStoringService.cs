using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Abstractions.Services.Files;
using Abstractions.Services.Files.Contracts;
using ImagesAndFilesStorage.Exceptions;
using log4net;

namespace ImagesAndFilesStorage.Services
{
    public class FileStoringService : IFileStoringService
    {
        private readonly ICloudinaryService _cloudinaryService;
        private readonly ILog _logger = LogManager.GetLogger("ImagesAndFilesLogger");

        public FileStoringService(ICloudinaryService cloudinaryService)
        {
            _cloudinaryService = cloudinaryService;
        }

        public async Task<StoredFilesResponse> Store(StoreFilesRequest request)
        {
            var storedFilesData = new List<StoredFilesResponse.FileDataBase>();

            foreach (var fileData in request.Files)
            {
                try
                {
                    var uploadParams = GenerateFileUploadData(fileData);

                    var response = await _cloudinaryService.UploadFile(uploadParams);

                    storedFilesData.Add(new StoredFilesResponse.SavedFileData
                    {
                        FileName = fileData.FileName,
                        OriginalFileUrl = response.SecureUri.ToString()
                    });
                }
                catch (Exception e)
                {
                    storedFilesData.Add(new StoredFilesResponse.CorruptedFileData
                    {
                        FileName = fileData.FileName,
                        Error = e.Message
                    });

                    _logger.Warn(
                        $"Error while uploading file. Correlation id: {request.CorrelationId}, Message: {e.Message}",
                        e);
                }
            }

            return new StoredFilesResponse
            {
                StoredFiles = storedFilesData
            };
        }

        public async Task<FilesResponse> Get(FilesRequest request)
        {
            var filesResponses = await _cloudinaryService.GetFiles(request.ItemsOnPage, request.NextPageCursor);

            var files = filesResponses.Resources.Select(resource => new FilesResponse.File
            {
                Id = resource.PublicId,
                Url = resource.Uri.ToString()
            }).ToList();

            return new FilesResponse
            {
                Files = files,
                NextPageCursor = filesResponses.NextCursor
            };
        }

        public async Task Remove(DeleteFilesRequest deleteFilesRequest)
        {
            foreach (var originalFilesId in deleteFilesRequest.FilesIds)
            {
                try
                {
                    var deleteRequestParams = new DelResParams
                    {
                        PublicIds = new List<string>
                        {
                            originalFilesId
                        }
                    };

                    await _cloudinaryService.RemoveContent(deleteRequestParams);
                }
                catch (Exception e)
                {
                    _logger.Warn(
                        $"Error while deleting file. Correlation id: {deleteFilesRequest.CorrelationId}, Message: {e.Message}",
                        e);
                }
            }
        }

        private RawUploadParams GenerateFileUploadData(StoreFilesRequest.FileData data)
        {
            if (data.FileStream == null || data.FileStream.Length == 0)
            {
                throw new EmptyFileStreamException();
            }

            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(data.FileName, data.FileStream)
            };

            return uploadParams;
        }
    }
}