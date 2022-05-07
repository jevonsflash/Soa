using Microsoft.AspNetCore.Mvc;

namespace Soa.Sample.Web.Controllers
{
    public class HomeController : SampleControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}