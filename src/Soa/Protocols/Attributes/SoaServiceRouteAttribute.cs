using System;

namespace Soa.Protocols.Attributes
{
    /// <summary>
    ///     set the service route path
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
    public class SoaServiceRouteAttribute : Attribute
    {
        public SoaServiceRouteAttribute(bool isExposureToGateway = true)
        {
            IsExposureToGateway = isExposureToGateway;

        }
        public SoaServiceRouteAttribute(string routeTemplate, bool isExposureToGateway = true)
        {
            RouteTemplate = routeTemplate;
            IsExposureToGateway = isExposureToGateway;
        }

        public string RouteTemplate { get; }

        public bool IsExposureToGateway { get; set; }
    }
}