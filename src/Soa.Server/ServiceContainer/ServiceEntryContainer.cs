using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Transactions;
using Abp.Domain.Uow;
using Castle.MicroKernel;
using Castle.MicroKernel.Lifestyle;
using Castle.Windsor;
using Soa.Protocols.Attributes;
using Soa.Protocols.Communication;
using Soa.Protocols.Service;
using Soa.Protocols.ServiceDesc;
using Soa.ServiceId;
using Soa.TypeConverter;

namespace Soa.Server.ServiceContainer
{
    public class ServiceEntryContainer : IServiceEntryContainer
    {
        private readonly IWindsorContainer _container;
        private readonly IServiceIdGenerator _serviceIdGenerate;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ITypeConvertProvider _typeConvertProvider;
        private readonly List<SoaServiceEntry> _services;

        public ServiceEntryContainer(IWindsorContainer container, IServiceIdGenerator serviceIdGenerate,
                IUnitOfWorkManager unitOfWorkManager,
                ITypeConvertProvider typeConvertProvider)
        //public ServiceEntryContainer()
        {
            _serviceIdGenerate = serviceIdGenerate;
            _unitOfWorkManager = unitOfWorkManager;
            _container = container;
            _typeConvertProvider = typeConvertProvider;
            _services = new List<SoaServiceEntry>();
            new ConcurrentDictionary<Tuple<Type, string>, object>();
        }


        public IServiceEntryContainer AddServices(Type[] types)
        {
            //var serviceTypes = types.Where(x =>
            //{
            //    var typeinfo = x.GetTypeInfo();
            //    return typeinfo.IsInterface && typeinfo.GetCustomAttribute<SoaServiceRouteAttribute>() != null;
            //}).Distinct();

            var serviceTypes = types
                .Where(x => x.GetMethods().Any(y => y.GetCustomAttribute<SoaServiceAttribute>() != null)).Distinct();

            foreach (var type in serviceTypes)
            {
                var routeTemplate = type.GetCustomAttribute<SoaServiceRouteAttribute>();
                foreach (var methodInfo in type.GetTypeInfo().GetMethods().Where(x => x.GetCustomAttributes<SoaServiceDescAttribute>().Any()))
                {

                    SoaServiceDesc desc = new SoaServiceDesc();
                    var descriptorAttributes = methodInfo.GetCustomAttributes<SoaServiceDescAttribute>();
                    foreach (var attr in descriptorAttributes) attr.Apply(desc);
                    if (methodInfo.ReturnType.ToString().IndexOf("System.Threading.Tasks.Task", StringComparison.Ordinal) == 0 &&
                        methodInfo.ReturnType.IsGenericType)
                        desc.ReturnType = string.Join(",", methodInfo.ReturnType.GenericTypeArguments.Select(x => x.FullName));
                    else
                        desc.ReturnType = methodInfo.ReturnType.ToString();

                    if (string.IsNullOrEmpty(desc.Id))
                    {
                        desc.Id = _serviceIdGenerate.GenerateServiceId(methodInfo);
                    }

                    if (routeTemplate != null)
                        desc.RoutePath = SoaServiceRoute.ParseRoutePath(routeTemplate.RouteTemplate, type.Name,
                            methodInfo.Name, methodInfo.GetParameters());

                    var service = new SoaServiceEntry
                    {
                        Descriptor = desc,
                        Func = (paras, payload) =>
                        {

                            var instance = GetInstance(methodInfo.DeclaringType, payload);
                            var parameters = new List<object>();
                            foreach (var para in methodInfo.GetParameters())
                            {
                                paras.TryGetValue(para.Name, out var value);
                                var paraType = para.ParameterType;
                                var parameter = _typeConvertProvider.Convert(value, paraType);
                                parameters.Add(parameter);
                            }
                            using (var uow = _unitOfWorkManager.Begin(new UnitOfWorkOptions
                            {
                                Scope = TransactionScopeOption.Suppress
                            }))
                            {
                                var result = methodInfo.Invoke(instance, parameters.ToArray());
                                uow.Complete();
                                return Task.FromResult(result);
                            }

                        }
                    };

                    _services.Add(service);
                }
            }

            return this;
        }

        public List<SoaServiceEntry> GetServiceEntry()
        {
            return _services;
        }


        private object GetInstance(Type type, SoaPayload payload)
        {
            // all service are instancePerDependency, to avoid resolve the same isntance , so we add using scop here
            using (var scope = _container.BeginScope())
            {
                var args = new Arguments();
                args.AddTyped(typeof(SoaPayload), payload);
                try
                {
                    return _container.Resolve(type, args);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

            }
        }
    }
}