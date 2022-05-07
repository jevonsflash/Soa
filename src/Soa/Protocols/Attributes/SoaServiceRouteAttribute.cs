using System;

namespace Soa.Protocols.Attributes
{
    /// <summary>
    ///     set the service route path
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
    public class SoaServiceRouteAttribute : Attribute
    {
        public SoaServiceRouteAttribute(string routeTemplate)
        {
            RouteTemplate = routeTemplate;
        }

        public string RouteTemplate { get; }
    }
}