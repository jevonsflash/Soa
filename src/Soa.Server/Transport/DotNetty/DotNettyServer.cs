using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Abp.Logging;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Soa.Protocols.Communication;
using Soa.Protocols.Service;
using Soa.Serializer;
using Soa.Server.ServiceContainer;
using Soa.Server.Transport.DotNetty.ChannelHandlerAdapter;
using Soa.Server.TransportServer;

namespace Soa.Server.Transport.DotNetty
{
    public class DotNettyServer : IServer
    {
        private readonly List<SoaServiceRoute> _serviceRoutes = new List<SoaServiceRoute>();
        private readonly IServiceEntryContainer _serviceEntryContainer;
        private readonly DotNettyAddress _address;
        private readonly ISerializer _serializer;
        private IChannel _channel;

        private readonly Stack<Func<RequestDel, RequestDel>> _middlewares;


        public DotNettyServer(DotNettyAddress address, IServiceEntryContainer serviceEntryContainer, ISerializer serializer)
        {
            _serviceEntryContainer = serviceEntryContainer;
            _address = address;
            _serializer = serializer;
            _middlewares = new Stack<Func<RequestDel, RequestDel>>();
        }
        public List<SoaServiceRoute> GetServiceRoutes()
        {
            if (!_serviceRoutes.Any())
            {
                var serviceEntries = _serviceEntryContainer.GetServiceEntry();
                serviceEntries.ForEach(entry =>
                {
                    var serviceRoute = new SoaServiceRoute
                    {
                        Address = new List<SoaAddress> {
                             _address
                            },
                        ServiceDescriptor = entry.Descriptor
                    };
                    _serviceRoutes.Add(serviceRoute);
                });
            }

            return _serviceRoutes;
        }

        private async Task OnReceived(IChannelHandlerContext channel, SoaTransportMsg message)
        {
            LogHelper.Logger.Debug($"begin handling msg: {message.Id}");
            //TaskCompletionSource<TransportMessage> task;
            if (message.ContentType == typeof(SoaRemoteCallData).FullName)
            {
                IResponse response = new DotNettyResponse(channel, _serializer);
                var thisContext = new RemoteCallerContext(message, _serviceEntryContainer, response);

                var lastInvoke = new RequestDel(async context =>
                {
                    SoaRemoteCallResultData resultMessage = new SoaRemoteCallResultData();
                    if (context.ServiceEntry == null)
                    {
                        resultMessage.ExceptionMessage = $"can not find service {context.RemoteInvokeMessage.ServiceId}";
                        await response.WriteAsync(message.Id, resultMessage);
                    }
                    else if (context.ServiceEntry.Descriptor.WaitExecution)
                    {
                        await LocalServiceExecuteAsync(context.ServiceEntry, context.RemoteInvokeMessage, resultMessage);
                        await response.WriteAsync(message.Id, resultMessage);
                    }
                    else
                    {
                        await response.WriteAsync(message.Id, resultMessage);
                        await Task.Factory.StartNew(async () =>
                        {
                            await LocalServiceExecuteAsync(context.ServiceEntry, context.RemoteInvokeMessage, resultMessage);
                        });
                    }
                });

                foreach (var middleware in _middlewares)
                {
                    lastInvoke = middleware(lastInvoke);
                }
                await lastInvoke(thisContext);

            }
            else
            {
                LogHelper.Logger.Debug($"msg: {message.Id}, message type is not an  SoaRemoteCallData.");
            }
        }

        private async Task LocalServiceExecuteAsync(SoaServiceEntry serviceEntry, SoaRemoteCallData invokeMessage, SoaRemoteCallResultData resultMessage)
        {
            try
            {
                var cancelTokenSource = new CancellationTokenSource();
                //wait OnAuthorization(serviceEntry, cancelTokenSource ,,,,)
                if (!cancelTokenSource.IsCancellationRequested)
                {
                    var result = await serviceEntry.Func(invokeMessage.Parameters, invokeMessage.Payload);
                    var task = result as Task;
                    if (task == null)
                    {
                        resultMessage.Result = result;
                    }
                    else
                    {
                        task.Wait(cancelTokenSource.Token);
                        var taskType = task.GetType().GetTypeInfo();
                        if (taskType.IsGenericType)
                        {
                            resultMessage.Result = taskType.GetProperty("Result")?.GetValue(task);
                        }
                    }
                    resultMessage.ResultType = serviceEntry.Descriptor.ReturnType;

                }
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error("throw exception when excuting local service: " + serviceEntry.Descriptor.Id, ex);
                resultMessage.ExceptionMessage = ex.ToStackTraceString();
            }
        }



        public async Task StartAsync()
        {
            LogHelper.Logger.Info($"start server: {_address.Code}");
            var bossGroup = new MultithreadEventLoopGroup();
            var workerGroup = new MultithreadEventLoopGroup(4);
            var bootstrap = new ServerBootstrap();
            bootstrap
                .Group(bossGroup, workerGroup)
                //.Group(bossGroup, workerGroup)
                .Channel<TcpServerSocketChannel>()
                .Option(ChannelOption.SoBacklog, 100)
                .Option(ChannelOption.Allocator, PooledByteBufferAllocator.Default)
                .ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
                {
                    var pipeline = channel.Pipeline;
                    pipeline.AddLast(new LengthFieldPrepender(4));
                    pipeline.AddLast(new LengthFieldBasedFrameDecoder(int.MaxValue, 0, 4, 0, 4));
                    pipeline.AddLast(new ReadServerMessageChannelHandlerAdapter(_serializer));
                    pipeline.AddLast(new ServerHandlerChannelHandlerAdapter(async (context, message) =>
                    {
                        await OnReceived(context, message);
                    }));
                }));

            //var endpoint = new IPEndPoint(IPAddress.Parse(this.addre), this._port);
            _channel = await bootstrap.BindAsync(_address.CreateEndPoint());

            LogHelper.Logger.Info($"server start successfuly, address is： {_address.Code}");
        }

        private async Task InvokeMiddleware(RequestDel next, RemoteCallerContext context)
        {
            await next.Invoke(context);
        }

        public IServer Use(Func<RequestDel, RequestDel> middleware)
        {
            _middlewares.Push(middleware);
            return this;
        }
    }
}