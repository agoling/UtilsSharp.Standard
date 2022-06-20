using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace UtilsSharp.AsyncInterceptor
{
    public abstract class ProcessingAsyncInterceptor<TState> : IAsyncInterceptor where TState : class
    {
        /// <summary>
        /// Intercepts a synchronous method <paramref name="invocation"/>.
        /// </summary>
        /// <param name="invocation">The method invocation.</param>
        public void InterceptSynchronous(IInvocation invocation)
        {
            TState state = Proceed(invocation);

            // Signal that the invocation has been completed.
            CompletedInvocation(invocation, state, invocation.ReturnValue);
        }

        /// <summary>
        /// Intercepts an asynchronous method <paramref name="invocation"/> with return type of <see cref="Task"/>.
        /// </summary>
        /// <param name="invocation">The method invocation.</param>
        public void InterceptTaskAsynchronous(IInvocation invocation)
        {
            TState state = Proceed(invocation);
            invocation.ReturnValue = SignalWhenCompleteTaskAsync(invocation, state);
        }

        /// <summary>
        /// Intercepts an asynchronous method <paramref name="invocation"/> with return type of <see cref="Task{T}"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the <see cref="Task{T}"/> <see cref="Task{T}.Result"/>.</typeparam>
        /// <param name="invocation">The method invocation.</param>
        public void InterceptTaskAsynchronous<TResult>(IInvocation invocation)
        {
            TState state = Proceed(invocation);
            invocation.ReturnValue = SignalWhenCompleteTaskAsync<TResult>(invocation, state);
        }

        /// <summary>
        /// Intercepts an asynchronous method <paramref name="invocation"/> with return type of <see cref="Task"/>.
        /// </summary>
        /// <param name="invocation">The method invocation.</param>
        public void InterceptValueTaskAsynchronous(IInvocation invocation)
        {
            TState state = Proceed(invocation);
            invocation.ReturnValue = SignalWhenCompleteValueTaskAsync(invocation, state);
        }

        /// <summary>
        /// Intercepts an asynchronous method <paramref name="invocation"/> with return type of <see cref="Task{T}"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the <see cref="Task{T}"/> <see cref="Task{T}.Result"/>.</typeparam>
        /// <param name="invocation">The method invocation.</param>
        public void InterceptValueTaskAsynchronous<TResult>(IInvocation invocation)
        {
            TState state = Proceed(invocation);
            invocation.ReturnValue = SignalWhenCompleteValueTaskAsync<TResult>(invocation, state);
        }


        /// <summary>
        /// Override in derived classes to receive signals prior method <paramref name="invocation"/>.
        /// </summary>
        /// <param name="invocation">The method invocation.</param>
        /// <returns>The custom object used to maintain state between <see cref="StartingInvocation"/> and
        /// <see cref="CompletedInvocation(IInvocation, TState, object)"/>.</returns>
        protected abstract TState StartingInvocation(IInvocation invocation);

        /// <summary>
        /// Override in derived classes to receive signals after method <paramref name="invocation"/>.
        /// </summary>
        /// <param name="invocation">The method invocation.</param>
        /// <param name="state">The custom object used to maintain state between
        /// <see cref="StartingInvocation(IInvocation)"/> and
        /// <see cref="CompletedInvocation(IInvocation, TState)"/>.</param>
        protected virtual void CompletedInvocation(IInvocation invocation, TState state)
        {
        }

        /// <summary>
        /// Override in derived classes to receive signals after method <paramref name="invocation"/>.
        /// </summary>
        /// <param name="invocation">The method invocation.</param>
        /// <param name="state">The custom object used to maintain state between
        /// <see cref="StartingInvocation(IInvocation)"/> and
        /// <see cref="CompletedInvocation(IInvocation, TState, object)"/>.</param>
        /// <param name="returnValue">
        /// The underlying return value of the <paramref name="invocation"/>; or <see langword="null"/> if the
        /// invocation did not return a value.
        /// </param>
        protected virtual void CompletedInvocation(IInvocation invocation, TState state, object? returnValue)
        {
            CompletedInvocation(invocation, state);
        }

        /// <summary>
        /// Signals the <see cref="StartingInvocation"/> then <see cref="IInvocation.Proceed"/> on the
        /// <paramref name="invocation"/>.
        /// </summary>
        /// <param name="invocation">The method invocation.</param>
        /// <returns>The <typeparamref name="TState"/> returned by <see cref="StartingInvocation"/>.</returns>
        private TState Proceed(IInvocation invocation)
        {
            // Signal that the invocation is about to be started.
            TState state = StartingInvocation(invocation);

            // Execute the invocation.
            invocation.Proceed();

            return state;
        }

        /// <summary>
        /// Returns a <see cref="Task"/> that replaces the <paramref name="invocation"/>
        /// <see cref="IInvocation.ReturnValue"/>, that only completes after
        /// <see cref="CompletedInvocation(IInvocation, TState, object)"/> has been signaled.
        /// </summary>
        /// <param name="invocation">The method invocation.</param>
        /// <param name="state">
        /// The <typeparamref name="TState"/> returned by <see cref="StartingInvocation"/>.
        /// </param>
        private async Task SignalWhenCompleteTaskAsync(IInvocation invocation, TState state)
        {
            // Get the task to await.
            var returnValue = (Task)invocation.ReturnValue;

            await returnValue.ConfigureAwait(false);

            // Signal that the invocation has been completed.
            CompletedInvocation(invocation, state, null);
        }

        /// <summary>
        /// Returns a <see cref="Task{TResult}"/> that replaces the <paramref name="invocation"/>
        /// <see cref="IInvocation.ReturnValue"/>, that only completes after
        /// <see cref="CompletedInvocation(IInvocation, TState, object)"/> has been signaled.
        /// </summary>
        /// <param name="invocation">The method invocation.</param>
        /// <param name="state">
        /// The <typeparamref name="TState"/> returned by <see cref="StartingInvocation"/>.
        /// </param>
        private async Task<TResult> SignalWhenCompleteTaskAsync<TResult>(IInvocation invocation, TState state)
        {
            // Get the task to await.
            var returnValue = (Task<TResult>)invocation.ReturnValue;

            TResult result = await returnValue.ConfigureAwait(false);

            // Signal that the invocation has been completed.
            CompletedInvocation(invocation, state, result);

            return result;
        }

        /// <summary>
        /// Returns a <see cref="ValueTask"/> that replaces the <paramref name="invocation"/>
        /// <see cref="IInvocation.ReturnValue"/>, that only completes after
        /// <see cref="CompletedInvocation(IInvocation, TState, object)"/> has been signaled.
        /// </summary>
        /// <param name="invocation">The method invocation.</param>
        /// <param name="state">
        /// The <typeparamref name="TState"/> returned by <see cref="StartingInvocation"/>.
        /// </param>
        private async ValueTask SignalWhenCompleteValueTaskAsync(IInvocation invocation, TState state)
        {
            // Get the task to await.
            var returnValue = (ValueTask)invocation.ReturnValue;

            await returnValue.ConfigureAwait(false);

            // Signal that the invocation has been completed.
            CompletedInvocation(invocation, state, null);
        }

        /// <summary>
        /// Returns a <see cref="ValueTask{TResult}"/> that replaces the <paramref name="invocation"/>
        /// <see cref="IInvocation.ReturnValue"/>, that only completes after
        /// <see cref="CompletedInvocation(IInvocation, TState, object)"/> has been signaled.
        /// </summary>
        /// <param name="invocation">The method invocation.</param>
        /// <param name="state">
        /// The <typeparamref name="TState"/> returned by <see cref="StartingInvocation"/>.
        /// </param>
        private async ValueTask<TResult> SignalWhenCompleteValueTaskAsync<TResult>(IInvocation invocation, TState state)
        {
            // Get the task to await.
            var returnValue = (ValueTask<TResult>)invocation.ReturnValue;

            TResult result = await returnValue.ConfigureAwait(false);

            // Signal that the invocation has been completed.
            CompletedInvocation(invocation, state, result);

            return result;
        }
    }

}
