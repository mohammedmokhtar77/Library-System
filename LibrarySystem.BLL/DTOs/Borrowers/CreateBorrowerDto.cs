using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.BLL.DTOs.Borrowers;

public sealed class CreateBorrowerDto
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public DateTime MembershipDate { get; set; }
}