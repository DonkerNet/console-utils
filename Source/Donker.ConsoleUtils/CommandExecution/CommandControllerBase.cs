namespace Donker.ConsoleUtils.CommandExecution
{
    /// <summary>
    /// Base class for controllers containing methods to be executed by a <see cref="CommandService"/>.
    /// </summary>
    public abstract class CommandControllerBase
    {
        internal CommandService CommandServiceInternal;
        internal object LastResultInternal;
        internal string CommandLineInternal;

        /// <summary>
        /// Gets the instance of the <see cref="CommandService"/> that last called a command method of this controller
        /// </summary>
        protected CommandService CommandService => CommandServiceInternal;

        /// <summary>
        /// Gets the result data of the last command method called by a <see cref="CommandService"/>.
        /// </summary>
        protected object LastResult => LastResultInternal;

        /// <summary>
        /// Gets the last command line that was input by the user.
        /// </summary>
        protected string CommandLine => CommandLineInternal;

        /// <summary>
        /// Fires the exit event of the <see cref="CommandService"/> that last called a command method of this controller.
        /// </summary>
        protected void Exit() => CommandServiceInternal.FireExitEvent(this);
    }
}