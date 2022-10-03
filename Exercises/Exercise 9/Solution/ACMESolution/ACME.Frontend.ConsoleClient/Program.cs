using ACME.DataLayer.Entities;
using ACME.DataLayer.Repository.SqlServer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using IsolationLevel = System.Data.IsolationLevel;

namespace ACME.Frontend.ConsoleClient;

internal class Program
{
    const string databaseName = "ProductCatalog";
    const string connectionString = @$"Server=.\SQLEXPRESS;Database={databaseName};Trusted_Connection=True;MultipleActiveResultSets=true;";

    static void Main()
    {
        Exercise1();
        //Exercise2();
    }
    private static void Exercise1()
    {
        var connection = new SqlConnection(connectionString);
        var bld = new DbContextOptionsBuilder<ShopDatabaseContext>();
        bld.UseSqlServer(connection);
        var ctx = new ShopDatabaseContext(bld.Options);

        var bld2 = new DbContextOptionsBuilder<PriceHistoryContext>();
        bld2.UseSqlServer(connection);
        var historyCtx = new PriceHistoryContext(bld2.Options);

        var price = new Price
        {
            BasePrice = 100,
            ShopName = "ACME",
            PriceDate = DateTime.Now,
            ProductId = 1
        };

        // TODO 1: Make sure that both transactions succeed.
        // If one fails all must fail
        var transaction = ctx.Database.BeginTransaction();
        try
        {
            ctx.Prices.Add(price);
            ctx.SaveChanges();

            historyCtx.Database.UseTransaction(transaction.GetDbTransaction());
            historyCtx.Prices.Add(price);
            historyCtx.SaveChanges();

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
        }
    }
    private static void Exercise2()
    {
        var bld = new DbContextOptionsBuilder<ShopDatabaseContext>();
        bld.UseSqlServer(connectionString);
        var ctx = new ShopDatabaseContext(bld.Options);
        var remote = new CancellationTokenSource();
        remote.CancelAfter(6000);

        var rnd = new Random();
        Task.Run(() =>
        {
            do
            {
                var ctx2 = new ShopDatabaseContext(bld.Options);
                var price = ctx2.Prices.First(p => p.Id == rnd.NextInt64(1, 10));
                price.BasePrice = rnd.NextDouble() * 4000;
                ctx2.SaveChanges();
                Task.Delay(100).Wait();
                if (remote.Token.IsCancellationRequested) return;
            }
            while (true);
        });

        // TODO 2: Run the program and observe that the average prices 
        // changes all the time.
        // TODO 3: Make sure the program reads the same average all the time
        var prices = ctx.Prices.Where(p => p.ProductId < 10);
        using (ctx.Database.BeginTransaction(IsolationLevel.RepeatableRead))
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine(prices.Average(p => p.BasePrice));
                Task.Delay(1000).Wait();
            }

    }
}