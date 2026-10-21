namespace LibrarySystem.BLL.DTOs.Loans;

public sealed class UpdateLoanDto
{
    public DateTime LoanDate { get; set; }

    public DateTime? ReturnDate { get; set; }
}