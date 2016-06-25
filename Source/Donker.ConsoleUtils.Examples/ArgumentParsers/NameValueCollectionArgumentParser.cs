using System;
using System.Collections.Specialized;
using Donker.ConsoleUtils.CommandExecution;

namespace Donker.ConsoleUtils.Examples.ArgumentParsers
{
    /// <summary>
    /// Parses strings with the format "key1=value1;key2=value2" into NameValueCollections.
    /// </summary>
    public class NameValueCollectionArgumentParser : ICommandArgumentParser
    {
        public Type ResultType => typeof(NameValueCollection);

        public object Parse(string argumentName, string argumentValue)
        {
            NameValueCollection result = new NameValueCollection();

            foreach (string kvpString in argumentValue.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] kvp = kvpString.Split(new[] { '=' }, 2, StringSplitOptions.RemoveEmptyEntries);
                if (kvp.Length == 2)
                    result.Add(kvp[0], kvp[1]);
            }

            return result;
        }
    }
}