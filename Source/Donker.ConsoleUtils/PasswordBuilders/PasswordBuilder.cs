using System.Text;

namespace Donker.ConsoleUtils.PasswordBuilders
{
    /// <summary>
    /// Simple class for building a password.
    /// </summary>
    public class PasswordBuilder : IPasswordBuilder<string>
    {
        private readonly StringBuilder _stringBuilder;

        /// <summary>
        /// Gets the length of the password.
        /// </summary>
        public int Length => _stringBuilder.Length;

        /// <summary>
        /// Initializes a new instance of <see cref="PasswordBuilder"/>.
        /// </summary>
        public PasswordBuilder()
        {
            _stringBuilder = new StringBuilder();
        }

        /// <summary>
        /// Adds a character to the password.
        /// </summary>
        /// <param name="c">The character to add.</param>
        public void AddChar(char c) => _stringBuilder.Append(c);

        /// <summary>
        /// Removes the last character from the current password.
        /// </summary>
        public void Backspace()
        {
            if (_stringBuilder.Length == 0)
                return;

            _stringBuilder.Remove(_stringBuilder.Length - 1, 1);
        }

        /// <summary>
        /// Returns the fully built password.
        /// </summary>
        /// <returns>The result as a <see cref="string"/> object.</returns>
        public string GetResult() => _stringBuilder.ToString();
    }
}
