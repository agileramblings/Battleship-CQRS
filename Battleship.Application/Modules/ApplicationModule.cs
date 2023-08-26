using Autofac;
using Battleship.Domain.Core.Messaging;
using Battleship.Domain.Core.Services.Messaging;
using Battleship.Domain.Core.Services.Messaging.EventSource;
using Battleship.Domain.Core.Services.Persistence;
using Battleship.Domain.Core.Services.Persistence.Commands;
using Battleship.Domain.Core.Services.Persistence.CQRS;
using Battleship.Domain.Core.Services.Persistence.EventSource;
using Battleship.Domain.Core.Services.Persistence.EventSource.Aggregates;
using Battleship.Domain.Core.Services.Persistence.EventSource.Snapshot;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NodaTime;

namespace Battleship.Application.Modules;

public class ApplicationModule : Module
{
    private readonly IOwnershipContextResolver? _ownershipResolver;

    public ApplicationModule(IOwnershipContextResolver? ownershipResolver)
    {
        _ownershipResolver = ownershipResolver;
    }

    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        builder.Register(ctx => SystemClock.Instance).As<IClock>().SingleInstance();

        builder.RegisterType<DefaultCorrelationIdResolver>()
            .As<ICorrelationIdResolver>()
            .PropertiesAutowired();
        builder.RegisterType<Correlation>()
            .OnActivating(e => e.ReplaceInstance(
                new Correlation(
                    e.Context
                        .Resolve<ICorrelationIdResolver>()
                        .Resolve())));

        builder.RegisterInstance(new DefaultOwnershipContextResolver())
            .As<IOwnershipContextResolver>();

        builder.RegisterType<OwnershipContext>()
            .As<IOwnershipContext>()
            .OnActivating(e => e.ReplaceInstance(
                new OwnershipContext(
                    e.Context
                        .Resolve<IOwnershipContextResolver>()
                        .Resolve())));

        // ReadModel Persistence
        builder.RegisterType<InMemoryReadModelStorage>()
            .As<IReadModelQuery>()
            .As<IReadModelPersistence>()
            .SingleInstance();

        // Aggregate Command/Event Persistence
        builder.RegisterGeneric(typeof(DefaultSnapshotStrategy<>)).As(typeof(ISnapshotStrategy<>));
        builder.RegisterGeneric(typeof(AggregateRepository<>)).As(typeof(IAggregateRepository<>));
        builder.RegisterGeneric(typeof(EventStore<>)).As(typeof(IEventStore<>));
        builder.RegisterGeneric(typeof(InMemoryEventDescriptorStorage<>)).As(typeof(IEventDescriptorStorage<>)).SingleInstance();
        builder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>));
        builder.RegisterInstance(new NullLoggerFactory()).As<ILoggerFactory>();
        builder.RegisterType<InMemoryCommandRepository>().As<ICommandRepository>();

        // MessageBus (shared)
        builder.RegisterType<MediatRMessageBus>()
            .As<ICommandSender>()
            .As<IEventPublisher>()
            .SingleInstance();

        //builder.RegisterLogger();
    }
}