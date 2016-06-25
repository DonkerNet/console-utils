using System;
using System.Collections.Generic;

namespace Donker.ConsoleUtils.CommandExecution
{
    internal static class CommandRouteParser
    {
        /*
         * Route syntax: text{parameter1}text{parameter2}etc
         * 
         * - Parameters should be enclosed by curly brackets;
         * - Parameters should be separated by at leas one character.
         */

        public static bool TryParseCommandLine(string commandLine, IEnumerable<CommandRoutePart> routeParts, bool caseSensitive, out Dictionary<string, string> argumentValues)
        {
            /*
             * The code below checks if a command line matches the supplied route parts.
             * When it's a match, it outputs the arguments that the user has input in the command lines in their string representation.
             */

            StringComparer argumentComparer;
            StringComparison keyComparison;

            if (caseSensitive)
            {
                argumentComparer = StringComparer.Ordinal;
                keyComparison = StringComparison.Ordinal;
            }
            else
            {
                argumentComparer = StringComparer.OrdinalIgnoreCase;
                keyComparison = StringComparison.OrdinalIgnoreCase;
            }

            Dictionary<string, string> tempArgumentValues = new Dictionary<string, string>(argumentComparer);

            int startIndex = 0;
            string prevArgName = string.Empty;
            bool prevWasArg = false;

            foreach (CommandRoutePart routePart in routeParts)
            {
                if (routePart.IsArgument)
                {
                    prevArgName = routePart.Text;
                    prevWasArg = true;
                }
                else
                {
                    int newStartIndex = commandLine.IndexOf(routePart.Text, startIndex, keyComparison);

                    if (newStartIndex > startIndex)
                    {
                        tempArgumentValues.Add(prevArgName, commandLine.Substring(startIndex, newStartIndex - startIndex));
                    }
                    else if (newStartIndex < 0 || (newStartIndex > 0 && !prevWasArg))
                    {
                        argumentValues = null;
                        return false;
                    }
                    
                    startIndex = newStartIndex + routePart.Text.Length;
                    prevWasArg = false;
                }
            }

            if (startIndex < commandLine.Length)
            {
                if (prevWasArg)
                {
                    tempArgumentValues.Add(prevArgName, commandLine.Substring(startIndex, commandLine.Length - startIndex));
                }
                else
                {
                    argumentValues = null;
                    return false;
                }
            }

            argumentValues = tempArgumentValues;
            return true;
        }

        public static List<CommandRoutePart> GetRouteParts(string route)
        {
            /*
             * The code below splits a route string into text and parameter parts and places these in order in a list.
             * This tells us how a route is built and which parts are important, so we can easily match a route when a user inputs a command.
             */

            List<CommandRoutePart> routeParts = new List<CommandRoutePart>();

            bool argumentStarted = false;
            bool argumentEnded = false;
            int argumentStartIndex = -1;
            int argumentEndIndex = 0;

            for (int i = 0; i < route.Length; i++)
            {
                char c = route[i];

                if (c == '{')
                {
                    argumentStarted = !argumentStarted;
                    if (argumentStarted)
                    {
                        argumentStartIndex = i + 1;
                    }
                }
                else if (c == '}')
                {
                    argumentEnded = !argumentEnded;

                    if (argumentEnded)
                    {
                        if (argumentStarted)
                        {
                            string routePart = route.Substring(argumentEndIndex, argumentStartIndex - argumentEndIndex - 1);
                            string argumentName = route.Substring(argumentStartIndex, i - argumentStartIndex);

                            if (routePart.Length > 0)
                                routeParts.Add(new CommandRoutePart { Text = routePart});
                            else if (argumentEndIndex > 0)
                                throw new FormatException("Multiple arguments need to be separated from each other.");

                            if (argumentName.Length > 0)
                                routeParts.Add(new CommandRoutePart { Text = argumentName, IsArgument = true });

                            argumentEnded = false;
                            argumentStarted = false;
                            argumentEndIndex = i + 1;
                        }
                    }
                }
            }

            if (argumentEndIndex < route.Length)
            {
                string routePart = route.Substring(argumentEndIndex, route.Length - argumentEndIndex);
                routeParts.Add(new CommandRoutePart { Text = routePart });
            }

            return routeParts;
        }
    }
}