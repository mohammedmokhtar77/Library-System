namespace LibrarySystem.BLL.DTOs.Borrowers;

public sealed class BorrowerDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime MembershipDate { get; set; }
}