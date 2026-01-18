using MediatR;
using Microsoft.Extensions.Logging;
using System.Globalization;
using SPCS.Application.Files.Abstractions;
using SPCS.Files.Models;

namespace SPCS.Application.Files.Commands
{
    public class ParseFileCommand : IRequest<bool>
    {
        public int FileId { get; init; }
    }

    public class ParseFileCommandHandler(IFileRepository fileRepository, ILogger<ParseFileCommandHandler> logger) : IRequestHandler<ParseFileCommand, bool>
    {
        private readonly IFileRepository _fileRepository = fileRepository;
        private readonly ILogger<ParseFileCommandHandler> _logger = logger;

        public async Task<bool> Handle(ParseFileCommand request, CancellationToken cancellationToken)
        {
            var file = await _fileRepository.GetByIdAsync(request.FileId);
            if (file == null)
            {
                _logger.LogWarning("File with id {FileId} not found", request.FileId);
                return false;
            }

            if (string.IsNullOrEmpty(file.Path) || !System.IO.File.Exists(file.Path))
            {
                _logger.LogWarning("File path not found for file id {FileId}", request.FileId);
                return false;
            }

            var parsed = new List<ParsedTimestamp>();

            using var stream = System.IO.File.OpenRead(file.Path);
            using var reader = new System.IO.StreamReader(stream);

            // Expect CSV with headers: date;energy;ignored
            var header = await reader.ReadLineAsync();
            if (header == null) return false;
            var culture = CultureInfo.GetCultureInfo("el-GR");
            var dateFormats = new[] { "dd/MM/yyyy HH:mm", "d/MM/yyyy HH:mm" };

            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split(';');
                if (parts.Length < 2) continue;

                var datePart = parts[0].Trim();
                if (string.IsNullOrEmpty(datePart)) continue;

                if (!DateTimeOffset.TryParseExact(datePart, dateFormats, culture, DateTimeStyles.AssumeLocal, out var date))
                {
                    // Try fallback parse
                    if (!DateTimeOffset.TryParse(datePart, culture, DateTimeStyles.AssumeLocal, out date))
                        continue;
                }

                decimal production = 0m;
                var energyPart = parts[1].Trim();
                if (!string.IsNullOrEmpty(energyPart))
                {
                    // Some files use comma as decimal separator (e.g. "0,065")
                    decimal.TryParse(energyPart, NumberStyles.Number, culture, out production);
                }

                // Map parsed value to production or consumption depending on file type
                if (file.Type == SPCS.Files.Enums.FileType.Consumtion)
                {
                    parsed.Add(new ParsedTimestamp
                    {
                        FileId = file.Id,
                        Date = date,
                        ProductionValue = 0m,
                        ConsumptionValue = production
                    });
                }
                else
                {
                    parsed.Add(new ParsedTimestamp
                    {
                        FileId = file.Id,
                        Date = date,
                        ProductionValue = production,
                        ConsumptionValue = 0m
                    });
                }
            }

            if (parsed.Any())
            {
                await _fileRepository.AddParsedTimestampsAsync(parsed);
                return true;
            }

            return false;
        }
    }
}
