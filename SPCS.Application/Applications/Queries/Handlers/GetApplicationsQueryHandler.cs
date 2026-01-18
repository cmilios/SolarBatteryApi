using MediatR;
using SPCS.Application.Files.Abstractions;

namespace SPCS.Application.Applications.Queries.Handlers
{
    public class GetApplicationsQueryHandler(IApplicationRepository repository) : IRequestHandler<GetApplicationsQuery, IEnumerable<SPCS.Files.Models.Application>>
    {
        private readonly IApplicationRepository _repository = repository;

        public async Task<IEnumerable<SPCS.Files.Models.Application>> Handle(GetApplicationsQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync();
        }
    }

    public class GetApplicationByIdQueryHandler(IApplicationRepository repository) : IRequestHandler<GetApplicationByIdQuery, SPCS.Files.Models.Application?>
    {
        private readonly IApplicationRepository _repository = repository;

        public async Task<SPCS.Files.Models.Application?> Handle(GetApplicationByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.Id);
        }
    }
}
