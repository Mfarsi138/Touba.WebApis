using Touba.WebApis.IdentityServer.DataLayer.Models;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Touba.WebApis.API.Services.Implementation
{
    public class CustomProfleService : IProfileService
    {
        private readonly UserManager<User> userManager;
        public CustomProfleService(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext profileContext)
        {
            var sub = profileContext.Subject.Identity.GetSubjectId();
            var user = await userManager.FindByNameAsync(sub);

            profileContext.IssuedClaims.AddRange(await userManager.GetClaimsAsync(user));
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.Identity.GetSubjectId();
            var user = await userManager.FindByNameAsync(sub);
            context.IsActive = user.IsActive;
        }
    }
}
