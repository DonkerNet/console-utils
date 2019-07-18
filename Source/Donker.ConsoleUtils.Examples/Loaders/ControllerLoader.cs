using Donker.ConsoleUtils.CommandExecution;
using Donker.ConsoleUtils.Examples.Controllers;

namespace Donker.ConsoleUtils.Examples.Loaders
{
    public static class ControllerLoader
    {
        public static void Load(CommandService commandService)
        {
            commandService.RegisterControllerConstant(new HelpController());
            commandService.RegisterController<GreetingController>();
            commandService.RegisterController<PasswordController>();
            commandService.RegisterController<AgeController>();

            // Alternative instead of all above statements: commandService.RegisterControllersAt<AgeController>();
        }
    }
}