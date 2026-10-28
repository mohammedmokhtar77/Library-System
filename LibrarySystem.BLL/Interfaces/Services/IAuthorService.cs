using LibrarySystem.BLL.DTOs.Authors;
using LibrarySystem.BLL.Results;

namespace LibrarySystem.BLL.Interfaces.Services;

public interface IAuthorService
{
    public Task<GenericResult<PaginatedList<AuthorDto>>> GetAllAsync(
        int pageNumber
        , int pageSize,
        CancellationToken cancellationToken = default);
    
    public Task<GenericResult<AuthorDto>> GetByIdAsync(
        int authorId, 
        CancellationToken cancellationToken = default);
    
    public Task<GenericResult<AuthorDto>> CreateAsync(
        CreateAuthorDto dto,
        CancellationToken cancellationToken = default);
    
    public Task<Result> UpdateAsync(
        int id 
        , UpdateAuthorDto dto
        , CancellationToken cancellationToken = default);
    
    public Task<Result> DeleteAsync(
        int id
        , CancellationToken cancellationToken = default);
    
}