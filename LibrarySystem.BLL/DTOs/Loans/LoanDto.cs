namespace LibrarySystem.BLL.DTOs.Loans;

public sealed class LoanDto
{
    public int BookId { get; set; }
    public int BorrowerId { get; set; }

    public string? BookTitle { get; set; }
    public string? BorrowerName { get; set; }
    public DateTime LoanDate { get; set; }
    public DateTime? ReturnDate { get; set; }
}