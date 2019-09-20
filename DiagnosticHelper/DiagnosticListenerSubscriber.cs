using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DiagnosticHelper
{
    /// <summary>
    /// Generic base class to help subscribing to diagnostic events
    /// </summary>
    public abstract class DiagnosticListenerSubscriber :
        IObserver<DiagnosticListener>,
        IObserver<KeyValuePair<string, object>>
    {
        private readonly Func<DiagnosticListener, bool> _listenerPredicate;

        public DiagnosticListenerSubscriber(Func<DiagnosticListener, bool> predicate)
        {
            _listenerPredicate = predicate;
        }

        public DiagnosticListenerSubscriber(string listnerNamePrefix)
        {
            _listenerPredicate = listener => listener.Name.StartsWith(listnerNamePrefix);
        }

        public void Subscribe()
        {
            DiagnosticListener.AllListeners.Subscribe(this);
        }

        public virtual void OnCompleted() { }

        public virtual void OnError(Exception error) { }

        public abstract void OnEvent(DiagnosticLogEvent logEvent);

        public void OnNext(DiagnosticListener listener)
        {
            if (_listenerPredicate(listener))
            {
                listener.Subscribe(this);
            }
        }

        public void OnNext(KeyValuePair<string, object> log)
        {
            OnEvent(new DiagnosticLogEvent(log));
        }
    }
}
