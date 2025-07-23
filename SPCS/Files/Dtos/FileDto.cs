using SPCS.Files.Enums;

namespace SPCS.Files.Dtos
{
    public record FileDto
    {
        public string Name { get; set; } = default!;
        public FileType Type;
        public string ContentType { get; set; } = default!;
    }
}
