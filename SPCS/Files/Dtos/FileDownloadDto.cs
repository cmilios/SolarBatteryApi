using SPCS.Files.Enums;

namespace SPCS.Files.Dtos
{
    public record FileDownloadDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = default!;
        public FileType Type { get; init; }
        public string ContentType { get; init; } = default!;
        public byte[] Content { get; init; } = default!;
    }
}
