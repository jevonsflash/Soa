using System;
using System.Collections.Concurrent;
using Abp.Dependency;
using Abp.Logging;
using Soa.Protocols.Exception;
using Soa.Protocols.Service;

namespace Soa.Client.TransportClient
{
    public class DefaultTransportClientFactory : ITransportClientFactory, ISingletonDependency
    {

        public DefaultTransportClientFactory()
        {
            //Clients = new ConcurrentDictionary<EndPoint, Lazy<ITransportClient>>();
            Clients = new ConcurrentDictionary<string, Lazy<ITransportClient>>();
        }

        //public ConcurrentDictionary<EndPoint, Lazy<ITransportClient>> Clients { get; }
        public ConcurrentDictionary<string, Lazy<ITransportClient>> Clients { get; }


        public event CreatorDelegate ClientCreatorDelegate;

        public ITransportClient CreateClient<T>(T address)
            where T : SoaAddress
        {
            try
            {
                LogHelper.Logger.Debug($"creating transport client for: {address.Code}");
                //return Clients.GetOrAdd(address.CreateEndPoint(), ep => new Lazy<ITransportClient>(() =>
                var val = Clients.GetOrAdd($"{address.ServerFlag}-{address.Code}", ep => new Lazy<ITransportClient>(() =>
               {
                   ITransportClient client = null;
                   ClientCreatorDelegate?.Invoke(address, ref client);
                   return client;
               })).Value;
                LogHelper.Logger.Debug($"succed to create transport client for: {address.Code}");
                return val;
            }
            catch (Exception ex)
            {
                //Clients.TryRemove(address.CreateEndPoint(), out _);
                Clients.TryRemove($"{address.ServerFlag}-{address.Code}", out _);
                LogHelper.Logger.Error($"failed to create transport client for : {address.Code}", ex);
                throw new TransportException(ex.Message, ex);
            }
        }
    }
}