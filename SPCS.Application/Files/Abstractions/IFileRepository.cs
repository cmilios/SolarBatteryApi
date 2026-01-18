using SPCS.Common.Models;
using File = SPCS.Files.Models.File;

namespace SPCS.Application.Files.Abstractions
{
    public interface IFileRepository
    {
        Task AddAsync(File file);
        Task<SPCSConfiguration?> GetByName(string name);
        Task<File?> GetByIdAsync(int id);
        Task<IEnumerable<File>> GetAllAsync();
        Task AddParsedTimestampsAsync(IEnumerable<SPCS.Files.Models.ParsedTimestamp> timestamps);
        Task<IEnumerable<SPCS.Files.Models.ParsedTimestamp>> GetParsedTimestampsByFileIdAsync(int fileId);
        Task<IEnumerable<SPCS.Files.Models.ParsedTimestamp>> GetParsedTimestampsByApplicationIdAsync(int applicationId);
    }
}
