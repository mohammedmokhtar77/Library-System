namespace LibrarySystem.BLL.DTOs.Authors;

public sealed class AuthorDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public DateTime BirthDate { get; set; }
}