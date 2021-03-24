using Autofac;
using ImagesAndFilesStorage.Factories;
using ImagesAndFilesStorage.Services;

namespace ImagesAndFilesStorage
{
    public class ImagesAndFilesStorageModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ImageStoringService>().AsImplementedInterfaces();
            builder.RegisterType<SingleImageStorage>().AsImplementedInterfaces();
            builder.RegisterType<CloudinaryService>().AsImplementedInterfaces();
            builder.RegisterType<CloudinaryClientFactory>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<FileStoringService>().AsImplementedInterfaces();
            builder.RegisterType<SingleFileStorage>().AsImplementedInterfaces();
        }
    }
}