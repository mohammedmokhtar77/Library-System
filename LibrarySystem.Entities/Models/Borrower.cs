using LibrarySystem.Entities.Base;

namespace LibrarySystem.Entities.Models;

public class Borrower : AuditableEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public DateTime MembershipDate { get; set; }

    // One Borrower can have many Loans
    public ICollection<Loan> Loans { get; set; }
        = new List<Loan>();
}