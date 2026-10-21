using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.BLL.DTOs.Books;

public sealed class UpdateBookDto
{
    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    public string ISBN { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int AuthorId { get; set; }
}