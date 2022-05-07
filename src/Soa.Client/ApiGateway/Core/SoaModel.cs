using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Soa.Client.ApiGateway.Core
{
    [ModelBinder(BinderType = typeof(SoaModelBinder))]
    public class SoaModel
    {
        public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();
        public SoaModel() { }
        public SoaModel(Stream content)
        {
            using (var sr = new StreamReader(content))
            {
                var json = sr.ReadToEnd();
                Data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            }

        }

        public SoaModel(IFormCollection form)
        {
            foreach (var f in form)
            {
                Data.Add(f.Key, f.Value);
            }
        }
        public SoaModel(Dictionary<string, object> data)
        {
            Data = data;
        }
    }
}
