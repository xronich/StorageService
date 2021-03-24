using Exceptions.Abstractions.Attributes;
using ImagesAndFilesStorage.Exceptions.Base;

namespace ImagesAndFilesStorage.Exceptions
{
    public class CloudinaryRequestException : ImagesStorageExceptionBase
    {
        [Log]
        public string Error { get; }

        public CloudinaryRequestException(string error)
        {
            Error = error;
        }
    }
}