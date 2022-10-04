
using Microsoft.Data.SqlClient;
using System.Data;

namespace Oertijd;

internal class Program
{
    const string conStr = @"Server=.\sqlexpress;Database=Mod1DB;Integrated Security=True;TrustServerCertificate=Yes;MultipleActiveResultSets=true;";
    private static string user;
    private static string pass;

    static void Main(string[] args)
    {
        string user = "Karel";
        string pass = "Geheim'; DROP Database Mod1DB;--";
        string s = "SELECT * FROM Users WHERE username = '" + user + "' AND pass = '" + pass + "';";
        Console.WriteLine(s);
        int zoekId = 25;
        SqlConnection connection = new SqlConnection(conStr);
        connection.Open(); 
        var tran = connection.BeginTransaction();
        Console.WriteLine(connection.State);
        SqlCommand cmd = connection.CreateCommand();
        cmd.Connection = connection;
        cmd.CommandText = "SELECT * FROM dbo.Brands WHERE Id > @id";
        cmd.Parameters.AddWithValue("id", zoekId);
        cmd.Transaction = tran;
        SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.Default); // Returns wrapper around TDS stream
        while (rdr.Read())
        {
            string name = rdr.GetString(1);
            int id = rdr.GetInt32(0);

            Console.WriteLine($"[{id}] {name}");
            SqlCommand cmd2 = connection.CreateCommand();
            cmd2.Transaction = tran;
            cmd2.Connection = connection;
            cmd2.CommandText = "SELECT Name FROM dbo.Products WHERE Id = @id";
            cmd2.Parameters.AddWithValue("id", id);
            SqlDataReader rdr2 = cmd2.ExecuteReader(CommandBehavior.Default); // Returns wrapper around TDS stream
            while (rdr2.Read())
            {
                string pname = rdr2.GetString(0);
                Console.WriteLine($"\t{pname}");
            }
        }
        tran.Rollback();
        connection.Dispose();
    }
}