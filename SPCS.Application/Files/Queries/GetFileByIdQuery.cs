using MediatR;
using SPCS.Files.Dtos;

namespace SPCS.Application.Files.Queries
{
    public class GetFileByIdQuery : IRequest<FileDto?>
    {
        public int Id { get; init; }
    }
}
