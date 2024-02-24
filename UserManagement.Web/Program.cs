using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Westwind.AspNetCore.Markdown;

var builder = WebApplication.CreateBuilder(args);


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

var app = builder.Build();

app.UseMarkdown();

app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();
