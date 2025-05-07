using System;
using System.Collections.Generic;

namespace Signals
{
    public class TestSignalBus
    {
        private HashSet<Type> _unsubscribed = new();
        private List<object> _firedSignals = new();

        public void Fire<TSignal>(TSignal signal)
        {
            _firedSignals.Add(signal);
        }

        public bool HasFired<TSignal>()
        {
            return _firedSignals.Exists(s => s is TSignal);
        }

        public void Subscribe<TSignal>(Action callback) { }
        public void TryUnsubscribe<TSignal>(Action callback)
        {
            _unsubscribed.Add(typeof(TSignal));
        }

        public bool Unsubscribed<TSignal>()
        {
            return _unsubscribed.Contains(typeof(TSignal));
        }
    }
}