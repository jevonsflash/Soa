using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace Soa.Sample.IAuthorizedService.Authorization
{
    public class AuthorizedServiceAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission(PermissionNames.Pages_AuthorizedService, L("AuthorizedService"));
            context.CreatePermission(PermissionNames.Pages_Movie, L("Movie"));
            context.CreatePermission(PermissionNames.Pages_Music, L("Music"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, "AuthorizedService");
        }
    }
}
