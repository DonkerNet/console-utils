using Donker.ConsoleUtils.CommandExecution;
using Donker.ConsoleUtils.Examples.Controllers;
using Donker.ConsoleUtils.IoC;
using System;
using System.Collections.Generic;

namespace Donker.ConsoleUtils.Examples.IoC
{
    public class ExampleDependencyResolver : IDependencyResolver
    {
        public object GetService(Type type)
        {
            if (!typeof(CommandControllerBase).IsAssignableFrom(type))
                return null;

            if (typeof(AgeController).IsAssignableFrom(type))
                return new AgeController("dependency injected AgeController");

            return Activator.CreateInstance(type);
        }

        public T GetService<T>()
        {
            return (T)GetService(typeof(T));
        }

        public IEnumerable<object> GetServices(Type type)
        {
            return new[] { GetService(type) };
        }

        public IEnumerable<T> GetServices<T>()
        {
            return new[] { GetService<T>() };
        }
    }
}
