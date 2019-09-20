using System;
using System.Diagnostics;

namespace DiagnosticHelper
{
    public static class DiagnosticLoggingExtensions
    {
        public static void Log(this DiagnosticSource source, EventArgs eventArgs)
        {
            var name = eventArgs.GetType().Name;
            if (source.IsEnabled(name))
            {
                source.Write(name, eventArgs);
            }
        }
    }
}
