using SPCS.Concurrency.Models;

namespace SPCS.Application.Concurrency.Abstractions
{
    public interface IConcurrencyCalculationRepository
    {
        Task<ConcurrencyCalculation?> GetByIdAsync(int id);
        Task AddAsync(ConcurrencyCalculation concurrencyCalculation);
    }
}
