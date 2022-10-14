using System.Reflection;
using Autofac;
using Mapster;
using Module = Autofac.Module;

namespace MyTemplate.Common;

public class CommonModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
    }
}