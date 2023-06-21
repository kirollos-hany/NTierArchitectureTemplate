using Autofac;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using System.Reflection;
using Module = Autofac.Module;

namespace MyTemplate.Application;
public class ApplicationModule : Module
{ 
    protected override void Load(ContainerBuilder builder)
    {
        var configuration = MediatRConfigurationBuilder
            .Create(Assembly.GetExecutingAssembly())
            .WithAllOpenGenericHandlerTypesRegistered()
            .Build();

        builder.RegisterMediatR(configuration);
    }
}