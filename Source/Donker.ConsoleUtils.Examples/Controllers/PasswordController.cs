using System;
using System.Security;
using Donker.ConsoleUtils.CommandExecution;

namespace Donker.ConsoleUtils.Examples.Controllers
{
    public class PasswordController : CommandControllerBase
    {
        // Demonstrates the password input functionality
        [Command("read and show my password")]
        public string ReadPasswordLine()
        {
            string password = ConsoleEx.ReadPassword();
            Console.WriteLine("Your password is: {0}", password);
            return password;
        }

        // Demonstrates the secure password input functionality
        [Command("read but hide my password")]
        public int ReadPasswordLineSecure()
        {
            int passwordLength;

            using (SecureString password = ConsoleEx.ReadPasswordSecure())
            {
                passwordLength = password.Length;
                Console.WriteLine("Your secure password length: {0}", passwordLength);
            }

            return passwordLength;
        }
    }
}