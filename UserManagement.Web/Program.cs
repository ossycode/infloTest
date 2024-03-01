using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Westwind.AspNetCore.Markdown;
using Serilog;
using System;

var builder = WebApplication.CreateBuilder(args);

// Serilog
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration).ReadFrom.Services(services);
});

// Add services to the container.
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
    .AddMarkdown()
    .AddControllersWithViews();

//builder.Services.AddHttpLogging(options =>
//{
//    options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponseHeaders;
//});

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSerilogRequestLogging();
//app.UseHttpLogging();
app.UseMarkdown();

app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();
