using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Touba.WebApis.API.Helpers.Authorization
{
    /// <summary>
    /// An authorization handler that authorizes users if their role isn't among the specified excluded roles 
    /// (authorize if user dosn't have X roles)
    /// </summary>
    public class ExcludeRolesRequirement : AuthorizationHandler<ExcludeRolesRequirement>, IAuthorizationRequirement
    {
        private readonly string[] excludedRoles;

        public ExcludeRolesRequirement(string[] excludedRoles)
        {
            this.excludedRoles = excludedRoles;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ExcludeRolesRequirement requirement)
        {
            // Check if user has any of the excluded roles
            var hasExcludedRole = excludedRoles.Any(role => context.User.IsInRole(role));
            
            // Don't authorize if user has excluded role, or not authenticated
            if (hasExcludedRole || !context.User.Identity.IsAuthenticated) {
                context.Fail();
                return Task.FromResult(false);
            }

            // Authorize user if they don't have excluded role
            context.Succeed(requirement);
            return Task.FromResult(true);
        }
    }
}
