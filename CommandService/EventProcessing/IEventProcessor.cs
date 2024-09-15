using System;

namespace CommandService.EventProcessing;

public interface IEventProcessor
{
    Task ProccessEvent(string message);
}
