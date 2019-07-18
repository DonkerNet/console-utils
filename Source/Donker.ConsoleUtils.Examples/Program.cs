using System;
using System.Diagnostics;
using Donker.ConsoleUtils.CommandExecution;
using Donker.ConsoleUtils.Examples.IoC;
using Donker.ConsoleUtils.Examples.Loaders;
using Donker.ConsoleUtils.IoC;

namespace Donker.ConsoleUtils.Examples
{
    class Program
    {
        private static bool _canRun;

        static void Main()
        {
            // Set default colors
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;

            _canRun = true;

            // Create a dependency resolver 
            IDependencyResolver dependencyResolver = new ExampleDependencyResolver();

            // Create the command service that's responsible for all the magic, using a custom dependency resolver for creating controller instances
            CommandService commandService = new CommandService(dependencyResolver);

            // When the CommandControllerBase.Exit() method is called from within a controller, the input loop should stop so the application can exit
            commandService.Exit += (sender, args) => _canRun = false;

            // Load custom argument parsers, since the command service can only convert objects that implement IConvertible by default
            ArgumentParserLoader.Load(commandService);

            // Load all the controllers containing executable commands
            ControllerLoader.Load(commandService);
            
            Console.WriteLine("Type 'help' to show the available commands.");

            while (_canRun)
            {
                try
                {
                    ConsoleEx.Write("> ", ConsoleColor.Green);
                    
                    // Read the input, route it to a command and execute that command
                    commandService.ReadAndExecute(() => ConsoleEx.ReadLine(ConsoleColor.Green));
                }
                catch (CommandException ex)
                {
                    // Output any error that occured during the execution of the command
                    ConsoleEx.WriteLine("[ERROR: {0}] {1}", ConsoleColor.Red, ex.ErrorCode, ex.Message);
                    if (ex.InnerException != null)
                        ConsoleEx.WriteLine(ex.InnerException, ConsoleColor.Red);
                }
            }

            PressAnyKeyToContinue();
        }

        [Conditional("DEBUG")]
        private static void PressAnyKeyToContinue()
        {
            if (Debugger.IsAttached)
            {
                Console.WriteLine("Press any key to continue . . .");
                ConsoleEx.ReadKey(true);
            }
        }
    }
}
