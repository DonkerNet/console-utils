using System;

namespace Donker.ConsoleUtils.CommandExecution
{
    /// <summary>
    /// Uses custom logic to parse an argument from a <see cref="string"/> to another type.
    /// </summary>
    public interface ICommandArgumentParser
    {
        /// <summary>
        /// Gets the type to parse the <see cref="string"/> to.
        /// </summary>
        Type ResultType { get; }

        /// <summary>
        /// Parses an argument from a <see cref="string"/> to the <see cref="ResultType"/>.
        /// </summary>
        /// <param name="argumentName">The name of the argument to parse.</param>
        /// <param name="argumentValue">The value of the argument to parse as <see cref="string"/>.</param>
        /// <returns>The <paramref name="argumentValue"/> parsed to a <see cref="ResultType"/> object.</returns>
        object Parse(string argumentName, string argumentValue);
    }
}