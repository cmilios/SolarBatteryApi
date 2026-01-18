using MediatR;
using SPCS.Application.Files.Abstractions;
using SPCS.Files.Dtos;
using SPCS.Files.Mappers;

namespace SPCS.Application.Files.Queries.Handlers
{
    public class GetFilesQueryHandler(IFileRepository fileRepository) : IRequestHandler<GetFilesQuery, IEnumerable<FileDto>>
    {
        private readonly IFileRepository _fileRepository = fileRepository;

        public async Task<IEnumerable<FileDto>> Handle(GetFilesQuery request, CancellationToken cancellationToken)
        {
            var files = await _fileRepository.GetAllAsync();
            return files.Select(FileDtoMapper.Map);
        }
    }
}
