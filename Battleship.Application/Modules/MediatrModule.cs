using Autofac;
using Battleship.Domain.Aggregates.Game;
using Battleship.Domain.Core.Messaging;
using Battleship.Domain.Core.Services.Messaging;
using Battleship.Domain.Core.Services.Messaging.EventSource;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Battleship.Application.Modules;

public class MediatRModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        // Mediator itself
        builder
            .RegisterType<Mediator>()
            .As<IMediator>();

        builder.RegisterAssemblyTypes(typeof(Game).Assembly, typeof(Program).Assembly)
            .Where(t => t.Name.EndsWith("Handler"))
            .AsImplementedInterfaces();

        builder
            .RegisterType<MediatRMessageBus>()
            .As<IEventPublisher>()
            .InstancePerLifetimeScope();
    }
}



public class MediatRMessageBus : IEventPublisher, ICommandSender
{
    private readonly ILogger _logger;
    private readonly IMediator _mediator;

    public MediatRMessageBus(IMediator mediator, ILogger<IEventPublisher> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task PublishAsync<T>(T @event) where T : EventBase
    {
        _logger.LogDebug("Published {EventType} to MediatR",
            @event.GetType().Name);

        try
        {
            await _mediator.Publish(@event);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task SendAsync<T>(T command) where T : CommandBase
    {
        try
        {
            await _mediator.Send(command);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}