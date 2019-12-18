using System;

namespace LibraProgramming.BlazEdit.TinyRx
{
    /// <summary>
    /// 
    /// </summary>
    public interface IObservable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="observer"></param>
        /// <returns></returns>
        IDisposable Subscribe(IObserver observer);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TObserver"></typeparam>
    public interface IObservable<in TObserver>
        where TObserver : IObserver
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="observer"></param>
        /// <returns></returns>
        IDisposable Subscribe(TObserver observer);
    }
}