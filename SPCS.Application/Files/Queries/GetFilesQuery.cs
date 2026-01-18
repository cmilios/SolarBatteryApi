using MediatR;
using SPCS.Files.Dtos;

namespace SPCS.Application.Files.Queries
{
    public class GetFilesQuery : IRequest<IEnumerable<FileDto>> { }

}
