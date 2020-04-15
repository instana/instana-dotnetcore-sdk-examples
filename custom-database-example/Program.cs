using Instana.Tracing.Sdk.Spans;
using System;

namespace custom_database_example
{
    class Program
    {
        static void Main(string[] args)
        {

            // let's say you want to connect to a database-system which Instana does not 
            // support for automatic tracing. How would you get tracing for this?
            // in this example we pretend you had access to the client (or a wrapper around a client)
            // which you can use for instrumentation.
            Console.WriteLine("Awesome fake database tracing demo v1.0");
            DatabaseClient client = new DatabaseClient();
            while (true)
            {
                try
                {
                    Console.Write("Query >");
                    var text = Console.ReadLine();
                    // we create an entry-span as the root to our trace here, and we're pretending it was an http-call
                    // by using the AsHttpTo extensions-method...
                    
                    using (var rootSpan = CustomSpan.CreateEntry(null, null).AsHttpTo("https://somewhere.inthe.cloud/api/databaseaccess"))
                    {
                        // dig into the code of DatabaseClient to see how the rest of the
                        // trace is created by means of the SDK...
                        bool connected = client.Connect("localhost", "superdatabase");
                        var reader = client.ExecuteQuery(text);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Could not connect to the database:" + e.Message);
                }
            }

        }
    }
}
