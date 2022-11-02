//using Serilog;

using MagicVillaApi.Data;
using MagicVillaApi.Logging;
using MagicVillaApi.Mapping;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddAutoMapper(typeof(MappingConfig));
//builder.Services.AddScoped<ILogging,LogginV2>();
//Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.File("log/villaLogs.txt",rollingInterval:RollingInterval.Day).CreateLogger();

//builder.Host.UseSerilog();  

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
