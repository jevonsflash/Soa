using Abp.Dependency;
using System;

namespace Soa.Serializer
{
    public interface ISerializer : ISingletonDependency
    {
        T Serialize<T>(object instance);

        TResult Deserialize<T, TResult>(T data) where TResult : class;
        object Deserialize<T>(T data, Type type);
    }
}