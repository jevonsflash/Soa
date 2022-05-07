﻿using System;
using System.Threading.Tasks;
using Soa.Client.TransportClient;
using Soa.Protocols.Communication;

namespace Soa.Client.Transport.Http
{
    public class HttpClientListener : IClientListener
    {
        public event Func<IClientSender, SoaTransportMsg, Task> OnReceived;

        public async Task Received(IClientSender sender, SoaTransportMsg message)
        {
            if (OnReceived == null)
                return;
            await OnReceived(sender, message);
        }
    }
}
