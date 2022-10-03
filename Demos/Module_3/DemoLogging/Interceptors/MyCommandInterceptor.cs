using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace DemoLogging.Interceptors;

public class MyCommandInterceptor : DbCommandInterceptor
{
    public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
    {
        if (command.CommandText.Contains("FROM [Core].[ProductGroups]"))
        {
            command.CommandText += "WHERE Id > 5";
        }
        return base.ReaderExecuting(command, eventData, result);
    }
}
