using System;
using System.Threading.Tasks;
using Abp.Logging;
using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using Soa.Protocols.Communication;
using Soa.Protocols.Communication;
using Soa.Serializer;
using Soa.Server.TransportServer;

namespace Soa.Server.Transport.DotNetty
{
    public class DotNettyResponse : IResponse
    {
        private readonly IChannelHandlerContext _channel;
        private readonly ISerializer _serializer;
        public DotNettyResponse(IChannelHandlerContext channel, ISerializer serializer)
        {
            _channel = channel;
            _serializer = serializer;
        }
        public async Task WriteAsync(string messageId, SoaRemoteCallResultData resultMessage)
        {
            try
            {
                LogHelper.Logger.Debug($"finish handling msg: {messageId}");
                var data = _serializer.Serialize<byte[]>(new SoaTransportMsg(messageId, resultMessage));
                var buffer = Unpooled.Buffer(data.Length, data.Length);
                buffer.WriteBytes(data);
                await _channel.WriteAndFlushAsync(buffer);
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error("throw exception when response msg: " + messageId, ex);
            }
        }
    }
}
