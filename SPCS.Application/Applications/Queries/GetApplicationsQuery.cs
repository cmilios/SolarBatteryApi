using MediatR;

namespace SPCS.Application.Applications.Queries
{
    public class GetApplicationsQuery : IRequest<IEnumerable<SPCS.Files.Models.Application>> { }

    public class GetApplicationByIdQuery : IRequest<SPCS.Files.Models.Application?>
    {
        public int Id { get; init; }
    }
}
