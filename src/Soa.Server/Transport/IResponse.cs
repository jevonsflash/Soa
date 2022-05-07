using System.Threading.Tasks;
using Soa.Protocols.Communication;

namespace Soa.Server.TransportServer
{
    /// <summary>
    ///     server response
    /// </summary>
    public interface IResponse
    {
        Task WriteAsync(string messageId, SoaRemoteCallResultData resultMessage);
    }
}