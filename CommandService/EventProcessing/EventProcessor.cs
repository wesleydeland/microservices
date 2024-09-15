using System;
using System.Text.Json;
using AutoMapper;
using CommandService.Data;
using CommandService.DTOs;
using CommandService.Models;

namespace CommandService.EventProcessing;

public class EventProcessor : IEventProcessor
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMapper _mapper;

    public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
    {
        _scopeFactory = scopeFactory;
        _mapper = mapper;
    }
    public async Task ProccessEvent(string message)
    {
        Console.WriteLine($"--> Processing Event - {message}");
        var eventType = DetermineEvent(message);

        switch (eventType)
        {
            case EventType.PlatformPublished:
                await AddPlatform(message);
                break;
            default:
                break;
        }
    }

    private EventType DetermineEvent(string notificationMessage)
    {
        Console.WriteLine("--> Determining Event");

        var eventType = JsonSerializer.Deserialize<GenericEventDTO>(notificationMessage);

        switch (eventType?.Event)
        {
            case "Platform_Published":
                Console.WriteLine("--> Platform Published Event Detected");
                return EventType.PlatformPublished;
            default:
                Console.WriteLine("--> Undetermined Event");
                return EventType.Undetermined;
        }
    }

    private async Task AddPlatform(string platformPublishMessage)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var repo = scope.ServiceProvider.GetRequiredService<ICommandRepository>();

            var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishDTO>(platformPublishMessage);

            try
            {
                var platfrom = _mapper.Map<Platform>(platformPublishedDto);

                if (!await repo.ExternalPlatformExists(platfrom.ExternalId))
                {
                    await repo.CreatePlatform(platfrom);
                    await repo.SaveChanges();
                }
                else
                {
                    Console.WriteLine("--> Platform already exists...");
                }

            }
            catch (System.Exception ex)
            {

                Console.WriteLine($"--> Could not add Platform to DB {ex.Message}");
            }
        }
    }
}


enum EventType
{
    PlatformPublished,
    Undetermined
}
