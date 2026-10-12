namespace Library_System.Entities;

public class Loan
{
    // Composite Primary Key + Foreign Key
    public int BookId { get; set; }

    // Composite Primary Key + Foreign Key
    public int BorrowerId { get; set; }

    public DateTime LoanDate { get; set; }

    public DateTime? ReturnDate { get; set; }

    // Navigation Properties
    public Book Book { get; set; } = null!;

    public Borrower Borrower { get; set; } = null!;
}