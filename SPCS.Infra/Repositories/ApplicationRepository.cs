using SPCS.Application.Files.Abstractions;
using SPCS.Data;
using Microsoft.EntityFrameworkCore;

namespace SPCS.Infra.Repositories
{
    public class ApplicationRepository(SPCSContext context) : IApplicationRepository
    {
        private readonly SPCSContext _context = context;

        public async Task AddAsync(SPCS.Files.Models.Application application)
        {
            _context.Add(application);
            await _context.SaveChangesAsync();
        }

        public async Task<SPCS.Files.Models.Application?> GetByIdAsync(int id)
        {
            return await _context.Set<SPCS.Files.Models.Application>().Include(a => a.Files).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<SPCS.Files.Models.Application>> GetAllAsync()
        {
            return await _context.Set<SPCS.Files.Models.Application>().Include(a => a.Files).ToListAsync();
        }
    }
}
