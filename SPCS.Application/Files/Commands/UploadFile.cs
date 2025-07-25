using MediatR;
using SPCS.Application.Files.Abstractions;
using SPCS.Files.Dtos;
using SPCS.Files.Enums;
using SPCS.Files.Mappers;
using File = SPCS.Files.Models.File;
using FileIO = System.IO.File;

namespace SPCS.Application.Files.Commands
{
    public class FileUploadCommand : IRequest<FileDto?>
    {
        public required string FileName { get; init; }
        public required byte[] Content { get; init; }
        public required string ContentType { get; init; }
        public FileType FileType { get; init; }
    }

    public class UploadFile(IFileRepository fileRepository) : IRequestHandler<FileUploadCommand, FileDto?>
    {
        private readonly IFileRepository _fileRepository = fileRepository;
        public async Task<FileDto?> Handle(FileUploadCommand request, CancellationToken cancellationToken)
        {
            var fileGeneralPath = await _fileRepository.GetByName("fileGeneralPath");
            if (fileGeneralPath == null)
            {
                throw new ArgumentNullException(nameof(fileGeneralPath), "Configuration Missing: FileGeneralPath not found");
            }
            var file = new File
            {
                Name = request.FileName,
                ContentType = request.ContentType,
                Path = fileGeneralPath?.Value + request.FileName,
                Type = request.FileType,
            };
            if (file.Path == null)
            {
                throw new ArgumentNullException(nameof(request.FileName), "File path cannot be null");
            }

            if (!Directory.Exists(fileGeneralPath?.Value))
            {
                Directory.CreateDirectory(fileGeneralPath?.Value!);
            }
            await FileIO.WriteAllBytesAsync(file.Path, request.Content, cancellationToken);

            await _fileRepository.AddAsync(file);

            return FileDtoMapper.Map(file);
        }
    }
}
