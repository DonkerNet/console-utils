using System;

namespace Donker.ConsoleUtils.CommandExecution
{
    /// <summary>
    /// Attribute for specifying the route, as well as other settings, for a <see cref="CommandControllerBase"/> method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CommandAttribute : Attribute
    {
        private bool? _caseSensitiveRouting;
        internal bool IsCaseSensitiveRoutingSet => _caseSensitiveRouting.HasValue;

        /// <summary>
        /// Gets the route to use for executing the command method.
        /// </summary>
        public string Route { get; }
        /// <summary>
        /// Gets or sets whether case sensitive routing is used. When set, this overrides the case sensitivity setting of the <see cref="CommandControllerAttribute"/> if that was specified. Default is <c>false</c>.
        /// </summary>
        public bool CaseSensitiveRouting
        {
            get { return _caseSensitiveRouting.GetValueOrDefault(); }
            set { _caseSensitiveRouting = value; }
        }
        /// <summary>
        /// Gets or sets if the controller prefix should be used. If <c>false</c>, a prefix specified in the <see cref="CommandControllerAttribute"/> will not be used for this route. Default is <c>true</c>.
        /// </summary>
        public bool UseControllerPrefix { get; set; }
        /// <summary>
        /// Gets or sets the description that describes the purpose of the command.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandAttribute"/> class.
        /// </summary>
        /// <param name="route">The route to map the method to.</param>
        /// <exception cref="ArgumentNullException"><paramref name="route"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="route"/> is empty.</exception>
        public CommandAttribute(string route)
        {
            if (route == null)
                throw new ArgumentNullException(nameof(route), "'route' cannot be null.");
            if (route.Length == 0)
                throw new ArgumentException("'route' cannot be empty.", nameof(route));

            Route = route;
            UseControllerPrefix = true;
        }
    }
}