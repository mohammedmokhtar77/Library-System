using LibrarySystem.BLL.Interfaces.Repositories;
using LibrarySystem.DAL.Data;
using LibrarySystem.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.DAL.Repositories;

public class BookRepository(LibraryContext context) 
    : GenericRepository<Book , int>(context),IBookRepository
{ }