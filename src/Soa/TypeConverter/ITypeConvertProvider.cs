using Abp.Dependency;
using System;

namespace Soa.TypeConverter
{
    public interface ITypeConvertProvider
    {
        object Convert(object instance, Type destinationType);
    }
}