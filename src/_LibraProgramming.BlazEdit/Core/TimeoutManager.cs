using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibraProgramming.BlazEdit.Extensions;

namespace LibraProgramming.BlazEdit.Core
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class TimeoutManager : ITimeoutManager, IDisposable
    {
        private static readonly TimeSpan Never = TimeSpan.FromMilliseconds(-1.0d);
        private readonly Collection<TimeoutSubscription> subscriptions;
        private bool disposed;

        /// <summary>
        /// 
        /// </summary>
        public TimeoutManager()
        {
            subscriptions = new Collection<TimeoutSubscription>();
        }

        /// <inheritdoc cref="ITimeoutManager.CreateTimeout(System.Action, System.TimeSpan)" />
        public ITimeout CreateTimeout(Action callback, TimeSpan timeout)
        {
            EnsureNotDisposed();

            if (null == callback)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            return CreateTimeoutInternal(() => new Task(callback), timeout);
        }

        /// <inheritdoc cref="ITimeoutManager.CreateTimeout(System.Func{System.Threading.Tasks.Task}, System.TimeSpan)" />
        public ITimeout CreateTimeout(Func<Task> callback, TimeSpan timeout)
        {
            EnsureNotDisposed();

            if (null == callback)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            return CreateTimeoutInternal(callback, timeout);
        }

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            Dispose(true);
        }

        private ITimeout CreateTimeoutInternal(Func<Task> callback, TimeSpan timeout)
        {
            var subscription = new TimeoutSubscription(this, timeout, callback);

            subscriptions.Add(subscription);

            return subscription;
        }

        private void RemoveSubscription(TimeoutSubscription subscription)
        {
            if (subscriptions.Remove(subscription))
            {
                ;
            }
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
                    var disposables = subscriptions.ToArray();
                    foreach (var disposable in disposables)
                    {
                        disposable.Dispose();
                    }
                }
            }
            finally
            {
                disposed = true;
            }
        }

        private void EnsureNotDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(TimeoutManager));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private sealed class TimeoutSubscription : ITimeout
        {
            private readonly TimeoutManager manager;
            private readonly Func<Task> callback;
            private readonly Timer timer;
            private bool disposed;

            public TimeoutSubscription(TimeoutManager manager, TimeSpan timeout, Func<Task> callback)
            {
                this.manager = manager;
                this.callback = callback;

                timer = new Timer(OnTimerCallback, null, timeout, Never);
            }

            public void Dispose()
            {
                Dispose(true);
            }

            private void OnTimerCallback(object arg)
            {
                if (disposed)
                {
                    return;
                }

                Task.Run(callback.Invoke).RunAndForget();

                Dispose();
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
                        timer.Dispose();
                        manager.RemoveSubscription(this);
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