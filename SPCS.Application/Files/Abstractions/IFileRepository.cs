using SPCS.Common.Models;
using File = SPCS.Files.Models.File;


namespace SPCS.Application.Files.Abstractions
{
    public interface IFileRepository
    {
        Task AddAsync(File file);
        Task<SPCSConfiguration?> GetByName(string name);

    }
}