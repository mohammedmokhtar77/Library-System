using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.BLL.DTOs.Authors;

public sealed class CreateAuthorDto
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public DateTime BirthDate { get; set; }
}