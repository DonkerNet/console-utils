using System;

namespace Donker.ConsoleUtils.CommandExecution
{
    /// <summary>
    /// A wrapper for any exception that occurs when executing a command.
    /// </summary>
    public class CommandException : Exception
    {
        /// <summary>
        /// Gets the command line that was input.
        /// </summary>
        public string CommandLine { get; }
        /// <summary>
        /// Gets the type of error that occured.
        /// </summary>
        public CommandErrorCode ErrorCode { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandException"/> class.
        /// </summary>
        /// <param name="message">The description of the exception.</param>
        /// <param name="commandLine">The command line that was input.</param>
        /// <param name="errorCode">The type of error that occured.</param>
        /// <param name="innerException">The exception that was thrown.</param>
        public CommandException(string message, string commandLine, CommandErrorCode errorCode, Exception innerException)
            : base(message, innerException)
        {
            CommandLine = commandLine;
            ErrorCode = errorCode;
        }
    }
}
