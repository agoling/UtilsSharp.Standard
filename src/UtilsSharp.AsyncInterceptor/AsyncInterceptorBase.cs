using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace UtilsSharp.AsyncInterceptor
{
    /// <summary>
    /// Task
    /// </summary>
    [SuppressMessage(
    "Design", "CA1031:Do not catch general exception types", Justification = "Must propagate the same exceptions.")]
    public abstract partial class AsyncInterceptorBase : IAsyncInterceptor
    {
#if NET45
    /// <summary>
    /// A completed <see cref="Task"/>.
    /// </summary>
    private static readonly Task CompletedTask = Task.FromResult(0);
#endif

        private static readonly MethodInfo InterceptTaskSynchronousMethodInfo =
            typeof(AsyncInterceptorBase).GetMethod(
                nameof(InterceptTaskSynchronousResult), BindingFlags.Static | BindingFlags.NonPublic)!;

        private static readonly ConcurrentDictionary<Type, GenericTaskSynchronousHandler> GenericTaskSynchronousHandlers =
            new ConcurrentDictionary<Type, GenericTaskSynchronousHandler>
            {
                [typeof(void)] = InterceptTaskSynchronousVoid,
            };

        private delegate void GenericTaskSynchronousHandler(AsyncInterceptorBase me, IInvocation invocation);

        /// <summary>
        /// Intercepts a synchronous method <paramref name="invocation"/>.
        /// </summary>
        /// <param name="invocation">The method invocation.</param>
        public void InterceptSynchronous(IInvocation invocation)
        {
            Type returnType = invocation.Method.ReturnType;
            GenericTaskSynchronousHandler taskHandler = GenericTaskSynchronousHandlers.GetOrAdd(returnType, CreateTaskHandler);
            taskHandler(this, invocation);
            GenericValueTaskSynchronousHandler valueTaskhandler = GenericValueTaskSynchronousHandlers.GetOrAdd(returnType, CreateValueTaskHandler);
            valueTaskhandler(this, invocation);
        }

        /// <summary>
        /// Intercepts an asynchronous method <paramref name="invocation"/> with return type of <see cref="Task"/>.
        /// </summary>
        /// <param name="invocation">The method invocation.</param>
        public void InterceptTaskAsynchronous(IInvocation invocation)
        {
            invocation.ReturnValue = InterceptTaskAsync(invocation, invocation.CaptureProceedInfo(), ProceedTaskAsynchronous);
        }

        /// <summary>
        /// Intercepts an asynchronous method <paramref name="invocation"/> with return type of <see cref="Task{T}"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the <see cref="Task{T}"/> <see cref="Task{T}.Result"/>.</typeparam>
        /// <param name="invocation">The method invocation.</param>
        public void InterceptTaskAsynchronous<TResult>(IInvocation invocation)
        {
            invocation.ReturnValue =
                InterceptTaskAsync(invocation, invocation.CaptureProceedInfo(), ProceedTaskAsynchronous<TResult>);
        }

        /// <summary>
        /// Override in derived classes to intercept method invocations.
        /// </summary>
        /// <param name="invocation">The method invocation.</param>
        /// <param name="proceedInfo">The <see cref="IInvocationProceedInfo"/>.</param>
        /// <param name="proceed">The function to proceed the <paramref name="proceedInfo"/>.</param>
        /// <returns>A <see cref="Task" /> object that represents the asynchronous operation.</returns>
        protected abstract Task InterceptTaskAsync(
            IInvocation invocation,
            IInvocationProceedInfo proceedInfo,
            Func<IInvocation, IInvocationProceedInfo, Task> proceed);

        /// <summary>
        /// Override in derived classes to intercept method invocations.
        /// </summary>
        /// <typeparam name="TResult">The type of the <see cref="Task{T}"/> <see cref="Task{T}.Result"/>.</typeparam>
        /// <param name="invocation">The method invocation.</param>
        /// <param name="proceedInfo">The <see cref="IInvocationProceedInfo"/>.</param>
        /// <param name="proceed">The function to proceed the <paramref name="proceedInfo"/>.</param>
        /// <returns>A <see cref="Task" /> object that represents the asynchronous operation.</returns>
        protected abstract Task<TResult> InterceptTaskAsync<TResult>(
            IInvocation invocation,
            IInvocationProceedInfo proceedInfo,
            Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed);

        private static GenericTaskSynchronousHandler CreateTaskHandler(Type returnType)
        {
            MethodInfo method = InterceptTaskSynchronousMethodInfo.MakeGenericMethod(returnType);
            return (GenericTaskSynchronousHandler)method.CreateDelegate(typeof(GenericTaskSynchronousHandler));
        }

        private static void InterceptTaskSynchronousVoid(AsyncInterceptorBase me, IInvocation invocation)
        {
            Task task = me.InterceptTaskAsync(invocation, invocation.CaptureProceedInfo(), ProceedTaskSynchronous);

            // If the intercept task has yet to complete, wait for it.
            if (!task.IsCompleted)
            {
                // Need to use Task.Run() to prevent deadlock in .NET Framework ASP.NET requests.
                // GetAwaiter().GetResult() prevents a thrown exception being wrapped in a AggregateException.
                // See https://stackoverflow.com/a/17284612
                Task.Run(() => task).GetAwaiter().GetResult();
            }

            task.RethrowIfFaulted();
        }

        private static void InterceptTaskSynchronousResult<TResult>(AsyncInterceptorBase me, IInvocation invocation)
        {
            Task<TResult> task = me.InterceptTaskAsync(invocation, invocation.CaptureProceedInfo(), ProceedTaskSynchronous<TResult>);

            // If the intercept task has yet to complete, wait for it.
            if (!task.IsCompleted)
            {
                // Need to use Task.Run() to prevent deadlock in .NET Framework ASP.NET requests.
                // GetAwaiter().GetResult() prevents a thrown exception being wrapped in a AggregateException.
                // See https://stackoverflow.com/a/17284612
                Task.Run(() => task).GetAwaiter().GetResult();
            }

            task.RethrowIfFaulted();
        }

        private static Task ProceedTaskSynchronous(IInvocation invocation, IInvocationProceedInfo proceedInfo)
        {
            try
            {
                proceedInfo.Invoke();
#if NET45
            return CompletedTask;
#else
                return Task.CompletedTask;
#endif
            }
            catch (Exception e)
            {
#if NET45
            var tcs = new TaskCompletionSource<int>();
            tcs.SetException(e);
            return tcs.Task;
#else
                return Task.FromException(e);
#endif
            }
        }

        private static Task<TResult> ProceedTaskSynchronous<TResult>(
            IInvocation invocation,
            IInvocationProceedInfo proceedInfo)
        {
            try
            {
                proceedInfo.Invoke();
                return Task.FromResult((TResult)invocation.ReturnValue);
            }
            catch (Exception e)
            {
#if NET45
            var tcs = new TaskCompletionSource<TResult>();
            tcs.SetException(e);
            return tcs.Task;
#else
                return Task.FromException<TResult>(e);
#endif
            }
        }

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "The name explicitly says Asynchronous.")]
        private static async Task ProceedTaskAsynchronous(IInvocation invocation, IInvocationProceedInfo proceedInfo)
        {
            proceedInfo.Invoke();

            // Get the task to await.
            var originalReturnValue = (Task)invocation.ReturnValue;

            await originalReturnValue.ConfigureAwait(false);
        }

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "The name explicitly says Asynchronous.")]
        private static async Task<TResult> ProceedTaskAsynchronous<TResult>(
            IInvocation invocation,
            IInvocationProceedInfo proceedInfo)
        {
            proceedInfo.Invoke();

            // Get the task to await.
            var originalReturnValue = (Task<TResult>)invocation.ReturnValue;

            TResult result = await originalReturnValue.ConfigureAwait(false);
            return result;
        }
    }

    public abstract partial class AsyncInterceptorBase
    {
        private static readonly MethodInfo InterceptValueTaskSynchronousMethodInfo =
            typeof(AsyncInterceptorBase).GetMethod(
                nameof(InterceptValueTaskSynchronousResult), BindingFlags.Static | BindingFlags.NonPublic)!;

        private static readonly ConcurrentDictionary<Type, GenericValueTaskSynchronousHandler> GenericValueTaskSynchronousHandlers =
            new ConcurrentDictionary<Type, GenericValueTaskSynchronousHandler>
            {
                [typeof(void)] = InterceptValueTaskSynchronousVoid,
            };

        private delegate void GenericValueTaskSynchronousHandler(AsyncInterceptorBase me, IInvocation invocation);



        /// <summary>
        /// Intercepts an asynchronous method <paramref name="invocation"/> with return type of <see cref="ValueTask"/>.
        /// </summary>
        /// <param name="invocation">The method invocation.</param>
        public void InterceptValueTaskAsynchronous(IInvocation invocation)
        {
            invocation.ReturnValue = InterceptValueTaskAsync(invocation, invocation.CaptureProceedInfo(), ProceedValueTaskAsynchronous);
        }

        /// <summary>
        /// Intercepts an asynchronous method <paramref name="invocation"/> with return type of <see cref="ValueTask{T}"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the <see cref="ValueTask{T}"/> <see cref="Task{T}.Result"/>.</typeparam>
        /// <param name="invocation">The method invocation.</param>
        public void InterceptValueTaskAsynchronous<TResult>(IInvocation invocation)
        {
            invocation.ReturnValue = InterceptValueTaskAsync(invocation, invocation.CaptureProceedInfo(), ProceedValueTaskAsynchronous<TResult>);
        }

        /// <summary>
        /// Override in derived classes to intercept method invocations.
        /// </summary>
        /// <param name="invocation">The method invocation.</param>
        /// <param name="proceedInfo">The <see cref="IInvocationProceedInfo"/>.</param>
        /// <param name="proceed">The function to proceed the <paramref name="proceedInfo"/>.</param>
        /// <returns>A <see cref="ValueTask" /> object that represents the asynchronous operation.</returns>
        protected abstract ValueTask InterceptValueTaskAsync(
            IInvocation invocation,
            IInvocationProceedInfo proceedInfo,
            Func<IInvocation, IInvocationProceedInfo, ValueTask> proceed);

        /// <summary>
        /// Override in derived classes to intercept method invocations.
        /// </summary>
        /// <typeparam name="TResult">The type of the <see cref="ValueTask{T}"/> <see cref="ValueTask{T}.Result"/>.</typeparam>
        /// <param name="invocation">The method invocation.</param>
        /// <param name="proceedInfo">The <see cref="IInvocationProceedInfo"/>.</param>
        /// <param name="proceed">The function to proceed the <paramref name="proceedInfo"/>.</param>
        /// <returns>A <see cref="ValueTask" /> object that represents the asynchronous operation.</returns>
        protected abstract ValueTask<TResult> InterceptValueTaskAsync<TResult>(
            IInvocation invocation,
            IInvocationProceedInfo proceedInfo,
            Func<IInvocation, IInvocationProceedInfo, ValueTask<TResult>> proceed);

        private static GenericValueTaskSynchronousHandler CreateValueTaskHandler(Type returnType)
        {
            MethodInfo method = InterceptValueTaskSynchronousMethodInfo.MakeGenericMethod(returnType);
            return (GenericValueTaskSynchronousHandler)method.CreateDelegate(typeof(GenericValueTaskSynchronousHandler));
        }

        private static void InterceptValueTaskSynchronousVoid(AsyncInterceptorBase me, IInvocation invocation)
        {
            ValueTask task = me.InterceptValueTaskAsync(invocation, invocation.CaptureProceedInfo(), ProceedValueTaskSynchronous);

            // If the intercept task has yet to complete, wait for it.
            if (!task.IsCompleted)
            {
                // Need to use Task.Run() to prevent deadlock in .NET Framework ASP.NET requests.
                // GetAwaiter().GetResult() prevents a thrown exception being wrapped in a AggregateException.
                // See https://stackoverflow.com/a/17284612
                Task.Run(() => task).GetAwaiter().GetResult();
            }
            task.RethrowIfFaulted();
        }

        private static void InterceptValueTaskSynchronousResult<TResult>(AsyncInterceptorBase me, IInvocation invocation)
        {
            ValueTask<TResult> task = me.InterceptValueTaskAsync(invocation, invocation.CaptureProceedInfo(), ProceedValueTaskSynchronous<TResult>);

            // If the intercept task has yet to complete, wait for it.
            if (!task.IsCompleted)
            {
                // Need to use Task.Run() to prevent deadlock in .NET Framework ASP.NET requests.
                // GetAwaiter().GetResult() prevents a thrown exception being wrapped in a AggregateException.
                // See https://stackoverflow.com/a/17284612
                Task.Run(() => task).GetAwaiter().GetResult();
            }

            task.RethrowIfFaulted();
        }

        /// <summary>
        /// ValueTask
        /// </summary>
        private static ValueTask ProceedValueTaskSynchronous(IInvocation invocation, IInvocationProceedInfo proceedInfo)
        {
            try
            {
                proceedInfo.Invoke();
                return new ValueTask();
            }
            catch (Exception e)
            {
                var avt = new AsyncValueTaskMethodBuilder();
                avt.SetException(e);
                return avt.Task;
            }
        }

        private static ValueTask<TResult> ProceedValueTaskSynchronous<TResult>(
            IInvocation invocation,
            IInvocationProceedInfo proceedInfo)
        {
            try
            {
                proceedInfo.Invoke();
                return new ValueTask<TResult>((TResult) invocation.ReturnValue); 
            }
            catch (Exception e)
            {
                var avt = new AsyncValueTaskMethodBuilder<TResult>();
                avt.SetException(e);
                return avt.Task;
            }
        }

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "The name explicitly says Asynchronous.")]
        private static async ValueTask ProceedValueTaskAsynchronous(IInvocation invocation, IInvocationProceedInfo proceedInfo)
        {
            proceedInfo.Invoke();

            // Get the task to await.
            var originalReturnValue = (ValueTask)invocation.ReturnValue;

            await originalReturnValue.ConfigureAwait(false);
        }

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "The name explicitly says Asynchronous.")]
        private static async ValueTask<TResult> ProceedValueTaskAsynchronous<TResult>(
            IInvocation invocation,
            IInvocationProceedInfo proceedInfo)
        {
            proceedInfo.Invoke();
            // Get the task to await.
            var originalReturnValue = (ValueTask<TResult>)invocation.ReturnValue;
            TResult result = await originalReturnValue.ConfigureAwait(false);
            return result;
        }
    }
}
