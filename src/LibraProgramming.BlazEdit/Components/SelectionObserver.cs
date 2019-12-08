using System;
using LibraProgramming.BlazEdit.TinyRx;

namespace LibraProgramming.BlazEdit.Components
{
    internal class SelectionObserver
    {
        public static ISelectionObserver Create(
            Action<SelectionEventArgs> onSelectionStart,
            Action<SelectionEventArgs> onSelectionChange)
        {
            return CreateSubscribe(onSelectionStart, onSelectionChange, Stubs.Nop, Stubs.Throw);
        }

        public static ISelectionObserver Create(
            Action<SelectionEventArgs> onSelectionStart,
            Action<SelectionEventArgs> onSelectionChange,
            Action onCompleted,
            Action<Exception> onError)
        {
            return CreateSubscribe(onSelectionStart, onSelectionChange, onCompleted, onError);
        }

        private static ISelectionObserver CreateSubscribe(
            Action<SelectionEventArgs> onSelectionStart,
            Action<SelectionEventArgs> onSelectionChange,
            Action onCompleted,
            Action<Exception> onError)
        {
            if (null == onSelectionStart)
            {
                throw new ArgumentNullException(nameof(onSelectionStart));
            }

            if (null == onSelectionChange)
            {
                throw new ArgumentNullException(nameof(onSelectionChange));
            }

            if (null == onCompleted)
            {
                throw new ArgumentNullException(nameof(onCompleted));
            }

            if (null == onError)
            {
                throw new ArgumentNullException(nameof(onError));
            }

            if (Stubs.Nop == onCompleted && Stubs.Throw == onError)
            {
                return new EmptySelectionObserver(onSelectionStart, onSelectionChange);
            }

            return new AnonymousSelectionObserver(onSelectionStart, onSelectionChange, onError, onCompleted);
        }

        /// <summary>
        /// 
        /// </summary>
        private class EmptySelectionObserver : ISelectionObserver
        {
            private readonly Action<SelectionEventArgs> onSelectionStart;
            private readonly Action<SelectionEventArgs> onSelectionChange;

            public EmptySelectionObserver(
                Action<SelectionEventArgs> onSelectionStart,
                Action<SelectionEventArgs> onSelectionChange)
            {
                this.onSelectionStart = onSelectionStart;
                this.onSelectionChange = onSelectionChange;
            }

            public void OnCompleted()
            {
                ;
            }

            public void OnError(Exception exception)
            {
                throw exception;
            }

            public void OnSelectionStart(SelectionEventArgs e)
            {
                onSelectionStart.Invoke(e);
            }

            public void OnSelectionChange(SelectionEventArgs e)
            {
                onSelectionChange.Invoke(e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class AnonymousSelectionObserver : ISelectionObserver
        {
            private readonly Action<SelectionEventArgs> onSelectionStart;
            private readonly Action<SelectionEventArgs> onSelectionChange;
            private readonly Action<Exception> onError;
            private readonly Action onCompleted;

            public AnonymousSelectionObserver(
                Action<SelectionEventArgs> onSelectionStart,
                Action<SelectionEventArgs> onSelectionChange,
                Action<Exception> onError,
                Action onCompleted)
            {
                this.onSelectionStart = onSelectionStart;
                this.onSelectionChange = onSelectionChange;
                this.onError = onError;
                this.onCompleted = onCompleted;
            }

            public void OnCompleted()
            {
                onCompleted.Invoke();
            }

            public void OnError(Exception exception)
            {
                onError.Invoke(exception);
            }

            public void OnSelectionStart(SelectionEventArgs e)
            {
                onSelectionStart.Invoke(e);
            }

            public void OnSelectionChange(SelectionEventArgs e)
            {
                onSelectionChange.Invoke(e);
            }
        }
    }
}