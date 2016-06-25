using Donker.ConsoleUtils.CommandExecution;
using Donker.ConsoleUtils.Examples.ArgumentParsers;

namespace Donker.ConsoleUtils.Examples.Loaders
{
    public static class ArgumentParserLoader
    {
        public static void Load(CommandService commandService)
        {
            commandService.AddArgumentParser<NameValueCollectionArgumentParser>();

            // Alternative if you have more parsers: commandService.AddArgumentParsersAt<NameValueCollectionArgumentParser>();
        }
    }
}