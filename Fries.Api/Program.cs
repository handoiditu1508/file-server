using Fries.Api.Attributes;
using Fries.Api.Middlewares;
using Fries.Helpers;
using Fries.Helpers.Abstractions;
using Fries.Services.Abstractions.FilesUpload;
using Fries.Services.Abstractions.LoggingService;
using Fries.Services.FilesStorage;
using Fries.Services.LoggingService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Reflection;

var appCors = "AppCors";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(appCors,
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

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
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme()
    {
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Name = AppSettings.ApiKey.Name
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "ApiKey" }
            },
            new string[] { }
        }
    });

    // Add support for swagger endpoint description
    // https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddTransient<IHttpHelper, HttpHelper>();
builder.Services.AddSingleton<IFilesStorageService, FilesStorageService>();
builder.Services.AddSingleton<ILoggingService, LoggingService>();
builder.Services.AddSafeListActionFilter();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(appCors);

app.UseAuthorization();

app.UseMiddleware<SafeListMiddleware>(AppSettings.SafeList);

app.UseMiddleware<ApiKeyMiddleware>();

app.MapControllers();

app.Run();
