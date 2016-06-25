using System;
using System.Security;
using Donker.ConsoleUtils.PasswordBuilders;

namespace Donker.ConsoleUtils
{
    /// <summary>
    /// Wrapper with additional methods for the <see cref="Console"/> class.
    /// </summary>
    public static partial class ConsoleEx
    {
        private const char DefaultPasswordOutputChar = '*';
        private const int DefaultMaxPasswordLength = 0;

        #region ReadPassword overloads

        /// <summary>
        /// Reads and masks a password from the input.
        /// </summary>
        /// <param name="outputChar">The character to output instead.</param>
        /// <param name="maxLength">The maximum length of the password.</param>
        /// <returns>The password as <see cref="string"/>.</returns>
        public static string ReadPassword(char outputChar, int maxLength)
        {
            PasswordBuilder passwordBuilder = new PasswordBuilder();
            string result = ReadPasswordInternal(outputChar, maxLength, passwordBuilder);
            Console.WriteLine();
            return result;
        }

        /// <summary>
        /// Reads and masks a password from the input.
        /// </summary>
        /// <param name="outputChar">The character to output instead.</param>
        /// <returns>The password as <see cref="string"/>.</returns>
        public static string ReadPassword(char outputChar) => ReadPassword(outputChar, DefaultMaxPasswordLength);

        /// <summary>
        /// Reads and masks a password from the input.
        /// </summary>
        /// <param name="maxLength">The maximum length of the password.</param>
        /// <returns>The password as <see cref="string"/>.</returns>
        public static string ReadPassword(int maxLength) => ReadPassword(DefaultPasswordOutputChar, maxLength);

        /// <summary>
        /// Reads and masks a password from the input.
        /// </summary>
        /// <returns>The password as <see cref="string"/>.</returns>
        public static string ReadPassword() => ReadPassword(DefaultPasswordOutputChar, DefaultMaxPasswordLength);

        #endregion

        #region ReadPasswordSecure overloads

        /// <summary>
        /// Reads and masks a password from the input as a <see cref="SecureString"/>.
        /// </summary>
        /// <param name="outputChar">The character to output instead.</param>
        /// <param name="maxLength">The maximum length of the password.</param>
        /// <returns>The password as <see cref="SecureString"/>.</returns>
        public static SecureString ReadPasswordSecure(char outputChar, int maxLength)
        {
            SecureString result;

            using (SecurePasswordBuilder passwordBuilder = new SecurePasswordBuilder())
            {
                result = ReadPasswordInternal(outputChar, maxLength, passwordBuilder);
                Console.WriteLine();
            }
            
            return result;
        }

        /// <summary>
        /// Reads and masks a password from the input as a <see cref="SecureString"/>.
        /// </summary>
        /// <param name="outputChar">The character to output instead.</param>
        /// <returns>The password as <see cref="SecureString"/>.</returns>
        public static SecureString ReadPasswordSecure(char outputChar) => ReadPasswordSecure(outputChar, DefaultMaxPasswordLength);

        /// <summary>
        /// Reads and masks a password from the input as a <see cref="SecureString"/>.
        /// </summary>
        /// <param name="maxLength">The maximum length of the password.</param>
        /// <returns>The password as <see cref="SecureString"/>.</returns>
        public static SecureString ReadPasswordSecure(int maxLength) => ReadPasswordSecure(DefaultPasswordOutputChar, maxLength);

        /// <summary>
        /// Reads and masks a password from the input as a <see cref="SecureString"/>.
        /// </summary>
        /// <returns>The password as <see cref="SecureString"/>.</returns>
        public static SecureString ReadPasswordSecure() => ReadPasswordSecure(DefaultPasswordOutputChar, DefaultMaxPasswordLength);

        #endregion

        private static TResult ReadPasswordInternal<TResult>(char outputChar, int maxLength, IPasswordBuilder<TResult> passwordBuilder)
        {
            ConsoleKeyInfo keyInfo;

            while ((keyInfo = Console.ReadKey(true)).Key != ConsoleKey.Enter)
            {
                int currentLength = passwordBuilder.Length;

                if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    if (currentLength > 0)
                    {
                        passwordBuilder.Backspace();
                        Console.Write("\b \b");
                    }
                }
                else if (maxLength == 0 || currentLength < maxLength)
                {
                    passwordBuilder.AddChar(keyInfo.KeyChar);
                    Console.Write(outputChar);
                }
            }

            return passwordBuilder.GetResult();
        }
    }
}