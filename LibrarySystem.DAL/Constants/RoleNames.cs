namespace LibrarySystem.DAL.Constants;

public static class RoleNames
{
    public const string Admin = "Admin";
    public const string Librarian = "Librarian";
    public const string Member = "Member";
    public const string Viewer = "Viewer";

    public static readonly string[] All =
    [
        Admin,
        Librarian,
        Member,
        Viewer
    ];
}