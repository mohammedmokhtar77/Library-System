using AutoMapper;
using LibrarySystem.BLL.DTOs.Loans;
using LibrarySystem.Entities.Models;

namespace LibrarySystem.BLL.DTOs.Mapping;

public class LoanProfile : Profile
{
    public LoanProfile()
    {
        CreateMap<Loan, LoanDto>()
            .ForMember(d => d.BookTitle, 
                o => o.MapFrom(s => s.Book.Title))
            .ForMember(d => d.BorrowerName, 
                o => o.MapFrom(s => s.Borrower.Name));
        CreateMap<CreateLoanDto, Loan>(); 
        CreateMap<UpdateLoanDto, Loan>();
    }
}