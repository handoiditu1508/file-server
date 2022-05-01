using Fries.Api.Middlewares;
using Fries.Helpers;
using Fries.Helpers.Abstractions;
using Fries.Services.Abstractions.FilesUpload;
using Fries.Services.Abstractions.LoggingService;
using Fries.Services.FilesStorage;
using Fries.Services.LoggingService;

var builder = WebApplication.CreateBuilder(args);

// Config ILogger
builder.Logging.ClearProviders();
builder.Logging.AddProvider(new CustomLoggerProvider());

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IHttpHelper, HttpHelper>();
builder.Services.AddSingleton<IFilesStorageService, FilesStorageService>();
builder.Services.AddSingleton<ILoggingService, LoggingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ApiKeyMiddleware>();

app.MapControllers();

app.Run();
