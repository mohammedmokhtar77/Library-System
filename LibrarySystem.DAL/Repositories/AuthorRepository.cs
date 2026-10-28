using LibrarySystem.BLL.Interfaces.Repositories;
using LibrarySystem.DAL.Data;
using LibrarySystem.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.DAL.Repositories;

public class AuthorRepository(LibraryContext context) : 
    GenericRepository<Author , int> (context),IAuthorRepository
{ }