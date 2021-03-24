namespace ImagesAndFilesStorage.Configuration
{
    public interface ICloudinaryConfiguration
    {
        string CloudName { get; }
        string ApiKey { get; }
        string ApiSecret { get; }
    }
}