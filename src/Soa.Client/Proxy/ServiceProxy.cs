using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Abp.Dependency;
using Soa.Client.RemoteCaller;

namespace Soa.Client.Proxy
{
    public class ServiceProxy : IServiceProxy, ISingletonDependency
    {
        private readonly IRemoteServiceCaller _remoteServiceInvoker;
        private readonly IList<Type> _serviceProxyTypes;

        public ServiceProxy(IServiceProxyGenerator generator, IRemoteServiceCaller remoteServiceInvoker)
        {
            _serviceProxyTypes = generator.GetGeneratedServiceProxyTypes().ToList();
            _remoteServiceInvoker = remoteServiceInvoker;
        }

        public T GetService<T>() where T : class
        {
            //var instanceType = typeof(T);
            var proxyType = _serviceProxyTypes.Single(typeof(T).GetTypeInfo().IsAssignableFrom);
            var instance = proxyType.GetTypeInfo().GetConstructors().First().Invoke(new object[]
            {
                _remoteServiceInvoker
            });
            return instance as T;
        }
    }
}