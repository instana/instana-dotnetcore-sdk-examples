namespace InstanaSDKConsoleExample
{
    using System.Collections.Generic;
    
    internal interface IStep
    {
        string Execute(string input, Dictionary<string, object> protocol);
    }
}
