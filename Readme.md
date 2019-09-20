# Diagnostic Helper

Simplify subscribing to diagnostic events using `System.Diagnostics.DiagnosticSource`

```csharp
public class MyHandlerStarted : EventArgs { /**/ }
public class MyHandlerFinished : EventArgs { /**/ }
public class MyHandlerFailed : EventArgs { /**/ }

// Example usage of DiagnosticSource logging
public class MyHandler
{
    private static DiagnosticSource _log = new DiagnosticListener("MyHandler");
    public void Handle()
    {
        _log.Log(new MyHandlerStarted(/* possible payload here */));

        try
        {
            // Do something interesting
            _log.Log(new MyHandlerFinished(/* possible payload here */));
        }
        catch (Exception ex)
        {
            _log.Log(new MyHandlerFailed(ex));
            throw;
        }
    }
}

// Can be used by consumers
public class MyDiagnosticEventSubscriber : DiagnosticListenerSubscriber
{
    public EventHandler<MyHandlerStarted> OnMyHandlerStarted;
    public EventHandler<MyHandlerFinished> OnMyHandlerFinished;
    public EventHandler<MyHandlerFailed> OnMyHandlerFailed;

    public MyDiagnosticEventSubscriber() : base("MyHandler") { }

    public override void OnEvent(DiagnosticLogEvent log)
    {
        log.TryInvokeEventHandler(OnMyHandlerStarted);
        log.TryInvokeEventHandler(OnMyHandlerFinished);
        log.TryInvokeEventHandler(OnMyHandlerFailed);
    }
}

// Example usage
public class LogMyHandlerEvents : MyDiagnosticEventSubscriber
{
    private ILogger _log;
    public LogStuff(ILogger log) => _log = log;
    public JobSchedulingDiagnosticEventSubscriber() : base("MyHandler")
    {
        OnMyHandlerStarted += MyHandlerStarted;
        OnMyHandlerFinished += MyHandlerFinished;
        OnMyHandlerFailed += MyHandlerFailed;
    }

    public void MyHandlerStarted(this sender, MyHandlerStarted args)
    {
        _log.Debug("My handler started with {Context}", args.Context);
    }

    public void MyHandlerFinished(this sender, MyHandlerFinished args)
    {
        _log.Debug("My handler finished with {Context}", args.Context);
    }

    public void MyHandlerFailed(this sender, MyHandlerFinished args)
    {
        _log.Critical(args.Exception, "My handler failed");
    }
}

// Invoke subscription like this
new LogMyHandlerEvents(logger).Subscribe();
```
