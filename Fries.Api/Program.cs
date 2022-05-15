using Fries.Api.Middlewares;
using Fries.Helpers;
using Fries.Helpers.Abstractions;
using Fries.Services.Abstractions.FilesUpload;
using Fries.Services.Abstractions.LoggingService;
using Fries.Services.FilesStorage;
using Fries.Services.LoggingService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Config ILogger
builder.Logging.ClearProviders();
builder.Logging.AddProvider(new CustomLoggerProvider());

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// Add api key authentication for swagger
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme()
    {
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Name = AppSettings.ApiKey.Name
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "ApiKey" }
            },
            new string[] { }
        }
    });
});

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
