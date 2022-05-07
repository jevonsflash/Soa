using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using Soa.Client.ApiGateway.Core;
using Soa.Client.RemoteCaller;
using Soa.Protocols.Communication;
using Soa.Serializer;

namespace Soa.Client.ApiGateway
{
    //[Produces("application/json")]
    //[Route("api/[controller]")]
    public class SoaServicesController : AbpController
    {
        private readonly IRemoteServiceCaller remoteServiceInvoker;
        private readonly ISerializer converter;

        public SoaServicesController(IRemoteServiceCaller remoteServiceInvoker, ISerializer converter)
        {
            this.remoteServiceInvoker = remoteServiceInvoker;
            this.converter = converter;
        }

        [HttpGet, HttpPost]
        //public async Task<object> Path(string path, [FromQuery] MyQueryString query, [FromBody] Dictionary<string, object> model)
        public async Task<IActionResult> SoaPath(string path, [FromQuery] SoaQueryString query, [ModelBinder]SoaModel model)
        {
            var paras = new Dictionary<string, object>();
            if (model?.Data != null)
            {
                paras = model.Data;
            }
            if (query.Collection.Count > 0)
            {
                foreach (var key in query.Collection.AllKeys)
                {
                    paras[key] = query.Collection[key];
                }
            }
            var result = await Invoke(path, paras);

            if (result.ResultType != typeof(SoaFile).ToString())
                return new JsonResult(result.Result);

            var file = result.Result as SoaFile;
            return File(file?.Data, "application/octet-stream", file?.FileName);
        }

        public async Task<SoaRemoteCallResultData> Invoke(string path, IDictionary<string, object> paras)
        {
            var result = await remoteServiceInvoker.InvokeAsync(path, paras);
            if (!string.IsNullOrEmpty(result.ExceptionMessage))
            {
                throw new SoaHttpStatusCodeException(400, $"{result.ToErrorString()}", path);
            }

            if (!string.IsNullOrEmpty(result.ErrorCode) || !string.IsNullOrEmpty(result.ErrorMsg))
            {
                if (int.TryParse(result.ErrorCode, out int erroCode) && erroCode > 200 && erroCode < 600)
                {
                    throw new SoaHttpStatusCodeException(erroCode, result.ToErrorString(), path);
                }

                return new SoaRemoteCallResultData { ErrorCode = result.ErrorCode, ErrorMsg = result.ErrorMsg };
            }
            if (result.ResultType == typeof(SoaFile).ToString())
            {
                var file = converter.Deserialize(result.Result, typeof(SoaFile));
                result.Result = file;
            }

            return result;
        }

    }
}