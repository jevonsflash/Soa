using System.Collections.Specialized;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace Soa.Client.ApiGateway.Core
{
    [ModelBinder(BinderType = typeof(SoaQueryStringModelBinder))]
    public class SoaQueryString
    {
        public string Query { get; set; }

        public NameValueCollection Collection { get; set; }
        public SoaQueryString(string query)
        {
            Query = query;
            Collection = HttpUtility.ParseQueryString(query);
        }
    }
}
