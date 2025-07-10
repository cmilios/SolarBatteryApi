using Microsoft.EntityFrameworkCore;
using Serilog;
using SPCS.API.Middleware;
using SPCS.Application;
using SPCS.Application.Concurrency.Abstractions;
using SPCS.Application.Concurrency.DomainServices;
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

// Add services to the container.
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<AssemblyReference>();
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IConcurrencyCalculator, ConcurrencyCalculator>();

builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        //opt.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        opt.JsonSerializerOptions.WriteIndented = true;
    });
builder.Host.UseSerilog();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SPCSContext>();
    dbContext.Database.Migrate();
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
