using LibrarySystem.Entities.Base;

namespace LibrarySystem.Entities.Models;

public class Book : AuditableEntity
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string ISBN { get; set; } = string.Empty;

    // Foreign Key
    public int AuthorId { get; set; }

    // Many Books belong to one Author
    public Author Author { get; set; } = null!;

    // One Book can have many Loans
    public ICollection<Loan> Loans { get; set; }
        = new List<Loan>();
}