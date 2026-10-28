using AutoMapper;
using LibrarySystem.BLL.DTOs.Books;
using LibrarySystem.Entities.Models;

namespace LibrarySystem.BLL.DTOs.Mapping;

public class BookProfile : Profile
{
    public BookProfile()
    {
        CreateMap<Book, BookDto>();
        CreateMap<CreateBookDto, Book>();
        CreateMap<UpdateBookDto, Book>();
    }
}