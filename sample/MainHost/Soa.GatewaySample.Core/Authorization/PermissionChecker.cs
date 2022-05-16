using Abp.Authorization;
using Soa.GatewaySample.Authorization.Roles;
using Soa.GatewaySample.Authorization.Users;

namespace Soa.GatewaySample.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
