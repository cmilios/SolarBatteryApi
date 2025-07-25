using Microsoft.EntityFrameworkCore;
using Serilog;
using SPCS.API.Middleware;
using SPCS.Application;
using SPCS.Application.Concurrency.Abstractions;
using SPCS.Application.Concurrency.DomainServices;
using SPCS.Application.Files.Abstractions;
using SPCS.Data;
using SPCS.Infra.Repositories;


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<SPCSContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IConcurrencyCalculationRepository, ConcurrencyCalculationRepository>();
builder.Services.AddScoped<IFileRepository, FileRepository>();

// Add services to the container.
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<AssemblyReference>();
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IConcurrencyCalculator, ConcurrencyCalculator>();


builder.Host.UseSerilog();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SPCSContext>();
    dbContext.Database.Migrate();
    dbContext.Database.ExecuteSqlRaw(@"
    INSERT INTO Configuration (Name,Value,Type)
    VALUES ('fileGeneralPath', 'C:\files\', 1);
");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
