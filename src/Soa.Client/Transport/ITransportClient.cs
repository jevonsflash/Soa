using System.Threading.Tasks;
using Soa.Protocols.Communication;

namespace Soa.Client.TransportClient
{
    public interface ITransportClient
    {
        /// <summary>
        ///     send request
        /// </summary>
        /// <param name="invokeMessage"></param>
        /// <returns></returns>
        Task<SoaRemoteCallResultData> SendAsync(SoaRemoteCallData invokeMessage);

        //Task OnReceive(TransportMessage message);
    }
}