using System;

namespace Donker.ConsoleUtils.CommandExecution
{
    /// <summary>
    /// Contains command data describing the fired event.
    /// </summary>
    public class CommandServiceEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the <see cref="CommandService"/> that had it's event fired.
        /// </summary>
        public CommandService Service { get; }
        /// <summary>
        /// Gets the <see cref="CommandControllerBase"/> responsible for the event being fired.
        /// </summary>
        public CommandControllerBase Controller { get; }

        internal CommandServiceEventArgs(CommandService service, CommandControllerBase controller)
        {
            Service = service;
            Controller = controller;
        }
    }
}
