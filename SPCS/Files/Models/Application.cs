using System.Collections.Generic;

namespace SPCS.Files.Models
{
    public class Application
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public List<File> Files { get; set; } = new();
    }
}
