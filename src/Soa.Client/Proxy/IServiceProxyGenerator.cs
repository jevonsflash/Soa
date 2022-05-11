using System;
using System.Collections.Generic;
using System.Reflection;

namespace Soa.Client.Proxy
{
    public interface IServiceProxyGenerator
    {
        /// <summary>
        ///     generate proxy for types
        /// </summary>
        /// <param name="interfaceTypes"></param>
        /// <returns></returns>
        IEnumerable<Type> GenerateProxyTypes(IEnumerable<Type> interfaceTypes);
        Assembly GenerateProxy(IEnumerable<Type> interfaceTypes);
        Assembly GetGeneratedServiceProxyAssembly();

        /// <summary>
        ///     get all generated service proxy
        /// </summary>
        /// <returns></returns>
        IEnumerable<Type> GetGeneratedServiceProxyTypes();
    }
}