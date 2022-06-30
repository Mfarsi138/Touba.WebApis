using AutoMapper;
using Touba.WebApis.Helpers.MessageBroker.Models;
using Touba.WebApis.Helpers.MessageBroker.Models.Account;
using Touba.WebApis.API.Models.Account;
using Touba.WebApis.IdentityServer.DataLayer.Models;

namespace Touba.WebApis.API.Data.AutoMapperConfig
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<MB_GetUserByRoleResponse, User>()
                .ReverseMap();

            CreateMap<User, UserProfileResponse>();

            CreateMap<UpdateUserProfileRequest, User>();

            CreateMap<UpdateUserProfileRequest, MB_UserProfileChange>();
        }
    }
}
