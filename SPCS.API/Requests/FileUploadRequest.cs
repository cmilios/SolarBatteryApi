using System.ComponentModel.DataAnnotations;

namespace SPCS.API.Requests
{
    public class FileUploadRequest
    {
        [Required]
        public IFormFile File { get; set; } = default!;

        [Required]
        public int Type { get; set; }
    }
}
