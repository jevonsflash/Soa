using Abp.Authorization;
using Abp.Localization;

namespace Soa.Sample.Service2.Authorization
{

    public class Service2AuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {

            var permission1 = context.CreatePermission(PermissionNames.Pages_Service2, L("Service2"));
            permission1.CreateChildPermission(PermissionNames.Pages_Service2_Create, L("Create"));
            permission1.CreateChildPermission(PermissionNames.Pages_Service2_Edit, L("Edit"));
            permission1.CreateChildPermission(PermissionNames.Pages_Service2_Delete, L("Delete"));

           
        }


        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, "MatoProject");
        }
    }
}
