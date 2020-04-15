using Instana.Tracing.Sdk.Spans;
using System;
using System.Data;
using System.Threading;

namespace custom_database_example
{
    public class DatabaseClient
    {
        
        private bool _isConnected = false;
        private string _databaseName = string.Empty;
        public bool Connect(string host, string databaseName)
        {
            // create the span for the outgoing call. Since we know that we're calling
            // a database, we can annotate the call accordingly with convenient extension-methods
            // Here we use: CustomExitSpan.AsDbCallTo()
            using (var connectionSpan = CustomSpan.CreateExit(this, null)
                                                  .AsDbCallTo(host + ":" + databaseName, "CONNECT", "sql"))
            {
                if (!string.IsNullOrEmpty(host) && !string.IsNullOrEmpty(databaseName))
                {
                    Thread.Sleep(2);
                    _isConnected = true;
                    _databaseName = databaseName;
                }
                return _isConnected;
            }
        }

        public IDataReader ExecuteQuery(string queryText)
        {
            // Create the span for the query targeting the database (-fake)
            using (var commandSpan = CustomSpan.CreateExit(this, null).AsDbCallTo(_databaseName, queryText, "sql"))
            {
                // we're wrapping the basic functionality here, which allows us to collect
                // errors if they happen automatically.
                commandSpan.WrapAction(() =>
                {
                    if (!_isConnected)
                    {
                        throw new ArgumentException("The client is not connected, command can not be executed");
                    }
                    if (string.IsNullOrEmpty(queryText))
                    {
                        throw new ArgumentException("The command must not be empty!");
                    }
                    Thread.Sleep(queryText.Length);
                }, true);
                return null;
            }

        }
    }
}
