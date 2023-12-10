using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using RsaEncryption.WebApi.Business;
using RsaEncryption.WebApi.Business.ApplicationService;
using RsaEncryption.WebApi.Entities;
using RsaEncryption.WebApi.Entities.Config;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyAllowedOrigins", policy => { policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
});

// Se crea la variable services para añadir los servicios que se adicionaban con IServiceCollection
var services = builder.Services.AddMemoryCache();

//Configuraciones de Swagger
var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1_0",
        Title = $"{Assembly.GetExecutingAssembly().GetName().Name}"
    });

    options.IncludeXmlComments(xmlPath);//Configuramos los comentarios en los controladores
});


services.AddSingleton<IEncryptionService, EncryptionService>();

services.Configure<AppConfig>(config.GetSection("AppConfig"));

services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
        .Where(e => !e.Key.StartsWith("request"))
            .ToDictionary(
                e => e.Key.Replace("$.", ""),
                e => e.Value.Errors.Select(x => x.ErrorMessage).ToArray()
            );

        var problemDetails = new DefaultResponse()
        {
            statusDescription = "Ocurrieron uno o más errores de validación.",
            status = false,
            data = errors
        };

        return new BadRequestObjectResult(problemDetails);
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(config =>
        config.SwaggerEndpoint("/swagger/v1/swagger.json", "API SWAGGER!"));
}

app.UseCors("MyAllowedOrigins");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
