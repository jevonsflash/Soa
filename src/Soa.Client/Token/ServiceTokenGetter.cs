using Abp.Dependency;
using System;

namespace Soa.Client.Token
{
    /// <summary>
    ///     how to get the token
    /// </summary>
    public class ServiceTokenGetter : IServiceTokenGetter, ISingletonDependency
    {
        public Func<string> GetToken { get; set; }
    }
}