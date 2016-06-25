namespace Donker.ConsoleUtils.CommandExecution
{
    /// <summary>
    /// Declares the type of error that occured when attempting to execute a command.
    /// </summary>
    public enum CommandErrorCode
    {
        /// <summary>
        /// No command line was specified.
        /// </summary>
        EmptyInput = 1,
        /// <summary>
        /// No route matches the input.
        /// </summary>
        UnknownCommand = 2,
        /// <summary>
        /// One or more arguments could not be parsed.
        /// </summary>
        ArgumentParseError = 3,
        /// <summary>
        /// An exception occured inside the command that was run.
        /// </summary>
        CommandError = 4
    }
}