using Autofac;
using AutofacSerilogIntegration;

namespace Battleship.Application.Modules
{
    public class LoggingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterLogger();
        }
    }
}
