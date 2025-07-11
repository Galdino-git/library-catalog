using MyLib.Application.Handlers.Users.DTOs;
using MyLib.Domain.Entities;

namespace MyLib.Application.Mappers
{
    public class PasswordResetRequestMappingProfile : Profile
    {
        public PasswordResetRequestMappingProfile()
        {
            CreateMap<PasswordResetRequest, PasswordResetRequestDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Token, opt => opt.MapFrom(src => src.Token))
                .ForMember(dest => dest.Expiration, opt => opt.MapFrom(src => src.Expiration))
                .ForMember(dest => dest.Used, opt => opt.MapFrom(src => src.Used))
                .ForMember(dest => dest.RequestedAt, opt => opt.MapFrom(src => src.RequestedAt))
                .ForMember(dest => dest.UsedAt, opt => opt.MapFrom(src => src.UsedAt));
        }
    }
}
