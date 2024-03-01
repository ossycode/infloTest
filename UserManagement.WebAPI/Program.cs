using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration).ReadFrom.Services(services);
});

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services
    .AddDataAccess(options =>
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        if (connectionString != null)
        {
            options.UseSqlServer(connectionString);
        }

    })
    .AddDomainServices()
    .AddControllers(options =>
    {
        options.Filters.Add(new ProducesAttribute("application/json"));
        options.Filters.Add(new ConsumesAttribute("application/json"));
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => { options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "api.xml")); });

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors("AllowAll");
app.UseHsts();
app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();
