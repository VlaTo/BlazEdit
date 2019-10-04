using System;
using Microsoft.AspNetCore.Components;

namespace LibraProgramming.BlazEdit.Components
{
    public class ToolComponent : ComponentBase, IDisposable
    {
        private bool disposed;

        protected ToolComponent()
        {
        }

        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            try
            {
                OnDispose();
            }
            finally
            {
                disposed = true;
            }
        }

        protected virtual void OnDispose()
        {
        }
    }
}