using Abp.Dependency;
using System.Reflection;

namespace Soa.ServiceId
{
    public interface IServiceIdGenerator
    {
        string GenerateServiceId(MethodInfo method);
    }
}