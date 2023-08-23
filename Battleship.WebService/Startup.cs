using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Battleship.Domain.CommandHandlers;
using Battleship.Domain.CQRS;
using Battleship.Domain.CQRS.Events;
using Battleship.Domain.CQRS.Events.Storage;
using Battleship.Domain.CQRS.Persistence;
using Battleship.Domain.CQRS.Utilities;
using Battleship.Domain.EventHandlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace Battleship.WebService
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)   
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                //c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
                c.SwaggerDoc("v1", new OpenApiInfo(){Contact = new OpenApiContact(){Email = "dmhwhite@gmail.com" , Name = "Dave White"}, Description = "API for Battleship-CQRS"});
            });

            // Autofac
            var builder = new ContainerBuilder();
            builder.Populate(services);

            builder.RegisterModule(new AutofacModule());

            // ReadModel Persistence
            builder.RegisterType<InMemoryReadModelStorage>()
                .As<IReadModelFacade>()
                .As<IReadModelPersistence>()
                .SingleInstance();

            // Agggregate Command/Event Persistence
            builder.RegisterType<AggregateRepository>().As<IAggregateRepository>().SingleInstance();
            builder.RegisterType<InMemoryCommandRepository>().As<ICommandRepository>().SingleInstance();
            builder.RegisterType<InMemoryEventDescriptorStorage>().As<IEventDescriptorStorage>().SingleInstance();
            builder.RegisterType<EventStore>().As<IEventStore>().SingleInstance();

            // Command/Event Handling
            builder.RegisterType<CommandHandlerFactory>().As<ICommandHandlerFactory>().SingleInstance();
            builder.RegisterType<EventHandlerFactory>().As<IEventHandlerFactory>().SingleInstance();

            // MessageBus (shared)
            builder.RegisterType<MessageBus>()
                .As<ICommandSender>()
                .As<IEventPublisher>()
                .SingleInstance();

            var container = builder.Build();

            // Create the IServiceProvider based on the container.
            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();

            app.Use(async (context, next) =>
            {
                // Do work that doesn't write to the Response.
                try
                {
                    await next.Invoke();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    Debugger.Break();
                }

                // Do logging or other work that doesn't write to the Response.
            });
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseMvc();
        }
    }
}
