using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Donker.ConsoleUtils.CommandExecution
{
    /// <summary>
    /// Service for mapping and executing methods for specific command lines.
    /// </summary>
    public class CommandService
    {
        private readonly List<Type> _registeredControllerTypes;
        private readonly List<CommandData> _commandDataCollection;
        private readonly Dictionary<Type, ICommandArgumentParser> _argumentParsers;

        private readonly object _syncRoot;

        private object _lastResult;

        /// <summary>
        /// Event triggered when a <see cref="CommandControllerBase"/> method requests to exit the application.
        /// </summary>
        public event EventHandler<CommandServiceEventArgs> Exit;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandService"/> class.
        /// </summary>
        public CommandService()
        {
            _registeredControllerTypes = new List<Type>();
            _commandDataCollection = new List<CommandData>();
            _argumentParsers = new Dictionary<Type, ICommandArgumentParser>();

            _syncRoot = new object();
        }

        /// <summary>
        /// Maps <see cref="CommandControllerBase"/> methods, using the <see cref="CommandAttribute"/> attribute, to their respective routes.
        /// </summary>
        /// <param name="controller">The <see cref="CommandControllerBase"/> instance containing the methods to register.</param>
        /// <exception cref="ArgumentNullException"><paramref name="controller"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">A controller of the same type has already been registered.</exception>
        /// <exception cref="FormatException">Multiple route arguments need to be separated from each other.</exception>
        public void RegisterController(CommandControllerBase controller)
        {
            if (controller == null)
                throw new ArgumentNullException(nameof(controller), "'controller' cannot be null.");

            lock (_syncRoot)
            {
                RegisterControllerInternal(controller);
            }
        }

        /// <summary>
        /// Maps <see cref="CommandControllerBase" /> methods, using the <see cref="CommandAttribute" /> attribute, to their respective routes.
        /// </summary>
        /// <param name="controllerType">The <see cref="Type" /> of the <see cref="CommandControllerBase" /> containing the methods to register.</param>
        /// <exception cref="ArgumentNullException"><paramref name="controllerType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="controllerType"/> does not implement the <see cref="CommandControllerBase"/> class
        /// or a controller of the same type has already been registered.
        /// </exception>
        public void RegisterController(Type controllerType)
        {
            if (controllerType == null)
                throw new ArgumentNullException(nameof(controllerType), "'controllerType' cannot be null.");
            if (!typeof(CommandControllerBase).IsAssignableFrom(controllerType))
                throw new ArgumentException("'controllerType' needs to implement the CommandControllerBase class.");

            CommandControllerBase controller = (CommandControllerBase)Activator.CreateInstance(controllerType);

            lock (_syncRoot)
            {
                RegisterControllerInternal(controller);
            }
        }

        /// <summary>
        /// Registers the controller.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="CommandControllerBase"/> to register.</typeparam>
        public void RegisterController<T>()
            where T : CommandControllerBase, new()
        {
            CommandControllerBase controller = new T();

            lock (_syncRoot)
            {
                RegisterControllerInternal(controller);
            }
        }

        /// <summary>
        /// Registers the controller and all others that are in the same assembly and namespace.
        /// </summary>
        /// <param name="controllerType">The type of the <see cref="CommandControllerBase"/> to register.</param>
        public void RegisterControllersAt(Type controllerType)
        {
            if (controllerType == null)
                throw new ArgumentNullException(nameof(controllerType), "'controllerType' cannot be null.");

            Type baseType = typeof(CommandControllerBase);

            if (!baseType.IsAssignableFrom(controllerType))
                throw new ArgumentException("'controllerType' needs to implement the CommandControllerBase class.", nameof(controllerType));

            IEnumerable<Type> controllerTypes = controllerType.Assembly
                .GetTypes()
                .Where(t =>
                    t.Namespace == controllerType.Namespace
                    && baseType.IsAssignableFrom(t)
                    && t.GetConstructors().Any(ctor => ctor.GetParameters().Length == 0));

            lock (_syncRoot)
            {
                foreach (Type siblingControllerType in controllerTypes)
                {
                    CommandControllerBase siblingController = (CommandControllerBase)Activator.CreateInstance(siblingControllerType);
                    RegisterControllerInternal(siblingController);
                }
            }
        }

        /// <summary>
        /// Registers the controller and all others that are in the same assembly and namespace.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="CommandControllerBase"/> to register.</typeparam>
        public void RegisterControllersAt<T>()
            where T : CommandControllerBase, new()
        {
            RegisterControllersAt(typeof(T));
        }

        /// <summary>
        /// Builds a list with information about all the routes that are currently registered.
        /// </summary>
        /// <returns>A <see cref="List{T}"/> with the routes as <see cref="CommandRouteDescription"/> objects.</returns>
        public IList<CommandRouteDescription> BuildRouteDescriptions()
        {
            lock (_syncRoot)
            {
                List<CommandRouteDescription> routeList = new List<CommandRouteDescription>();

                foreach (Type controllerType in _registeredControllerTypes)
                {
                    foreach (CommandData commandData in _commandDataCollection)
                    {
                        if (commandData.ControllerInstance.GetType() != controllerType)
                            continue;

                        StringBuilder routeBuilder = new StringBuilder();

                        foreach (CommandRoutePart routePart in commandData.RouteParts)
                        {
                            if (!routePart.IsArgument)
                                routeBuilder.Append(routePart.Text + ' ');
                            else
                            {
                                CommandArgument argument = commandData.Arguments.FirstOrDefault(a => string.Equals(a.Name, routePart.Text));
                                if (argument != null)
                                    routeBuilder.AppendFormat("{{{0}:{1}}} ", routePart.Text, argument.Type.Name);
                                else
                                    routeBuilder.AppendFormat("{{{0}}} ", routePart.Text);
                            }
                        }

                        routeList.Add(new CommandRouteDescription(
                            routeBuilder.ToString(0, routeBuilder.Length - 1),
                            commandData.Description,
                            commandData.Method));
                    }
                }

                return routeList;
            }
        }

        /// <summary>
        /// Adds an argument parser to the collection.
        /// </summary>
        /// <param name="argumentParser">The instance of the argument parser to add.</param>
        /// <exception cref="ArgumentNullException"><paramref name="argumentParser"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">An argument parser of the same type has already been added.</exception>
        public void AddArgumentParser(ICommandArgumentParser argumentParser)
        {
            if (argumentParser == null)
                throw new ArgumentNullException(nameof(argumentParser), "'argumentParser' cannot be null.");

            lock (_syncRoot)
            {
                AddArgumentParserInternal(argumentParser);
            }
        }

        /// <summary>
        /// Adds an argument parser to the collectiion.
        /// </summary>
        /// <param name="argumentParserType">Type of the argument parser to add.</param>
        /// <exception cref="ArgumentNullException"><paramref name="argumentParserType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="argumentParserType"/> does not implement the <see cref="ICommandArgumentParser"/> interface
        /// or an argument parser of the same type has already been added.
        /// </exception>
        public void AddArgumentParser(Type argumentParserType)
        {
            if (argumentParserType == null)
                throw new ArgumentNullException(nameof(argumentParserType), "'argumentParserType' cannot be null.");
            if (!typeof(ICommandArgumentParser).IsAssignableFrom(argumentParserType))
                throw new ArgumentException("'argumentParserType' needs to implement the ICommandArgumentParser interface.", nameof(argumentParserType));

            ICommandArgumentParser argumentParser = (ICommandArgumentParser)Activator.CreateInstance(argumentParserType);

            lock (_syncRoot)
            {
                AddArgumentParserInternal(argumentParser);
            }
        }

        /// <summary>
        /// Adds an argument parser to the collection.
        /// </summary>
        /// <typeparam name="T">The type of the argument parser to add.</typeparam>
        public void AddArgumentParser<T>()
            where T : ICommandArgumentParser, new()
        {
            ICommandArgumentParser argumentParser = new T();

            lock (_syncRoot)
            {
                AddArgumentParserInternal(argumentParser);
            }
        }

        /// <summary>
        /// Registers the argument parsers and all others that are in the same assembly and namespace.
        /// </summary>
        /// <param name="argumentParserType">The type of the <see cref="ICommandArgumentParser"/> to register.</param>
        public void AddArgumentParsersAt(Type argumentParserType)
        {
            if (argumentParserType == null)
                throw new ArgumentNullException(nameof(argumentParserType), "'argumentParserType' cannot be null.");

            Type baseType = typeof(ICommandArgumentParser);

            if (!baseType.IsAssignableFrom(argumentParserType))
                throw new ArgumentException("'argumentParserType' needs to implement the ICommandArgumentParser interface.", nameof(argumentParserType));

            IEnumerable<Type> argumentParserTypes = argumentParserType.Assembly
                .GetTypes()
                .Where(t =>
                    t.Namespace == argumentParserType.Namespace
                    && baseType.IsAssignableFrom(t)
                    && t.GetConstructors().Any(ctor => ctor.GetParameters().Length == 0));

            lock (_syncRoot)
            {
                foreach (Type siblingArgumentParserType in argumentParserTypes)
                {
                    ICommandArgumentParser siblingArgumentParser = (ICommandArgumentParser)Activator.CreateInstance(siblingArgumentParserType);
                    AddArgumentParserInternal(siblingArgumentParser);
                }
            }
        }

        /// <summary>
        /// Registers the argument parsers and all others that are in the same assembly and namespace.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="ICommandArgumentParser"/> to register.</typeparam>
        public void AddArgumentParsersAt<T>()
            where T : ICommandArgumentParser, new()
        {
            AddArgumentParsersAt(typeof(T));
        }

        /// <summary>
        /// Executes the specified command line.
        /// </summary>
        /// <param name="commandLine">The command line.</param>
        /// <returns>Result information as a <see cref="CommandResult"/> object.</returns>
        /// <exception cref="CommandException">A detailed exception wrapper, thrown when execution goes wrong.</exception>
        public CommandResult Execute(string commandLine)
        {
            if (string.IsNullOrEmpty(commandLine))
                throw new CommandException("No command line specified.", commandLine, CommandErrorCode.EmptyInput, null);

            lock (_syncRoot)
            {
                foreach (CommandData commandData in _commandDataCollection)
                {
                    Dictionary<string, string> argumentValues;
                    if (CommandRouteParser.TryParseCommandLine(commandLine, commandData.RouteParts, commandData.CaseSensitive, out argumentValues))
                    {
                        int argumentCount = commandData.Arguments.Count;
                        object[] parsedArguments = new object[argumentCount];

                        for (int i = 0; i < argumentCount; i++)
                        {
                            CommandArgument argumentData = commandData.Arguments[i];
                            string argumentValue;

                            ICommandArgumentParser argumentParser;

                            if (!argumentValues.TryGetValue(argumentData.Name, out argumentValue))
                            {
                                parsedArguments[i] = argumentData.Type.IsValueType
                                    ? Activator.CreateInstance(argumentData.Type)
                                    : null;
                            }
                            else if (_argumentParsers.TryGetValue(argumentData.Type, out argumentParser))
                            {
                                try
                                {
                                    parsedArguments[i] = argumentParser.Parse(argumentData.Name, argumentValue);
                                }
                                catch (Exception ex)
                                {
                                    throw new CommandException(commandLine, "An argument parser threw an exception.", CommandErrorCode.ArgumentParseError, ex);
                                }
                            }
                            else if (typeof(IConvertible).IsAssignableFrom(argumentData.Type))
                            {
                                try
                                {
                                    parsedArguments[i] = Convert.ChangeType(argumentValue, argumentData.Type);
                                }
                                catch (Exception ex)
                                {
                                    throw new CommandException(commandLine, "There was an error converting an argument.", CommandErrorCode.ArgumentParseError, ex);
                                }
                            }
                            else
                            {
                                throw new CommandException(commandLine, $"Could not find a way to parse argument '{argumentData.Name}'.", CommandErrorCode.ArgumentParseError, null);
                            }
                        }

                        CommandControllerBase controller = commandData.ControllerInstance;
                        controller.CommandServiceInternal = this;
                        controller.CommandLineInternal = commandLine;
                        controller.LastResultInternal = _lastResult;

                        try
                        {
                            _lastResult = commandData.Method.Invoke(controller, parsedArguments);
                        }
                        catch (Exception ex)
                        {
                            throw new CommandException("An error occure while executing the command method.", commandLine, CommandErrorCode.CommandError, ex);
                        }

                        return new CommandResult(_lastResult, controller.GetType(), commandData.Method);
                    }
                }

                throw new CommandException("No route matched the input.", commandLine, CommandErrorCode.UnknownCommand, null);
            }
        }

        /// <summary>
        /// Reads and executes a command line.
        /// </summary>
        /// <returns>Result information as a <see cref="CommandResult"/> object.</returns>
        /// <exception cref="CommandException">A detailed exception wrapper, thrown when execution goes wrong.</exception>
        public CommandResult ReadAndExecute()
        {
            return Execute(Console.ReadLine());
        }

        /// <summary>
        /// Reads and executes a command line using the specified function.
        /// </summary>
        /// <param name="readFunc">The function to use for reading the line.</param>
        /// <returns>Result information as a <see cref="CommandResult"/> object.</returns>
        /// <exception cref="CommandException">A detailed exception wrapper, thrown when execution goes wrong.</exception>
        public CommandResult ReadAndExecute(Func<string> readFunc)
        {
            return Execute(readFunc.Invoke());
        }

        internal void FireExitEvent(CommandControllerBase controller)
        {
            EventHandler<CommandServiceEventArgs> exitEventHandler = Exit;
            exitEventHandler?.Invoke(this, new CommandServiceEventArgs(this, controller));
        }

        private void RegisterControllerInternal(CommandControllerBase controller)
        {
            Type controllerType = controller.GetType();

            if (_registeredControllerTypes.Contains(controllerType))
                throw new ArgumentException("A controller of the same type has already been registered.", nameof(controller));

            string prefix = string.Empty;
            bool controllerCaseSensitive = false;

            CommandControllerAttribute controllerAttribute = controllerType.GetCustomAttributes(typeof(CommandControllerAttribute), true).FirstOrDefault() as CommandControllerAttribute;

            if (controllerAttribute != null)
            {
                prefix = controllerAttribute.Prefix;
                controllerCaseSensitive = controllerAttribute.CaseSensitiveRouting;
            }

            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;

            foreach (MethodInfo method in controllerType.GetMethods(flags))
            {
                foreach (CommandAttribute attribute in method.GetCustomAttributes(typeof(CommandAttribute), true))
                {
                    string fullRoute = attribute.UseControllerPrefix
                        ? prefix + attribute.Route
                        : attribute.Route;

                    CommandData commandData = new CommandData
                    {
                        Arguments = method.GetParameters()
                            .Select(p => new CommandArgument { Name = p.Name, Type = p.ParameterType })
                            .ToList(),
                        CaseSensitive = attribute.IsCaseSensitiveRoutingSet ? attribute.CaseSensitiveRouting : controllerCaseSensitive,
                        ControllerInstance = controller,
                        Method = method,
                        RouteParts = CommandRouteParser.GetRouteParts(fullRoute),
                        Description = attribute.Description
                    };

                    _commandDataCollection.Add(commandData);
                }
            }

            _registeredControllerTypes.Add(controllerType);
        }

        private void AddArgumentParserInternal(ICommandArgumentParser argumentParser)
        {
            if (_argumentParsers.ContainsKey(argumentParser.ResultType))
                throw new ArgumentException("An argument parser of the same type has already been added.", nameof(argumentParser));

            _argumentParsers.Add(argumentParser.ResultType, argumentParser);
        }
    }
}