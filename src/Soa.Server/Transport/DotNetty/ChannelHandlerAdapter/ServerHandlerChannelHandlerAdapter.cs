using System;
using System.Threading.Tasks;
using Abp.Logging;
using DotNetty.Transport.Channels;
using Soa.Protocols.Communication;

namespace Soa.Server.Transport.DotNetty.ChannelHandlerAdapter
{
    class ServerHandlerChannelHandlerAdapter : global::DotNetty.Transport.Channels.ChannelHandlerAdapter
    {
        private readonly Action<IChannelHandlerContext, SoaTransportMsg> _readAction;

        public ServerHandlerChannelHandlerAdapter(Action<IChannelHandlerContext, SoaTransportMsg> readAction)
        {
            _readAction = readAction;
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            Task.Run(() =>
            {
                var msg = message as SoaTransportMsg;
                _readAction(context, msg);
            });
        }

        public override void ChannelReadComplete(IChannelHandlerContext context)
        {
            context.Flush();
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            LogHelper.Logger.Error($"throw exception when communicate with server:{context.Channel.RemoteAddress}", exception);
        }

    }
}
