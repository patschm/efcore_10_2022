using ACME.DataLayer.Interfaces;
using ACME.DataLayer.Repository.SqlServer;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ACME.Backend.ShopApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        IConfiguration config = builder.Configuration;
        // Add services to the container.
        var connectionString = config.GetConnectionString("SqlServerExpress");
        // TODO 8: Register the ShopDatabaseContext in the dependency injector

        // TODO 9: Register the UnitOfWork class in the dependency injector      

        builder.Services.AddControllers().AddNewtonsoftJson();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}