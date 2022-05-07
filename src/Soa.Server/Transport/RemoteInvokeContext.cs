using System;
using System.Linq;
using Abp.Logging;
using Soa.Protocols.Communication;
using Soa.Protocols.Service;
using Soa.Server.ServiceContainer;

namespace Soa.Server.TransportServer
{
    /// <summary>
    ///     context of service invoker
    /// </summary>
    public class RemoteCallerContext
    {
        public RemoteCallerContext(SoaTransportMsg transportMessage, IServiceEntryContainer serviceEntryContainer, IResponse response)
        {
            Response = response;
            TransportMessage = transportMessage;
            try
            {
                RemoteInvokeMessage = transportMessage.GetContent<SoaRemoteCallData>();
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error("failed to convert transportmsg.content to  SoaRemoteCallerData.", ex);
                return;
            }

            ServiceEntry = serviceEntryContainer.GetServiceEntry()
                .FirstOrDefault(x => x.Descriptor.Id == RemoteInvokeMessage.ServiceId);
            if (ServiceEntry == null)
            {
                LogHelper.Logger.Warn($"not found service: {RemoteInvokeMessage.ServiceId}");
            }
        }

        public SoaServiceEntry ServiceEntry { get; }
        public IResponse Response { get; }

        public SoaTransportMsg TransportMessage { get; }

        public SoaRemoteCallData RemoteInvokeMessage { get; }
    }
}