using Microsoft.EntityFrameworkCore;
using SPCS.Application.Files.Abstractions;
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

        public async Task<List<string?>?> GetFileGeneralPath()
        {
            return await _context.GeneralFilePath.FirstOrDefaultAsync();

        }
    }
}
