namespace InstanaSDKConsoleExample
{
    using System;
    using System.Collections.Generic;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("This is an example of how to use the Instana SDK for .NET Core");
            var pipeline = new Pipeline();
            pipeline.AddStep(new ValidationStep());
            pipeline.AddStep(new WordcountStep());
            Console.Write("Pipeline has been set up, hit <enter> to execute a job");
            Console.ReadLine();
            while (true)
            {
                Console.Write("Job-Input ('Q' to quit) >");
                string input = Console.ReadLine();
                if (input.ToUpper() == "Q")
                {
                    return;
                }

                Dictionary<string, object> stepResults = new Dictionary<string, object>();
                try
                {
                    pipeline.ExecuteJob(input, stepResults);
                }
                catch
                {
                    Console.WriteLine("Error while executing pipeline");
                }
                finally
                {
                    Console.WriteLine("--- Processing Results ---");
                    foreach(string k in stepResults.Keys)
                    {
                        Console.WriteLine($"{k}\t:{stepResults[k]}");
                    }

                }
            }
        }
    }
}
