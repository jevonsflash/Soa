using DotNetty.Common.Utilities;
using DotNetty.Transport.Channels;
using Soa.Client.TransportClient;
using Soa.Protocols.Communication;

namespace Soa.Client.Transport.DotNetty.ChannelHandlerAdapter
{
    class ClientHandlerChannelHandlerAdapter : global::DotNetty.Transport.Channels.ChannelHandlerAdapter
    {
        private readonly ITransportClientFactory _factory;


        public ClientHandlerChannelHandlerAdapter(ITransportClientFactory factory
           )
        {
            _factory = factory;
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            _factory.Clients.TryRemove(context.Channel.GetAttribute(AttributeKey<string>.ValueOf(typeof(DefaultTransportClientFactory), "addresscode")).Get(), out _);
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var msg = message as SoaTransportMsg;

            var listener = context.Channel.GetAttribute(AttributeKey<IClientListener>.ValueOf(typeof(DefaultTransportClientFactory), nameof(IClientListener))).Get();
            var sender = context.Channel.GetAttribute(AttributeKey<IClientSender>.ValueOf(typeof(DefaultTransportClientFactory), nameof(IClientSender))).Get();

            listener.Received(sender, msg);
        }

    }
}
