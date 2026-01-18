using MediatR;
using SPCS.Files.Dtos;

namespace SPCS.Application.Files.Queries
{
    public class DownloadFileQuery : IRequest<FileDownloadDto?>
    {
        public int Id { get; init; }
    }
}
