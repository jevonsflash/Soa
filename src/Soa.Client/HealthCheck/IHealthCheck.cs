using Abp.Dependency;
using System;
using System.Threading.Tasks;

namespace Soa.Client.HealthCheck
{
    /// <summary>
    ///     server health check monitor
    /// </summary>
    public interface IHealthCheck
    {
        Task CheckHealth(int timeout);
    }
}