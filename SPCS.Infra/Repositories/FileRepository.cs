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
    }
}
