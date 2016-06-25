using System;
using System.Reflection;

namespace Donker.ConsoleUtils.CommandExecution
{
    /// <summary>
    /// Contains information of a command that has been executed.
    /// </summary>
    public class CommandResult
    {
        /// <summary>
        /// Gets the data returned by the called command method.
        /// </summary>
        public object Data { get; }
        /// <summary>
        /// Gets the type of the <see cref="CommandControllerBase"/> that contains the called method.
        /// </summary>
        public Type ControllerType { get; }
        /// <summary>
        /// Gets the information of the called method.
        /// </summary>
        public MethodInfo Method { get; }

        internal CommandResult(object data, Type controllerType, MethodInfo method)
        {
            Data = data;
            ControllerType = controllerType;
            Method = method;
        }
    }
}