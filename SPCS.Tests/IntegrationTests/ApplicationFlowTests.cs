using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using SPCS.Data;
using SPCS.Infra.Repositories;
using SPCS.Application.Files.Abstractions;
using SPCS.Application.Applications.Commands;
using SPCS.Application.Files.Commands;
using Microsoft.Extensions.Logging.Abstractions;
using SPCS.Files.Models;
using SPCS.Files.Enums;
using System.Linq;
using SPCS.Application.Files.Queries;
using SPCS.Application.Files.Queries.Handlers;
using SPCS.Application.Applications.Queries;
using SPCS.Application.Applications.Queries.Handlers;
using SPCS.Application.Concurrency.Commands;
using SPCS.Application.Concurrency.DomainServices;
using SPCS.Application.Concurrency.Abstractions;
using System.IO;
using SPCS.Common.Models;
using SPCS.Common.Enums;

namespace SPCS.Tests.IntegrationTests
{
    public class ApplicationFlowTests
    {
        private SPCSContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<SPCSContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;
            return new SPCSContext(options);
        }

        [Fact]
        public async Task FullFlow_CreateApp_Upload_Parse_Calculate()
        {
            using var context = CreateContext();
            var appRepo = new ApplicationRepository(context);
            var fileRepo = new FileRepository(context);

            // Seed configuration
            var fileGeneralPathConfig = new SPCS.Common.Models.SPCSConfiguration
            {
                Name = "fileGeneralPath",
                Value = Path.Combine(Path.GetTempPath(), "spcs_test_files/"),
                Type = SPCSConfigType.Path
            };
            context.Configuration.Add(fileGeneralPathConfig);
            await context.SaveChangesAsync();

            // Create application
            var createAppHandler = new CreateApplicationCommandHandler(appRepo);
            var app = await createAppHandler.Handle(new CreateApplicationCommand { Name = "test" }, default);

            // Upload two files: production and consumption
            var uploadHandler = new UploadFile(fileRepo);

            var prodContent = System.Text.Encoding.UTF8.GetBytes("date;energy;ignore\n01/01/2025 00:00;1,0;x\n01/01/2025 00:15;2,0;x\n");
            var consContent = System.Text.Encoding.UTF8.GetBytes("date;energy;ignore\n01/01/2025 00:00;0,5;x\n01/01/2025 00:15;0,5;x\n");

            var prod = await uploadHandler.Handle(new FileUploadCommand { FileName = "prod.csv", Content = prodContent, ContentType = "text/csv", FileType = FileType.Production, ApplicationId = app.Id }, default);
            var cons = await uploadHandler.Handle(new FileUploadCommand { FileName = "cons.csv", Content = consContent, ContentType = "text/csv", FileType = FileType.Consumtion, ApplicationId = app.Id }, default);

            // Parse production
            var parseHandler = new ParseFileCommandHandler(fileRepo, NullLogger<ParseFileCommandHandler>.Instance);
            var prodFile = context.Files.First(f => f.Name == "prod.csv");
            var consFile = context.Files.First(f => f.Name == "cons.csv");

            var parseProdResult = await parseHandler.Handle(new ParseFileCommand { FileId = prodFile.Id }, default);
            var parseConsResult = await parseHandler.Handle(new ParseFileCommand { FileId = consFile.Id }, default);

            Assert.True(parseProdResult);
            Assert.True(parseConsResult);

            // Calculate using CreateConcurrencyCalculationCommand (now uses parsed timestamps)
            var concurrencyHandler = new SPCS.Application.Concurrency.Commands.CreateConcurrencyCalculationCommandHandler(
                new ConcurrencyCalculator(),
                new SPCS.Infra.Repositories.ConcurrencyCalculationRepository(context),
                fileRepo
            );

            var concurrencyCommand = new SPCS.Application.Concurrency.Commands.CreateConcurrencyCalculationCommand
            {
                ApplicationId = app.Id,
                BatteryInitialState = 50,
                BatteryLowestThreshold = 20,
                BatteryHighestThreshold = 90,
                BatteryCapacity = 100,
                BatteryChargingRate = 5,
                BatteryDischargingRate = 5
            };

            var calc = await concurrencyHandler.Handle(concurrencyCommand, default);

            Assert.NotNull(calc);
            Assert.True(calc.BatteryHistory.Any());
        }
    }
}
