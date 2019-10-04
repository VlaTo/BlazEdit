using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using LibraProgramming.BlazEdit.Extensions;

namespace LibraProgramming.BlazEdit.Core
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class MessageAggregator : IMessageAggregator
    {
        private readonly Dictionary<Type, MessageHandlers> messages;
        private readonly object syncRoot;

        /// <summary>
        /// 
        /// </summary>
        public MessageAggregator()
        {
            syncRoot = new object();
            messages = new Dictionary<Type, MessageHandlers>();
        }

        /// <inheritdoc cref="IMessageAggregator.Publish" />
        public void Publish(IMessage message)
        {
            if (null == message)
            {
                throw new ArgumentNullException(nameof(message));
            }

            MessageHandlers handlers;

            lock (syncRoot)
            {
                messages.TryGetValue(message.GetType(), out handlers);
            }

            if (null != handlers)
            {
                handlers.InvokeAsync(message).RunAndForget();
            }
        }

        /// <inheritdoc cref="IMessageAggregator.Subscribe" />
        public IDisposable Subscribe(IMessageHandler handler)
        {
            if (null == handler)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            foreach (var implementedInterface in handler.GetType().GetInterfaces())
            {
                if (false == implementedInterface.IsGenericType)
                {
                    continue;
                }

                var hasMessageHandler = Array.Exists(
                    implementedInterface.GetInterfaces(),
                    x => x.IsAssignableFrom(typeof(IMessageHandler))
                );

                if (false == hasMessageHandler)
                {
                    continue;
                }

                var messageTypes = implementedInterface.GetGenericArguments();
                
                if (1 != messageTypes.Length)
                {
                    continue;
                }

                AddMessageHandler(messageTypes[0], handler);
            }

            return new Subscription(this, handler);
        }

        private void AddMessageHandler(Type messageType, IMessageHandler handler)
        {
            lock (syncRoot)
            {
                if (false == messages.TryGetValue(messageType, out var handlers))
                {
                    handlers = new MessageHandlers(messageType);
                    messages.Add(messageType, handlers);
                }

                handlers.Add(handler);
            }
        }

        private void RemoveHandler(IMessageHandler handler)
        {
            lock (syncRoot)
            {
                foreach (var key in messages.Keys)
                {
                    var handlers = messages[key];

                    if (handlers.Remove(handler) && 0 < handlers.Count)
                    {
                        messages.Remove(key);
                    }
                }
            }
        }

        /// <summary>
        /// Defines holder for message handlers.
        /// </summary>
        private class MessageHandlers
        {
            private readonly Collection<WeakReference<IMessageHandler>> messageHandlers;
            private readonly MethodInfo handleMethod;

            public int Count => messageHandlers.Count;

            public MessageHandlers(Type messageType)
            {
                var handlerType = typeof(IMessageHandler<>).MakeGenericType(messageType);
                handleMethod = handlerType.GetMethod("HandleAsync");
                messageHandlers = new Collection<WeakReference<IMessageHandler>>();
            }

            public void Add(IMessageHandler handler)
            {
                messageHandlers.Add(new WeakReference<IMessageHandler>(handler));
            }

            public Task InvokeAsync(IMessage message)
            {
                var references = messageHandlers.ToArray();

                return Task.Factory.StartNew(async () =>
                {
                    //var m = Delegate.CreateDelegate(null, null, true);

                    foreach (var reference in references)
                    {
                        if (reference.TryGetTarget(out var target))
                        {
                            var result = (Task) handleMethod.Invoke(target, new object[] {message});

                            await result;

                            continue;
                        }

                        messageHandlers.Remove(reference);
                    }
                });
            }

            public bool Remove(IMessageHandler handler)
            {
                var index = FindIndex(handler);

                if (0 > index)
                {
                    return false;
                }

                messageHandlers.RemoveAt(index);

                return true;
            }

            private int FindIndex(IMessageHandler handler)
            {
                for (var index = 0; index < messageHandlers.Count; index++)
                {
                    if (messageHandlers[index].TryGetTarget(out var target))
                    {
                        if (ReferenceEquals(target, handler))
                        {
                            return index;
                        }
                    }
                }

                return -1;
            }
        }

        /// <summary>
        /// Message handler subscription token.
        /// </summary>
        private class Subscription : IDisposable
        {
            private readonly MessageAggregator aggregator;
            private readonly IMessageHandler handler;

            public Subscription(MessageAggregator aggregator, IMessageHandler handler)
            {
                this.aggregator = aggregator;
                this.handler = handler;
            }

            public void Dispose()
            {
                aggregator.RemoveHandler(handler);
            }
        }
    }
}