using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace DemoLogging.Interceptors;

public class MyTransactionInterceptor: DbTransactionInterceptor
{
    public override DbTransaction TransactionStarted(DbConnection connection, TransactionEndEventData eventData, DbTransaction result)
    {
        Console.WriteLine("We started a transaction...");
        return base.TransactionStarted(connection, eventData, result);
    }
    public override void TransactionCommitted(DbTransaction transaction, TransactionEndEventData eventData)
    {
        Console.WriteLine("The transaction is commited");
        base.TransactionCommitted(transaction, eventData);
    }
    public override void TransactionRolledBack(DbTransaction transaction, TransactionEndEventData eventData)
    {
        Console.WriteLine("The transaction is rolled back");
        base.TransactionRolledBack(transaction, eventData);
    }
}
