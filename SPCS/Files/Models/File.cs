using SPCS.Files.Enums;

namespace SPCS.Files.Models
{
    public record File
    {
        public string Name { get; set; } = default!;
        public string Path { get; set; } = default!;
        public FileType Type { get; set; } = default!;
        public string ContentType { get; set; } = default!;
    }
}
