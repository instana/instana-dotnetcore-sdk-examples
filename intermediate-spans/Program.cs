using Instana.Tracing.Sdk.Spans;
using System;

namespace intermediate_spans
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                // let's start with a simple entry-span
                using (var entrySpan = CustomSpan.CreateEntryForNewTrace(null).AsMessageReceiveFrom("out_of_nowhere"))
                {
                    // now let's do some work.
                    Worker w = new Worker();
                    w.DoSomeWork("Please do something!");
                }
                Console.WriteLine("Hit <enter> to repeat, enter 'q' to quit>");
                var input = Console.ReadLine();
                if (input == "q")
                    break;
            }
        }
    }
}
