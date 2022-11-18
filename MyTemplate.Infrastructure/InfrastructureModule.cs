using Autofac;
using ExtCore.FileStorage.Abstractions;
using ExtCore.FileStorage.FileSystem;
using MyTemplate.Infrastructure.IO;
using MyTemplate.Infrastructure.Persistence;
using Module = Autofac.Module;
namespace MyTemplate.Infrastructure;

public class InfrastructureModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterGeneric(typeof(Repository<>))
        .As(typeof(IRepository<>))
        .As(typeof(IReadRepository<>))
        .InstancePerLifetimeScope();

        builder.RegisterType<FileStorage>()
        .As<IFileStorage>()
        .InstancePerDependency();

        builder.RegisterType<FileExtensionContentTypeProvider>()
          .As<IContentTypeProvider>()
          .InstancePerDependency();

        builder.RegisterType<FileManager>()
          .As<IFileManager>()
          .InstancePerDependency();
    }
}