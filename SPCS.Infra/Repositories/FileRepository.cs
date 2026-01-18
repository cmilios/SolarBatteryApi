using Microsoft.EntityFrameworkCore;
using SPCS.Application.Files.Abstractions;
using SPCS.Common.Models;
using SPCS.Data;
using File = SPCS.Files.Models.File;

namespace SPCS.Infra.Repositories
{

    public class FileRepository(SPCSContext context) : IFileRepository
    {
        private readonly SPCSContext _context = context;

        public async Task AddAsync(File file)
        {
            _context.Files.Add(file);
            await _context.SaveChangesAsync();
        }

        public async Task<SPCSConfiguration?> GetByName(string name)
        {
            return await _context.Configuration.FirstOrDefaultAsync(x => x.Name == name);

        }

        public async Task<File?> GetByIdAsync(int id)
        {
            return await _context.Files.FindAsync(id);
        }

        public async Task<IEnumerable<File>> GetAllAsync()
        {
            return await _context.Files.ToListAsync();
        }

        public async Task AddParsedTimestampsAsync(IEnumerable<SPCS.Files.Models.ParsedTimestamp> timestamps)
        {
            _context.AddRange(timestamps);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<SPCS.Files.Models.ParsedTimestamp>> GetParsedTimestampsByFileIdAsync(int fileId)
        {
            return await _context.Set<SPCS.Files.Models.ParsedTimestamp>().Where(x => x.FileId == fileId).ToListAsync();
        }

        public async Task<IEnumerable<SPCS.Files.Models.ParsedTimestamp>> GetParsedTimestampsByApplicationIdAsync(int applicationId)
        {
            var stamps = await _context.Set<SPCS.Files.Models.ParsedTimestamp>()
                .Include(p => p.File).ToListAsync();

            return stamps
                .Where(p => p.File.ApplicationId == applicationId);
        }
    }
}
