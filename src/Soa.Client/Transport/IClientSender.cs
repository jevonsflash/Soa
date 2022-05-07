using System.Threading.Tasks;
using Soa.Protocols.Communication;

namespace Soa.Client.TransportClient
{
    /// <summary>
    ///     sender for client
    /// </summary>
    public interface IClientSender
    {
        /// <summary>
        ///     send msg to server
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        Task SendAsync(SoaTransportMsg msg);
    }
}