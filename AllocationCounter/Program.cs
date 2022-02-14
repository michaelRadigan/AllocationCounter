using System;
    using System.Diagnostics.Tracing;

    namespace AllocationCounter
    {
        enum ClrRuntimeEventKeywords
        {
            GC = 0x1,
            GCHandle = 0x2,
            Fusion = 0x4,
            Loader = 0x8,
            Jit = 0x10,
            Contention = 0x4000,
            Exceptions                   = 0x8000,
            Clr_Type                    = 0x80000,
            GC_AllocHigh =               0x200000,
            GC_HeapAndTypeNames       = 0x1000000,
            GC_AllocLow        =        0x2000000,
        }

        class SimpleEventListener : EventListener
        {
            public ulong countTotalEvents = 0;
            public static int keyword;

            EventSource eventSourceDotNet;

            public SimpleEventListener() { }

            // Called whenever an EventSource is created.
            protected override void OnEventSourceCreated(EventSource eventSource)
            {
                Console.WriteLine(eventSource.Name);
                if (eventSource.Name.Equals("Microsoft-Windows-DotNETRuntime"))
                {
                    EnableEvents(eventSource, EventLevel.Informational, (EventKeywords) (ClrRuntimeEventKeywords.GC_AllocHigh | ClrRuntimeEventKeywords.GC_AllocLow) );
                    eventSourceDotNet = eventSource;
                }
            }
            // Called whenever an event is written.
            protected override void OnEventWritten(EventWrittenEventArgs eventData)
            {
                
                Console.WriteLine(eventData);
                if(eventData.EventName == "GCSampledObjectAllocationHigh")
                {
                    Console.WriteLine($"Did allocate {eventData.Payload[3]} bytes");
                }
                    //eventData.EventName
                    //"BulkType"
                    //eventData.PayloadNames
                    //Count = 2
                    //    [0]: "Count"
                    //    [1]: "ClrInstanceID"
                    //eventData.Payload
                    //Count = 2
                    //    [0]: 1
                    //    [1]: 11

                    //eventData.PayloadNames
                    //Count = 5
                    //    [0]: "Address"
                    //    [1]: "TypeID"
                    //    [2]: "ObjectCountForTypeSample"
                    //    [3]: "TotalSizeForTypeSample"
                    //    [4]: "ClrInstanceID"
                    //eventData.EventName
                    //"GCSampledObjectAllocationHigh"
            }
        }

        class Program
        {
            static void Main(string[] args)
            {
                SimpleEventListener.keyword = (int)ClrRuntimeEventKeywords.GC;
                var listener = new SimpleEventListener();

                Console.WriteLine("Hello World!");

                Allocate10();
                Allocate5K();
                GC.Collect();
                Console.ReadLine();
            }
            static void Allocate10()
            {
                for (int i = 0; i < 10; i++)
                {
                    int[] x = new int[100];
                }
            }

            static void Allocate5K()
            {
                for (int i = 0; i < 5000; i++)
                {
                    int[] x = new int[100];
                }
            }
        }

    }

