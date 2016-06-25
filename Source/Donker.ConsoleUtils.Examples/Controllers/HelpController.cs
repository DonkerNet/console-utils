using System;
using System.Linq;
using Donker.ConsoleUtils.CommandExecution;

namespace Donker.ConsoleUtils.Examples.Controllers
{
    public class HelpController : CommandControllerBase
    {
        // You can map multiple routes to a command
        [Command("show commands", Description = "Shows all available commands")]
        [Command("help", Description = "Shows all available commands")]
        [Command("/?", Description = "Shows all available commands")]
        public void ShowCommands()
        {
            var routes = CommandService.BuildRouteDescriptions()
                .Select(r => $"{r.Route}{Environment.NewLine}    {r.Description}");

            Console.WriteLine("Available commands:");

            ConsoleEx.WritePage(routes, 3, PageSizeReachedCallback);
        }

        // When ConsoleEx.WritePage reached the maximum number of items per page, we wait for the user to continue or cancel
        private void PageSizeReachedCallback(int index, int lineCount, out bool showNextPage)
        {
            bool shouldContinue = true;
            bool shouldReadKey = true;

            Console.Write(". . . Press ENTER to show more or CTRL+C to cancel . . .");

            while (shouldContinue && shouldReadKey)
            {
                Console.TreatControlCAsInput = true;
                ConsoleKeyInfo pressedKey = Console.ReadKey(true);
                Console.TreatControlCAsInput = false;

                switch (pressedKey.Key)
                {
                    case ConsoleKey.C:
                        if (pressedKey.Modifiers.HasFlag(ConsoleModifiers.Control))
                        {
                            Console.Write("^C");
                            shouldContinue = false;
                        }
                        break;
                    case ConsoleKey.Enter:
                        shouldReadKey = false;
                        break;
                }
            }

            Console.WriteLine();

            showNextPage = shouldContinue;
        }
    }
}