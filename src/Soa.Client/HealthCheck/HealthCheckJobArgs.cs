using System;
using Abp;
using Abp.Domain.Entities;
using Abp.Notifications;

namespace Soa.Client.HealthCheck
{
    [Serializable]
    public class HealthCheckJobArgs
    {
        public int Timeout { get; set; }
        public HealthCheckJobArgs()
        {
        }
    }

}