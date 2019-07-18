using System;
using System.Collections.Generic;
using System.Reflection;

namespace Donker.ConsoleUtils.CommandExecution
{
    internal class CommandData
    {
        public bool CaseSensitive { get; set; }
        public bool InstantiatePerExecutedCommand { get; set; }
        public List<CommandRoutePart> RouteParts { get; set; }
        public Type ControllerType { get; set; }
        public CommandControllerBase ControllerInstance { get; set; }
        public MethodInfo Method { get; set; }
        public List<CommandArgument> Arguments { get; set; }
        public string Description { get; set; }
    }
}