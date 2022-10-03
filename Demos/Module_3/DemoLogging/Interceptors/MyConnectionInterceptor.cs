using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace DemoLogging.Interceptors;

public class MyConnectionInterceptor: DbConnectionInterceptor
{
    public override void ConnectionOpened(DbConnection connection, ConnectionEndEventData eventData)
    {
        Console.WriteLine("Whoehoeee. The connection is opened");
        base.ConnectionOpened(connection, eventData);
    }
    public override void ConnectionClosed(DbConnection connection, ConnectionEndEventData eventData)
    {
        Console.WriteLine("Awww. The connection is closed");
        base.ConnectionClosed(connection, eventData);
    }
}
