using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Castle.DynamicProxy;

namespace UtilsSharp.AsyncInterceptor
{
    public abstract class AsyncTimingInterceptor : ProcessingAsyncInterceptor<Stopwatch>
    {
        /// <summary>
        /// Signals <see cref="StartingTiming"/> before starting a <see cref="Stopwatch"/> to time the method
        /// <paramref name="invocation"/>.
        /// </summary>
        /// <param name="invocation">The method invocation.</param>
        /// <returns>The <see cref="Stopwatch"/> to time the method <paramref name="invocation"/>.</returns>
        protected sealed override Stopwatch StartingInvocation(IInvocation invocation)
        {
            StartingTiming(invocation);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            return stopwatch;
        }

        /// <summary>
        /// Signals <see cref="CompletedTiming"/> after stopping a <see cref="Stopwatch"/> to time the method
        /// <paramref name="invocation"/>.
        /// </summary>
        /// <param name="invocation">The method invocation.</param>
        /// <param name="state">The <see cref="Stopwatch"/> returned by <see cref="StartingInvocation"/> to time
        /// the method <paramref name="invocation"/>.</param>
        protected sealed override void CompletedInvocation(IInvocation invocation, Stopwatch state)
        {
            state.Stop();
            CompletedTiming(invocation, state);
        }

        /// <summary>
        /// Override in derived classes to receive signals prior method <paramref name="invocation"/> timing.
        /// </summary>
        /// <param name="invocation">The method invocation.</param>
        protected abstract void StartingTiming(IInvocation invocation);

        /// <summary>
        /// Override in derived classes to receive signals after method <paramref name="invocation"/> timing.
        /// </summary>
        /// <param name="invocation">The method invocation.</param>
        /// <param name="stopwatch">A <see cref="Stopwatch"/> used to time the method <paramref name="invocation"/>.
        /// </param>
        protected abstract void CompletedTiming(IInvocation invocation, Stopwatch stopwatch);
    }
}
