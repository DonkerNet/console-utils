using System.Reflection;

namespace Donker.ConsoleUtils.CommandExecution
{
    /// <summary>
    /// Contains information about a route and the purpose of it's command.
    /// </summary>
    public class CommandRouteDescription
    {
        /// <summary>
        /// Gets the route of the command.
        /// </summary>
        public string Route { get; }
        /// <summary>
        /// Gets the description of the command.
        /// </summary>
        public string Description { get; }
        /// <summary>
        /// Gets information about the method associated with the command.
        /// </summary>
        public MethodInfo Method { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="CommandRouteDescription"/> using the specified route and description.
        /// </summary>
        /// <param name="route">The route of the command.</param>
        /// <param name="description">The description of the command.</param>
        /// <param name="method">Information about the method associated with the command.</param>
        public CommandRouteDescription(string route, string description, MethodInfo method)
        {
            Route = route;
            Description = description;
            Method = method;
        }
    }
}