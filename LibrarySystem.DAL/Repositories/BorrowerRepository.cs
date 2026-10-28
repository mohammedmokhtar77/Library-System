using LibrarySystem.BLL.Interfaces.Repositories;
using LibrarySystem.DAL.Data;
using LibrarySystem.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.DAL.Repositories;

public class BorrowerRepository(LibraryContext context) : 
    GenericRepository<Borrower , int>(context),IBorrowerRepository 
{ }