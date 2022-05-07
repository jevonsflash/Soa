using System.Threading.Tasks;
using Abp.Logging;
using Microsoft.AspNetCore.Http;
using Soa.Protocols.Communication;
using Soa.Protocols.Communication;
using Soa.Serializer;
using Soa.Server.TransportServer;

namespace Soa.Server.Transport.Http
{
    public class HttpResponse : IResponse
    {
        readonly Microsoft.AspNetCore.Http.HttpResponse _httpResponse;
        private readonly ISerializer _serializer;
        public HttpResponse(Microsoft.AspNetCore.Http.HttpResponse httpResponse, ISerializer serializer)
        {
            _httpResponse = httpResponse;
            _serializer = serializer;
        }
        public Task WriteAsync(string messageId, SoaRemoteCallResultData resultMessage)
        {
            LogHelper.Logger.Debug($"finish handling msg: {messageId}");
            var data = _serializer.Serialize<string>(new SoaTransportMsg(messageId, resultMessage));
            //_httpResponse.
            return _httpResponse.WriteAsync(data);

        }
    }
}
