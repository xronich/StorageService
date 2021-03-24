using System.Resources;
using Abstractions.Exceptions.Base;

namespace ImagesAndFilesStorage.Exceptions.Base
{
    public class ImagesStorageExceptionBase : BaseException
    {
        public override ResourceManager ExceptionCodesManager => ExceptionCodes.ResourceManager;
        public override ResourceManager ExceptionMessagesManager => ExceptionMessages.ResourceManager;
    }
}