using AutoMapper;
using LibrarySystem.BLL.DTOs.Authors;
using LibrarySystem.Entities.Models;

namespace LibrarySystem.BLL.DTOs.Mapping;

public class AuthorProfile : Profile
{
    public AuthorProfile()
    {
        CreateMap<Author, AuthorDto>();
        CreateMap<CreateAuthorDto, Author>();
        CreateMap<UpdateAuthorDto, Author>();
    }
}