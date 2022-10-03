using ACME.DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace ACME.Frontend.ConsoleClient;

internal class Program
{
    const string databaseName = "Shop";
    const string connectionString = @$"Server=.\SQLEXPRESS;Database={databaseName};Trusted_Connection=True;MultipleActiveResultSets=true;";
    
    static void Main()
    {
        // TODO 3: Initialize the ShopDatabaseContext and create the database.
        // Run the application and check if the database was created according to
        // the specifications
     
    }
}