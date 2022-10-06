using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Transactions;

namespace Transactions;

internal class Program
{
    public static string connectionString = @"Server=.\SQLEXPRESS;Database=ShopDatabase;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False";
    public static string backupConnectionString = @"Server=.\SQLEXPRESS;Database=ShopDatabaseBackup;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False";
    
    static void Main(string[] args)
    {
        //LocalTransactions();
        //MultipleContexts();
        MultipleSameContexts();
    }
    private static void MultipleSameContexts()
    {
        var connection = new SqlConnection(connectionString);
        connection.Open();
        var optionsBuilder = new DbContextOptionsBuilder<ProductContext>();
        optionsBuilder.UseSqlServer(connection);
        var context = new ProductContext(optionsBuilder.Options);

        var optionsBuilder2 = new DbContextOptionsBuilder<ProductContext>();
        optionsBuilder2.UseSqlServer(connection);
        var context2 = new ProductContext(optionsBuilder2.Options);

        using (var tran = connection.BeginTransaction())
        {
            context.Database.UseTransaction(tran);
            context.Brands.Add(new Brand { Name = "Merk 4" });
            context.SaveChanges();

            context2.Database.UseTransaction(tran);
            context2.Brands.Add(new Brand { Name = "Merk 5" });
            context2.SaveChanges();

            var cmd = new SqlCommand();
            cmd.CommandText = "";
            cmd.Connection = connection;
            cmd.Transaction = tran;
            var rdr = cmd.ExecuteReader();
            tran.Commit();
        }

        connection.Close();
    }

    private static void MultipleContexts()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ProductContext>();
        optionsBuilder.UseSqlServer(connectionString);
        var context = new ProductContext(optionsBuilder.Options);

        var optionsBuilder2 = new DbContextOptionsBuilder<ProductHistoryContext>();
        optionsBuilder2.UseSqlServer(backupConnectionString);
        var context2 = new ProductHistoryContext(optionsBuilder2.Options);

        //context.Database.EnsureCreated();
        //context2.Database.EnsureCreated();
        System.Console.WriteLine("Enter to continue");
        Console.ReadLine();

        using (var tran = new TransactionScope())
        {
            context.Brands.Add(new Brand { Name = "Merk 1" });
            context.SaveChanges();
            context2.Brands.Add(new Brand { Name = "Merk 1" });
            context2.SaveChanges();
            tran.Complete();
        }
    }

    private static void LocalTransactions()
    {
        // By default SaveChanges wraps all changes in a transaction
        var connection = new SqlConnection(connectionString);
        var optionsBuilder = new DbContextOptionsBuilder<ProductContext>();
        optionsBuilder.UseSqlServer(connection);
        var context = new ProductContext(optionsBuilder.Options);

        using (var tran = connection.BeginTransaction())
        {
            context.Brands.Add(new Brand { Name = "Merk 1" });
            context.SaveChanges();
            context.Brands.Add(new Brand { Name = "Merk 2" });
            context.SaveChanges();
            tran.Commit();
        }  
    }
}