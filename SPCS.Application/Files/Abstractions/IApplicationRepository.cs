namespace SPCS.Application.Files.Abstractions
{
    public interface IApplicationRepository
    {
        Task AddAsync(SPCS.Files.Models.Application application);
        Task<SPCS.Files.Models.Application?> GetByIdAsync(int id);
        Task<IEnumerable<SPCS.Files.Models.Application>> GetAllAsync();
    }
}
