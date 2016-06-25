using System;
using Donker.ConsoleUtils.CommandExecution;

namespace Donker.ConsoleUtils.Examples.Controllers
{
    // You can set some options on a controller with the following attribute
    // In this case, we set a prefix to use for all command routes and force routes to be case sensitive
    [CommandController(Prefix = "Say ", CaseSensitiveRouting = true)]
    public class GreetingController : CommandControllerBase
    {
        // The following command will map to "Say hello to {name}." as it uses the prefix
        [Command("hello to {name}.")]
        public void Hello(string name)
        {
            Console.WriteLine("Hello there, {0}!", name);
        }

        // We don't use the controller prefix for the following command, as that would map it to "Say Say goodbye to {name}, please." instead of "Say goodbye to {name}, please."
        // We also override the case sensitivity that we set on the controller
        [Command("Say goodbye to {name}, please.", UseControllerPrefix = false, CaseSensitiveRouting = false)]
        public void Bye(string name)
        {
            Console.WriteLine("Goodbye, {0}. I'm sorry you have to leave.", name);

            // Exit the application
            Exit();
        }
    }
}