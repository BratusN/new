using AutoMapper;
using CoolTool.Dto;
using CoolTool.Entity.User;

namespace CoolTool.UserService.AutoMapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserInfo, UserInfoDto>()
                .ForMember(x => x.CompanyName, opt => opt.MapFrom(src => src.Company.Name))
                .ForMember(x => x.UserId, opt => opt.MapFrom(src => src.UserInfoId))
                .ForMember(x => x.UserName, opt => opt.MapFrom(src => src.IdentityUser.UserName));
        }
    }
}
