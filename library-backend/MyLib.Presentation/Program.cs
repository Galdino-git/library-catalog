using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MyLib.Application.Handlers.Books.Commands.RegisterBook;
using MyLib.Application.Mappers;
using MyLib.Application.Validation;
using MyLib.Domain.IRepositories;
using MyLib.Infrastructure.Data;
using MyLib.Infrastructure.Repositories;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add Controllers
builder.Services.AddControllers();

// Add DbContext
builder.Services.AddDbContext<BookCatalogDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookCatalogDb"));
});

// Add UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Add FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<RegisterBookCommandValidator>();

// Add AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile(new BookMappingProfile());
    cfg.AddProfile(new UserMappingProfile());
});

// Add MediatR (registers ALL handlers from Application assembly)
builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(typeof(RegisterBookCommandHandler).Assembly));

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "MyLib API",
        Version = "v1",
        Description = "API for MyLib Book Catalog",
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => 
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyLib API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
