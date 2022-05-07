using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Abp.Logging;
using Soa.Protocols.Communication;
using Soa.Protocols.Exception;

namespace Soa.Client.TransportClient
{
    public class DefaultTransportClient : ITransportClient, IDisposable
    {
        private readonly IClientListener _listener;

        private readonly ConcurrentDictionary<string, TaskCompletionSource<SoaTransportMsg>> _resultCallbackDic =
            new ConcurrentDictionary<string, TaskCompletionSource<SoaTransportMsg>>();

        //private readonly ISerializer _serializer;
        private readonly IClientSender _sender;

        public DefaultTransportClient(IClientListener listener, IClientSender sender)
        {
            //this._serializer = serializer;
            _listener = listener;
            _sender = sender;

            _listener.OnReceived += ListenerOnReceived;
        }

        public void Dispose()
        {
            (_sender as IDisposable)?.Dispose();
            ((IDisposable)_listener)?.Dispose();
            foreach (var task in _resultCallbackDic.Values) task.TrySetCanceled();
        }

        public async Task<SoaRemoteCallResultData> SendAsync(SoaRemoteCallData data)
        {
            try
            {
                LogHelper.Logger.Debug($"prepare sending: {data.ServiceId}");
                var transportMsg = new SoaTransportMsg(data);
                var callbackTask = RegisterResultCallbackAsync(transportMsg.Id);
                try
                {
                    await _sender.SendAsync(transportMsg);
                    LogHelper.Logger.Debug($"succed to send: {data.ServiceId}, msg: {transportMsg.Id}");
                    return await callbackTask;
                }
                catch (Exception ex)
                {
                    LogHelper.Logger.Error($"error occur when connecting with server, serviceid: {data.ServiceId}", ex);
                    throw;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error($"failed to send: {data.ServiceId}", ex);
                throw new TransportException(ex.Message, ex);
            }
        }

        private Task ListenerOnReceived(IClientSender sender, SoaTransportMsg message)
        {
            LogHelper.Logger.Debug($"receive response of msg: {message.Id}");
            if (!_resultCallbackDic.TryGetValue(message.Id, out var task))
            {
                return Task.CompletedTask;
            }

            if (message.ContentType != typeof(SoaRemoteCallResultData).FullName) return Task.CompletedTask;
            task.SetResult(message);
            return Task.CompletedTask;
        }

        private async Task<SoaRemoteCallResultData> RegisterResultCallbackAsync(string id)
        {
            var task = new TaskCompletionSource<SoaTransportMsg>();
            _resultCallbackDic.TryAdd(id, task);
            try
            {
                var result = await task.Task;
                return result.GetContent<SoaRemoteCallResultData>();
            }
            finally
            {
                _resultCallbackDic.TryRemove(id, out _);
            }
        }
    }
}