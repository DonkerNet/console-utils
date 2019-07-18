using System;

namespace Donker.ConsoleUtils.CommandExecution
{
    /// <summary>
    /// Attribute for specifying the route prefix, as well as other settings, for a <see cref="CommandControllerBase"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandControllerAttribute : Attribute
    {
        private string _prefix;

        /// <summary>
        /// Gets or sets the prefix to prepend to every method's route.
        /// </summary>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="value"/> is empty.</exception>
        public string Prefix
        {
            get { return _prefix; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value), "'value' cannot be null.");
                if (value.Length == 0)
                    throw new ArgumentException("'value' cannot be empty.", nameof(value));
                _prefix = value;
            }
        }

        /// <summary>
        /// Gets or sets if routes should be compared with or without case sensitivity. Default is <c>false</c>.
        /// </summary>
        public bool CaseSensitiveRouting { get; set; }

        /// <summary>
        /// Gets or sets if a new instance of the controller should be created each time a command is executed. Default is <c>false</c>.
        /// </summary>
        public bool InstantiatePerExecutedCommand { get; set; }
    }
}