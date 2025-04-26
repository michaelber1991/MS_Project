namespace Clean.Application.Interfaces;

public interface ISignalREvent
{
    string EventName { get; }
    object Payload { get; }
}