using MediatR;
using SPCS.Application.Files.Abstractions;
using SPCS.Files.Dtos;

namespace SPCS.Application.Files.Queries.Handlers
{
    public class DownloadFileQueryHandler(IFileRepository fileRepository) : IRequestHandler<DownloadFileQuery, FileDownloadDto?>
    {
        private readonly IFileRepository _fileRepository = fileRepository;

        public async Task<FileDownloadDto?> Handle(DownloadFileQuery request, CancellationToken cancellationToken)
        {
            var file = await _fileRepository.GetByIdAsync(request.Id);
            if (file == null) return null;
            if (string.IsNullOrEmpty(file.Path) || !System.IO.File.Exists(file.Path)) return null;

            var content = await System.IO.File.ReadAllBytesAsync(file.Path, cancellationToken);

            return new FileDownloadDto
            {
                Id = file.Id,
                Name = file.Name,
                Type = file.Type,
                ContentType = file.ContentType,
                Content = content
            };
        }
    }
}
