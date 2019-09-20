using System;
using System.Collections.Generic;

namespace DiagnosticHelper
{
    public class DiagnosticLogEvent
    {
        private readonly KeyValuePair<string, object> _log;
        public DiagnosticLogEvent(KeyValuePair<string, object> log) => _log = log;

        public void TryInvokeEventHandler<T>(EventHandler<T> handler) where T : EventArgs
        {
            if (_log.Value is T args)
            {
                handler?.Invoke(_log, args);
            }
        }

        public string Key => _log.Key;
        public object Value => _log.Value;
    }
}
