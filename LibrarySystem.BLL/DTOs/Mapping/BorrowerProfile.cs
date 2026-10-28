using AutoMapper;
using LibrarySystem.BLL.DTOs.Borrowers;
using LibrarySystem.Entities.Models;

namespace LibrarySystem.BLL.DTOs.Mapping;

public class BorrowerProfile : Profile
{
    public BorrowerProfile()
    {
        CreateMap<Borrower, BorrowerDto>();
        CreateMap<CreateBorrowerDto, Borrower>();
        CreateMap<UpdateBorrowerDto, Borrower>();
    }
}