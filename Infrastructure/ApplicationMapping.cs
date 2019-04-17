using AutoMapper;
using Domains;

namespace Infrastructure
{
    public class ApplicationMapping : Profile
    {
        public ApplicationMapping()
        {
            CreateMap<ApplicationUserDto, ApplicationUser>()
                .IncludeAllDerived().ReverseMap();
            CreateMap<PhoneDto, Phone>().ReverseMap();
            CreateMap<SignupDto, ApplicationUser>()
                .IncludeAllDerived().ReverseMap();
        }
    }
}
