using System.Reflection;
using Autofac;
using Battleship.Domain.CommandHandlers;
using Module = Autofac.Module;

namespace Battleship.Domain.CQRS.Utilities
{
    // This is used to find all CommandHandlers and EventHandlers dynamically
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var asm = Assembly.GetExecutingAssembly();
            var handlerType = typeof(ICommandHandler<>);
            builder.RegisterAssemblyTypes(asm).AsClosedTypesOf(handlerType).SingleInstance().PropertiesAutowired();
            var handlerType2 = typeof(IHandles<>);
            builder.RegisterAssemblyTypes(asm).AsClosedTypesOf(handlerType2).SingleInstance().PropertiesAutowired();
        }
    }
}