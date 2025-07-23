using File = SPCS.Files.Models.File;


namespace SPCS.Application.Files.Abstractions
{
    public interface IFileRepository
    {
        Task AddAsync(File file);
        Task<List<string?>?> GetFileGeneralPath();

    }
}