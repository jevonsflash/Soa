using System.Threading.Tasks;
using Soa.Sample.Web.Controllers;
using Shouldly;
using Xunit;

namespace Soa.Sample.Web.Tests.Controllers
{
    public class HomeController_Tests: SampleWebTestBase
    {
        [Fact]
        public async Task Index_Test()
        {
            //Act
            var response = await GetResponseAsStringAsync(
                GetUrl<HomeController>(nameof(HomeController.Index))
            );

            //Assert
            response.ShouldNotBeNullOrEmpty();
        }
    }
}
