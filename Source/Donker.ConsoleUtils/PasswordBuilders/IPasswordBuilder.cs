namespace Donker.ConsoleUtils.PasswordBuilders
{
    /// <summary>
    /// Interface for simple password building class.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    public interface IPasswordBuilder<out T>
    {
        /// <summary>
        /// Gets the length of the password.
        /// </summary>
        int Length { get; }
        /// <summary>
        /// Adds a character to the password.
        /// </summary>
        /// <param name="c">The character to add.</param>
        void AddChar(char c);
        /// <summary>
        /// Removes the last character from the current password.
        /// </summary>
        void Backspace();
        /// <summary>
        /// Returns the fully built password.
        /// </summary>
        /// <returns>The result as a <see cref="T"/> object.</returns>
        T GetResult();
    }
}