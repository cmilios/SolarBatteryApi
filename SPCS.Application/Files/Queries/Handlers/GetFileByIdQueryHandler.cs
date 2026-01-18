using MediatR;
using SPCS.Application.Files.Abstractions;
using SPCS.Files.Dtos;
using SPCS.Files.Mappers;

namespace SPCS.Application.Files.Queries.Handlers
{
    public class GetFileByIdQueryHandler(IFileRepository fileRepository) : IRequestHandler<GetFileByIdQuery, FileDto?>
    {
        private readonly IFileRepository _fileRepository = fileRepository;

        public async Task<FileDto?> Handle(GetFileByIdQuery request, CancellationToken cancellationToken)
        {
            var file = await _fileRepository.GetByIdAsync(request.Id);
            if (file == null) return null;
            return FileDtoMapper.Map(file);
        }
    }
}
