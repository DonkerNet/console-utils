using System;
using System.Collections.Generic;

namespace Donker.ConsoleUtils.IoC
{
    public interface IDependencyResolver
    {
        object GetService(Type type);
        IEnumerable<object> GetServices(Type type);
        T GetService<T>();
        IEnumerable<T> GetServices<T>();
    }
}
