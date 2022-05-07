using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Soa.Protocols.Communication;

namespace Soa.Client.ApiGateway.Core
{
    public class SoaModelBinder : IModelBinder
    {
        private readonly IModelBinder _modelBinder;
        public SoaModelBinder(IModelBinder modelBinder)
        {
            _modelBinder = modelBinder;
        }
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            SoaModel model = null;
            var req = bindingContext.ActionContext.HttpContext.Request;
            if (req.HasFormContentType)
            {
                var form = req.Form;
                if (form != null && (form.Any() || form.Files.Any()))
                {
                    if (form.Files.Any())
                    {
                        var list = new List<SoaFile>();
                        foreach (var file in form.Files)
                        {
                            using (var sr = file.OpenReadStream())
                            {
                                var bytes = new byte[sr.Length];
                                sr.ReadAsync(bytes, 0, bytes.Length);
                                var myFile = new SoaFile
                                {
                                    FileName = file.FileName,
                                    Data = bytes
                                };
                                list.Add(myFile);
                            }
                        }

                        var data = new Dictionary<string, object> {{"files", list}};
                        model = new SoaModel(data);
                    }
                    else
                    {
                        model = new SoaModel(form);
                    }
                }
            }
            else
            {
                var body = req.Body;
                if (body != null)
                {
                    model = new SoaModel(body);
                }
            }
            bindingContext.ModelState.SetModelValue("model", model, null);
            bindingContext.Result = ModelBindingResult.Success(model);
            return Task.CompletedTask;
        }
    }
}
