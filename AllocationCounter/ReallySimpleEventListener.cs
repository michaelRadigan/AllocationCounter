using System;
using System.Diagnostics.Tracing;

namespace AllocationCounter
{
    // Please note that this code is lovingly reppropriated from a fantastic Stack Overflow answer here: https://stackoverflow.com/questions/61081063/get-total-number-of-allocations-in-c-sharp   
    internal sealed class ReallySimpleEventListener : EventListener
    {
        // Called whenever an EventSource is created.
        protected override void OnEventSourceCreated(EventSource eventSource)
        {
            // Watch for the .NET runtime EventSource and enable all of its events.
            if (eventSource.Name.Equals("Microsoft-Windows-DotNETRuntime"))
            {
                EnableEvents(eventSource, EventLevel.Verbose, (EventKeywords)(-1));
            }
        }

        // Called whenever an event is written.
        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            // Write the contents of the event to the console.
            Console.WriteLine($"ThreadID = {eventData.OSThreadId} ID = {eventData.EventId} Name = {eventData.EventName}");
            for (int i = 0; i < eventData.Payload.Count; i++)
            {
                string payloadString = eventData.Payload[i]?.ToString() ?? string.Empty;
                Console.WriteLine($"\tName = \"{eventData.PayloadNames[i]}\" Value = \"{payloadString}\"");
            }
            Console.WriteLine("\n");
        }
    }
}