
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DemoLogging;

public class DiagnosticObserver : IObserver<DiagnosticListener>
{
    public void OnCompleted()
    {
        Console.WriteLine("Provider finished sending data");
    }

    public void OnError(Exception error)
    {
        Console.WriteLine(error.Message);
    }

    public void OnNext(DiagnosticListener value)
    {
        if (value.Name == DbLoggerCategory.Name)
        {
            value.Subscribe(new KeyValueObserver());
        }
    }
}
