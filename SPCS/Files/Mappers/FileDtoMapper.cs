using SPCS.Files.Dtos;
using File = SPCS.Files.Models.File;

namespace SPCS.Files.Mappers
{
    public static class FileDtoMapper
    {

        public static FileDto Map(File source)
        {
            return new FileDto
            {
                Id = source.Id,
                Name = source.Name,
                ContentType = source.ContentType,
                Type = source.Type,
            };
        }
    }
}
