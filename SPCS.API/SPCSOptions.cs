using System.Text.Json;
using System.Text.Json.Serialization;

namespace SPCS.API
{
    public static class SPCSOptions
    {
        public static JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            WriteIndented = true
        };
    }
}
