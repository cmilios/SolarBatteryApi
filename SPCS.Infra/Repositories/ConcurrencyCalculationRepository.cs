using Microsoft.EntityFrameworkCore;
using SPCS.Application.Concurrency.Abstractions;
using SPCS.Concurrency.Models;
using SPCS.Data;

namespace SPCS.Infra.Repositories
{
    public class ConcurrencyCalculationRepository : IConcurrencyCalculationRepository
    {
        private readonly SPCSContext _context;

        public ConcurrencyCalculationRepository(SPCSContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ConcurrencyCalculation concurrencyCalculation)
        {
            _context.ConcurrencyCalculations.Add(concurrencyCalculation);
            await _context.SaveChangesAsync();
        }

        public async Task<ConcurrencyCalculation?> GetByIdAsync(int id)
        {
            return await _context.ConcurrencyCalculations
                .Include(x => x.PowerTimestamps)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
