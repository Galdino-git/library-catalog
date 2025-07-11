using MyLib.Application.DTOs;
using MyLib.Application.Handlers.Books.Commands.RegisterBook;
using MyLib.Application.Handlers.Books.Commands.UpdateBook;
using MyLib.Domain.Entities;

namespace MyLib.Application.Mappers
{
    public class BookMappingProfile : Profile
    {
        public BookMappingProfile()
        {
            CreateMap<RegisterBookCommand, Book>()
                // Reforcing the mapping to ignore certain properties, even if they are not present in the command
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());


            CreateMap<UpdateBookCommand, Book>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.RegisteredByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.RegisteredByUser, opt => opt.Ignore());


            CreateMap<Book, BookDetailsDto>()
                .ForMember(dest => dest.RegisteredByUserName, opt => opt.MapFrom(src => src.RegisteredByUser != null ? src.RegisteredByUser.Username : string.Empty))
                .ForMember(dest => dest.LastUpdated, opt => opt.MapFrom(src => src.UpdatedAt ?? src.CreatedAt));
        }
    }
}
