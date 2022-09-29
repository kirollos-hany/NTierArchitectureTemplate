using System.Reflection;
using Autofac;
using Mapster;
using Module = Autofac.Module;

namespace MyTemplate.BLL;
public class BLLModule : Module
{ 
    protected override void Load(ContainerBuilder builder)
    {
        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
    }
}