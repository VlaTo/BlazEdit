using System;
using System.Collections.Generic;
using System.Linq;
using LibraProgramming.BlazEdit.Components;

namespace LibraProgramming.BlazEdit.TinyRx
{
    public class ObservableBase<TObserver> : IObservable<TObserver>
        where TObserver : IObserver
    {
        private readonly Dictionary<TObserver, IDisposable> observers;
        private readonly object gate;

        protected ObservableBase()
        {
            gate = new object();
            observers = new Dictionary<TObserver, IDisposable>();
        }

        public IDisposable Subscribe(TObserver observer)
        {
            if (null == observer)
            {
                throw new ArgumentNullException(nameof(observer));
            }

            lock (gate)
            {
                if (observers.TryGetValue(observer, out var disposable))
                {
                    return disposable;
                }

                var subscription = new Subscription(this, observer);

                observers.Add(observer, subscription);

                return subscription;
            }
        }

        protected void Raise(Action<TObserver> action)
        {
            Array.ForEach(GetObservers(), action);
        }

        private TObserver[] GetObservers()
        {
            lock (gate)
            {
                return observers.Count > 0 ? observers.Keys.ToArray() : Array.Empty<TObserver>();
            }
        }

        private void RemoveObserver(TObserver observer, bool dispose)
        {
            lock (gate)
            {
                if (observers.TryGetValue(observer, out var disposable))
                {
                    if (observers.Remove(observer))
                    {
                        observer.OnCompleted();
                    }

                    if (dispose)
                    {
                        disposable.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class Subscription : IDisposable
        {
            private readonly ObservableBase<TObserver> owner;
            private readonly TObserver observer;
            private bool disposed;

            public Subscription(ObservableBase<TObserver> owner, TObserver observer)
            {
                this.owner = owner;
                this.observer = observer;
            }

            public void Dispose()
            {
                Dispose(true);
            }

            private void Dispose(bool dispose)
            {
                if (disposed)
                {
                    return;
                }

                try
                {
                    if (dispose)
                    {
                        owner.RemoveObserver(observer, false);
                    }
                }
                finally
                {
                    disposed = true;
                }
            }
        }
    }
}