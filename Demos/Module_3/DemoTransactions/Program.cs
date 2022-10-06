using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Transactions;

namespace DemoTransactions;

internal class Program
{
    public static string connectionString = @"Server=.\SQLEXPRESS;Database=ShopDatabase;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False";
    public static string backupConnectionString = @"Server=.\SQLEXPRESS;Database=ShopDatabaseBackup;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False";
    
    static void Main(string[] args)
    {
        //LocalTransactions();
        //MultiContexts();
        //MixedLocalTransactions();
       //DistributedTransactions();
        //SavePoints();
        Isolations();
    }

    private static void LocalTransactions()
    {
        // By default SaveChanges wraps all changes in a transaction
        var optionsBuilder = new DbContextOptionsBuilder<ProductContext>();
        optionsBuilder.UseSqlServer(connectionString);
        var options = optionsBuilder.Options;
        var context = new ProductContext(options);
        
        var brand1 = new Brand { Name = "Brand 1", Website = "https://www.brand_1.nl" };
        var brand2 = new Brand { Name = "Brand 2", Website = "https://www.brand_2.nl" };//, Id=1 };

        using (var tran = context.Database.BeginTransaction())
        {
            context.Brands.Add(brand1);
            context.SaveChanges();
            context.Brands.Add(brand2);
            context.SaveChanges();

            tran.Commit(); // By not calling Commit() everything will be rolled back (Dispose())
        }

        // Clean up
        Console.WriteLine("Press enter to clean up");
        Console.ReadLine();
        context.RemoveRange(brand1);
        context.SaveChanges();
        context.RemoveRange(brand2);
        context.SaveChanges();
    }
    private static void MultiContexts()
    {
        // Every Context has it's own connection share the connection!
        var connection = new SqlConnection(connectionString);
        var optionsBuilder = new DbContextOptionsBuilder<ProductContext>();
        optionsBuilder.UseSqlServer(connection); // !!!!
        var options = optionsBuilder.Options;
        var context = new ProductContext(options);
        var context2 = new ProductContext(options);

        var brand1 = new Brand { Name = "Brand 1", Website = "https://www.brand_1.nl" };
        var brand2 = new Brand { Name = "Brand 2", Website = "https://www.brand_2.nl" };//, Id=1 };

        using (var tran = context.Database.BeginTransaction())
        {
            context.Brands.Add(brand1);
            context.SaveChanges();

            context2.Database.UseTransaction(tran.GetDbTransaction());
            context2.Brands.Add(brand2);
            context2.SaveChanges();
            tran.Commit(); 
        }

        // Clean up
        Console.WriteLine("Press enter to clean up");
        Console.ReadLine();
        context.RemoveRange(brand1);
        context.SaveChanges();
        context.RemoveRange(brand2);
        context.SaveChanges();
    }
    private static void MixedLocalTransactions()
    {
        var connection = new SqlConnection(connectionString);
        connection.Open();
        var optionsBuilder = new DbContextOptionsBuilder();
        optionsBuilder.UseSqlServer(connection); // !!!
        var context = new ProductContext(optionsBuilder.Options);

        var brand1 = new Brand { Name = "Brand 1", Website = "https://www.brand_1.nl" };
        var brand2 = new Brand { Name = "Brand 2", Website = "https://www.brand_2.nl" };

        using (var tran = connection.BeginTransaction())
        {
            var command = new SqlCommand();
            command.Connection = connection;
            command.Transaction = tran;
            command.CommandText = "INSERT INTO Core.Brands (Name, WebSite) VALUES (@name, @web)";
            command.Parameters.AddWithValue("name", brand1.Name);
            command.Parameters.AddWithValue("web", brand1.Website);
            command.ExecuteNonQuery();

            context.Database.UseTransaction(tran);
            {
                context.Brands.Add(brand2);
                context.SaveChanges();
            }

            tran.Commit();
        }
        // Clean up
        Console.WriteLine("Press enter to clean up");
        Console.ReadLine();
        brand1 = context.Brands.First(b => b.Name == "Brand 1");
        context.RemoveRange(brand1);
        context.SaveChanges();
        context.RemoveRange(brand2);
        context.SaveChanges();
    }
    private static void DistributedTransactions()
    {
        // .NET Core doesn't support Distributed Transactions because it would require
        // a different transaction manager on each platform.
        // Distributed Transactions will be supported in EF 7 and .NET 7 but only for the Windows platform

        var optionsBuilder = new DbContextOptionsBuilder();
        optionsBuilder.UseSqlServer(connectionString);
        var context = new ProductContext(optionsBuilder.Options);

        var optionsBuilder2 = new DbContextOptionsBuilder();
        optionsBuilder2.UseSqlServer(backupConnectionString);
        var context2 = new ProductHistoryContext(optionsBuilder2.Options);
        context2.Database.EnsureCreated();

        var brand1 = new Brand { Name = "Brand 1", Website = "https://www.brand_1.nl" };
        var brand2 = new Brand { Name = "Brand 2", Website = "https://www.brand_2.nl" };

        using (var transaction = new TransactionScope(
                                                        TransactionScopeOption.Required,
                                                        new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
        {
            context.Brands.Add(brand1);
            context.Brands.Add(brand2);
            context.SaveChanges();

            context2.Brands.Add(brand1);
            context2.Brands.Add(brand2);
            context2.SaveChanges();

            transaction.Complete();
        }

        // Clean up
        Console.WriteLine("Press enter to clean up");
        Console.ReadLine();
        context.RemoveRange(brand1);
        context.SaveChanges();
        context.RemoveRange(brand2);
        context.SaveChanges();
        context2.Database.EnsureDeleted();
    }
    private static void SavePoints()
    {
        // Savepoints are incompatible with SQL Server's Multiple Active Result Sets, and are not used.
        // If an error occurs during SaveChanges, the transaction may be left in an unknown state.
        string conString = @"Server=.\SQLEXPRESS;Database=ShopDatabase;Trusted_Connection=True;";
        var optionsBuilder = new DbContextOptionsBuilder();
        optionsBuilder.UseSqlServer(conString);
        var context = new ProductContext(optionsBuilder.Options);

        var brand1 = new Brand { Name = "Brand 1", Website = "https://www.brand_1.nl" };
        var brand2 = new Brand { Name = "Brand 2", Website = "https://www.brand_2.nl", Id=1 };

        using var tran = context.Database.BeginTransaction();
        try
        {
            context.Brands.Add(brand1);
            context.SaveChanges();
            tran.CreateSavepoint("After_Brand_1");
            context.Brands.Add(brand2);
            context.SaveChanges();

            tran.Commit(); // By not calling Commit() everything will be rolled back (Dispose())
        }
        catch(Exception)
        {
            tran.RollbackToSavepoint("After_Brand_1");
            // Fix problem entity
            context.Remove(brand2);
            context.SaveChanges();
            tran.Commit();
        }

        // Clean up
        Console.WriteLine("Press enter to clean up");
        Console.ReadLine();
        context.RemoveRange(brand1);
        context.SaveChanges();
    }
    private static void Isolations()
    {
        // There might be 3 problems
        // 1) Dirty Reads. Reading uncommited data
        // 2) Non-repeatable reads. Same query returns different results
        // 3) Phantom Reads. New rows are added or removed by another transaction to the records being read.
        //DirtyReads();
        //NonRepeatableReads();
        PhantomReads();

        Console.ReadLine();
    }
    private static void DirtyReads()
    {
        var optionsBuilder = new DbContextOptionsBuilder();
        optionsBuilder.UseSqlServer(connectionString);
        var context1 = new ProductContext(optionsBuilder.Options);
        var context2 = new ProductContext(optionsBuilder.Options);

        // Set the isolation level
        var isoLevel = System.Data.IsolationLevel.ReadCommitted;

        using var readTransaction = context1.Database.BeginTransaction(isoLevel);
        using var writeTransaction = context2.Database.BeginTransaction();

        var writeAction = new Task(() => {
            var brand = new Brand { Name = "Brand 1", Website = "https://www.brand_2.nl" };
            context2.Brands.Add(brand);
            context2.SaveChanges();

            Console.WriteLine("Waiting 1 seconds for commit");
            Task.Delay(1000).Wait();

            //writeTransaction.Commit();
        });

       
        var readData = Task.Run(() => {
            var query = context1.Brands;

            foreach (var b in query)
            {
                Console.WriteLine(b.Name);
            }
            
            writeAction.Start();
            Task.Delay(500).Wait();
            
            Console.WriteLine("==== Read again");
            foreach (var b in query)
            {
                Console.WriteLine(b.Name);
            }
            readTransaction.Commit();
        });

        Task.Delay(30000).Wait();
        var brand = context1.Brands.First(b => b.Name == "Brand 1");
        context1.Remove(brand);
        context1.SaveChanges();
    }
    private static void NonRepeatableReads()
    {
        var optionsBuilder = new DbContextOptionsBuilder();
        optionsBuilder.UseSqlServer(connectionString);
        var context1 = new ProductContext(optionsBuilder.Options);
        var brand = new Brand { Name = "Brand 1", Website = "https://www.brand_1.nl" };
        
        var context2 = new ProductContext(optionsBuilder.Options);
        context2.Brands.Add(brand);
        context2.SaveChanges();

        var modifyData = new Task(() => {
            using var writeTransaction = context2.Database.BeginTransaction();
            brand.Name = "Brand 2";
            context2.SaveChanges();
            writeTransaction.Commit();
        });

        var readData = Task.Run(() => {
            var isoLevel = System.Data.IsolationLevel.ReadCommitted;
            using var readTransaction = context1.Database.BeginTransaction(isoLevel);
            var query = context1.Brands.AsNoTracking();

            foreach (var b in query)
            {
                Console.WriteLine(b.Name);
            }
            modifyData.Start();

            Task.Delay(1000).Wait();
            Console.WriteLine("==== Read again");
            foreach (var b in query)
            {
                Console.WriteLine(b.Name);
            }
            readTransaction.Commit();
        });

        Task.Delay(5000).Wait();
        context2.Remove(brand);
        context2.SaveChanges();
    }
    private static void PhantomReads()
    {
        var optionsBuilder = new DbContextOptionsBuilder();
        optionsBuilder.UseSqlServer(connectionString);
        var context1 = new ProductContext(optionsBuilder.Options);
        var context2 = new ProductContext(optionsBuilder.Options);
        
        var insertData = new Task(() => {
            using var writeTransaction = context2.Database.BeginTransaction();
            var brand = new Brand { Name = "Brand 1", Website = "https://www.brand_1.nl" };
            context2.Brands.Add(brand);
            context2.SaveChanges();
            writeTransaction.Commit();

            Task.Delay(5000).Wait();
            context2.Remove(brand);
            context2.SaveChanges();
        });

        var readData = Task.Run(() => {
            var isoLevel = System.Data.IsolationLevel.Snapshot;
            using var readTransaction = context1.Database.BeginTransaction(isoLevel);
            var query = context1.Brands.Where(b=>b.Id > 2).AsNoTracking();

            foreach (var b in query)
            {
                Console.WriteLine(b.Name);
            }
            insertData.Start();

            Task.Delay(1000).Wait();
            Console.WriteLine("==== Read again");
            foreach (var b in query)
            {
                Console.WriteLine(b.Name);
            }
            readTransaction.Commit();
        });
    }
}