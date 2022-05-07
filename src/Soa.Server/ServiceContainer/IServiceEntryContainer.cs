using System;
using System.Collections.Generic;
using Soa.Protocols.Service;

namespace Soa.Server.ServiceContainer
{
    public interface IServiceEntryContainer
    {
        /// <summary>
        ///     get all service entries
        /// </summary>
        /// <returns></returns>
        List<SoaServiceEntry> GetServiceEntry();

        /// <summary>
        ///     add service
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        IServiceEntryContainer AddServices(Type[] types);
    }
}