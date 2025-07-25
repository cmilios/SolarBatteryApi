using SPCS.Common.Dtos;
using SPCS.Common.Models;

namespace SPCS.Common.Mappers
{
    public static class SPCSConfigurationDtoMapper
    {
        public static SPCSConfifurationDto Map(SPCSConfiguration source)
        {
            return new SPCSConfifurationDto
            {
                Name = source.Name,
                Type = source.Type,
                Id = source.Id,
                Value = source.Value,
            };
        }
    }
}
