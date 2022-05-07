using Abp.Dependency;
using System;

namespace Soa.Client.Token
{

    public interface IServiceTokenGetter
    {
        /// <summary>
        ///     get token
        /// </summary>
        Func<string> GetToken { get; set; }
    }
}