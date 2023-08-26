using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Battleship.Domain.Core.DDD;
using Battleship.Domain.Core.DDD.Exceptions;
using Battleship.Domain.Core.Messaging;
using Battleship.Domain.Core.Services.Messaging.EventSource;
using Microsoft.Extensions.Logging;

namespace Battleship.Domain.Core.Services.Persistence.EventSource;

public class EventStore<T> : IEventStore<T> where T : AggregateBase
{
    private static readonly object Lock = new();
    private readonly IEventDescriptorStorage<T> _descriptorStorage;
    private readonly ILogger _logger;
    private readonly IEventPublisher _publisher;

    public EventStore(IEventPublisher publisher, IEventDescriptorStorage<T> descriptorStorage,
        ILogger<EventStore<T>> logger)
    {
        _publisher = publisher;
        _descriptorStorage = descriptorStorage;
        _logger = logger;
    }

    public async Task<IEnumerable<string>> GetAggregateIdsAsync(int page, int count)
    {
        LogAction(nameof(GetAggregateIdsAsync));
        return await _descriptorStorage.GetAggregateIdsAsync(page, count);
    }

    public async Task PutAsync(string aggregateId, AggregateBase aggregateType, IEnumerable<EventBase> events,
        int expectedVersion, bool failOnConcurrency, bool batchSave = false)
    {
        LogAction(nameof(PutAsync));

        //TODO: DW - Remove this lock and push this up to specific eventstore impl?
        lock (Lock)
        {
            var lastVersion = _descriptorStorage.GetMostRecentVersion(aggregateId).GetAwaiter().GetResult();
            if (lastVersion >= 0)
            {
                // check whether latest event version matches current aggregate version
                // otherwise -> throw exception
                if (failOnConcurrency && lastVersion != expectedVersion) throw new ConcurrencyException(aggregateId, expectedVersion, lastVersion);
                expectedVersion = lastVersion;
            }
            else
            {
                expectedVersion = -1;
            }

            var i = expectedVersion;

            if (batchSave)
            {
                var descriptors = new List<EventDescriptor>();
                foreach (var @event in events)
                {
                    i++;
                    var newAggParams = @event.AggParams with { Version = i };
                    var eventToSave = @event with { AggParams = newAggParams };
                    descriptors.Add(new EventDescriptor(newAggParams.Owner, aggregateId, $"{aggregateType}", eventToSave, i, @event.EventParams.ReceivedOn,
                        @event.MessageId, @event.CorrelationId, @event.CausationId));
                }

                _descriptorStorage.AddDescriptorsAsync(aggregateId, descriptors)
                    .GetAwaiter().GetResult();
            }
            else
            {
                // iterate through current aggregate events increasing version with each processed event
                foreach (var @event in events)
                {
                    i++;
                    var newAggParams = @event.AggParams with { Version = i };
                    var eventToSave = @event with { AggParams = newAggParams };
                    // push event to the event descriptors list for current aggregate
                    _descriptorStorage.AddDescriptorAsync(aggregateId,
                        new EventDescriptor(newAggParams.Owner, aggregateId, $"{aggregateType}", eventToSave, i, @event.EventParams.ReceivedOn,
                            @event.MessageId, @event.CorrelationId, @event.CausationId)).GetAwaiter().GetResult();
                }
            }
        }

        foreach (var @event in events)
            // publish current event to the bus for further processing by subscribers
            await _publisher.PublishAsync(@event);
    }

    public async Task RepublishEventsForAggregate(string aggregateId)
    {
        LogAction(nameof(RepublishEventsForAggregate));

        var eventsToRepublish = await GetEventsForAggregateAsync(aggregateId);
        foreach (var @event in eventsToRepublish)
            // publish current event to the bus for further processing by subscribers
            await _publisher.PublishAsync(@event);
    }

    // collect all processed events for given aggregate and return them as a list
    // used to build up an aggregate from its history (Domain.LoadsFromHistory)
    public async Task<IEnumerable<EventBase>> GetEventsForAggregateAsync(string aggregateId)
    {
        LogAction(nameof(GetEventsForAggregateAsync));

        var eventDescriptors = (await _descriptorStorage.GetEventDescriptorsAsync(aggregateId)).ToList();
        if (!eventDescriptors.Any()) throw new AggregateNotFoundException(aggregateId);

        var events = eventDescriptors
            .Select(desc => desc.EventData);
        return events;
    }

    private void LogAction(string operation)
    {
        _logger.LogDebug("{Component} {Operation} executed", nameof(EventStore<T>), operation);
    }
}