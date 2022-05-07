using System.Text;
using Abp.Logging;
using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using Soa.Protocols.Communication;
using Soa.Serializer;

namespace Soa.Client.Transport.DotNetty.ChannelHandlerAdapter
{
    class ReadClientMessageChannelHandlerAdapter : global::DotNetty.Transport.Channels.ChannelHandlerAdapter
    {
        private readonly ISerializer _serializer;
        public ReadClientMessageChannelHandlerAdapter(ISerializer serializer)
        {
            _serializer = serializer;
        }
        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var buffer = (IByteBuffer)message;
            try
            {
                byte[] data = new byte[buffer.ReadableBytes];
                buffer.GetBytes(buffer.ReaderIndex, data);

                LogHelper.Logger.Debug($"received msg is: {Encoding.UTF8.GetString(data)}");
                var convertedMsg = _serializer.Deserialize<byte[], SoaTransportMsg>(data);
                if (convertedMsg.ContentType == typeof(SoaRemoteCallData).FullName)
                {
                    convertedMsg.Content = _serializer.Deserialize<string, SoaRemoteCallData>(convertedMsg.Content.ToString());
                }
                else if (convertedMsg.ContentType == typeof(SoaRemoteCallData).FullName)
                {
                    convertedMsg.Content = _serializer.Deserialize<string, SoaRemoteCallResultData>(convertedMsg.Content.ToString());
                }
                context.FireChannelRead(convertedMsg);
            }
            finally
            {
                buffer.Release();
            }
            //base.ChannelRead(context, message);
        }
    }
}
