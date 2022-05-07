using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Soa.Protocols.Service;

namespace Soa.Server.TransportServer
{
    public delegate Task RequestDel(RemoteCallerContext context);
    /// <summary>
    ///     transport server
    /// </summary>
    public interface IServer
    {
        /// <summary>
        ///     get all the service in this server
        /// </summary>
        /// <returns></returns>
        List<SoaServiceRoute> GetServiceRoutes();

        /// <summary>
        ///     start the server
        /// </summary>
        /// <returns></returns>
        Task StartAsync();

        /// <summary>
        ///     add middleware in response
        /// </summary>
        /// <param name="middleware"></param>
        /// <returns></returns>
        IServer Use(Func<RequestDel, RequestDel> middleware);
    }
}