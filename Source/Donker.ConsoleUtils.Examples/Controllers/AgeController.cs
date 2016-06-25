using System;
using System.Collections.Specialized;
using Donker.ConsoleUtils.CommandExecution;

namespace Donker.ConsoleUtils.Examples.Controllers
{
    public class AgeController : CommandControllerBase
    {
        // The 'collection' parameter is automatically converted by the custom NameValueCollectionArgumentParser
        [Command("Set ages: {collection}")]
        public NameValueCollection Set(NameValueCollection collection)
        {
            Console.WriteLine("Here it is:");

            foreach (string key in collection.AllKeys)
                ConsoleEx.WriteLine($"{key} is {collection[key]} years old.", ConsoleColor.Cyan);

            NameValueCollection lastResult = LastResult as NameValueCollection;
            if (lastResult != null)
            {
                Console.WriteLine("Previous:");

                foreach (string key in lastResult.AllKeys)
                    ConsoleEx.WriteLine($"{key} was {lastResult[key]} years old.", ConsoleColor.White);
            }

            return collection;
        }
    }
}