using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace UtilsSharp.AsyncInterceptor
{
    public class AsyncDeterminationInterceptor : IInterceptor
    {
        private static readonly MethodInfo HandleTaskAsyncMethodInfo =typeof(AsyncDeterminationInterceptor).GetMethod(nameof(HandleTaskAsyncWithResult), BindingFlags.Static | BindingFlags.NonPublic)!;
        private static readonly MethodInfo HandleValueTaskAsyncMethodInfo = typeof(AsyncDeterminationInterceptor).GetMethod(nameof(HandleValueTaskAsyncWithResult), BindingFlags.Static | BindingFlags.NonPublic)!;


        private static readonly ConcurrentDictionary<Type, GenericAsyncHandler> GenericAsyncHandlers =new ConcurrentDictionary<Type, GenericAsyncHandler>();


        public AsyncDeterminationInterceptor(IAsyncInterceptor asyncInterceptor)
        {
            AsyncInterceptor = asyncInterceptor;
        }

        private delegate void GenericAsyncHandler(IInvocation invocation, IAsyncInterceptor asyncInterceptor);

        private enum MethodType
        {
            Synchronous,
            TaskAsyncAction,
            TaskAsyncFunction,
            ValueTaskAsyncAction,
            ValueTaskAsyncFunction,
        }

        /// <summary>
        /// 获取基础异步侦听器
        /// </summary>
        public IAsyncInterceptor AsyncInterceptor { get; }

        /// <summary>
        /// 拦截方法
        /// </summary>
        /// <param name="invocation"></param>
        [DebuggerStepThrough]
        public virtual void Intercept(IInvocation invocation)
        {
            var methodType = GetMethodType(invocation.Method.ReturnType);
            switch (methodType)
            {
                case MethodType.TaskAsyncAction:
                    AsyncInterceptor.InterceptTaskAsynchronous(invocation);
                    return;
                case MethodType.TaskAsyncFunction:
                    GetTaskHandler(invocation.Method.ReturnType).Invoke(invocation, AsyncInterceptor);
                    return;
                case MethodType.ValueTaskAsyncAction:
                    AsyncInterceptor.InterceptValueTaskAsynchronous(invocation);
                    return;
                case MethodType.ValueTaskAsyncFunction:
                    GetValueTaskHandler(invocation.Method.ReturnType).Invoke(invocation, AsyncInterceptor);
                    return;
                case MethodType.Synchronous:
                default:
                    AsyncInterceptor.InterceptSynchronous(invocation);
                    return;
            }
        }

        /// <summary>
        /// 获取方法类型
        /// </summary>
        /// <param name="returnType"></param>
        /// <returns></returns>
        private static MethodType GetMethodType(Type returnType)
        {
            if (returnType == typeof(void) || returnType.Namespace != "System.Threading.Tasks")
            {
                return MethodType.Synchronous;
            }
            return returnType.Name switch
            {
                "Task" => 
                    returnType.GetTypeInfo().IsGenericType ? MethodType.TaskAsyncFunction : MethodType.TaskAsyncAction,
                "Task`1" => 
                    returnType.GetTypeInfo().IsGenericType ? MethodType.TaskAsyncFunction : MethodType.TaskAsyncAction,
                "ValueTask" =>
                    returnType.GetTypeInfo().IsGenericType? MethodType.ValueTaskAsyncFunction:MethodType.ValueTaskAsyncAction,
                "ValueTask`1" =>
                    returnType.GetTypeInfo().IsGenericType? MethodType.ValueTaskAsyncFunction: MethodType.ValueTaskAsyncAction,
                _ => MethodType.Synchronous
            };
        }

        /// <summary>
        /// GetTaskHandler
        /// </summary>
        private static GenericAsyncHandler GetTaskHandler(Type returnType)
        {
            GenericAsyncHandler handler = GenericAsyncHandlers.GetOrAdd(returnType, CreateTaskHandler);
            return handler;
        }

        /// <summary>
        /// GetValueTaskHandler
        /// </summary>
        private static GenericAsyncHandler GetValueTaskHandler(Type returnType)
        {
            GenericAsyncHandler handler = GenericAsyncHandlers.GetOrAdd(returnType, CreateValueTaskHandler);
            return handler;
        }

        /// <summary>
        /// CreateTaskHandler
        /// </summary>
        private static GenericAsyncHandler CreateTaskHandler(Type returnType)
        {
            Type taskReturnType = returnType.GetGenericArguments()[0];
            MethodInfo method = HandleTaskAsyncMethodInfo.MakeGenericMethod(taskReturnType);
            return (GenericAsyncHandler)method.CreateDelegate(typeof(GenericAsyncHandler));
        }

        /// <summary>
        /// CreateValueTaskHandler
        /// </summary>
        private static GenericAsyncHandler CreateValueTaskHandler(Type returnType)
        {
            Type taskReturnType = returnType.GetGenericArguments()[0];
            MethodInfo method = HandleValueTaskAsyncMethodInfo.MakeGenericMethod(taskReturnType);
            return (GenericAsyncHandler)method.CreateDelegate(typeof(GenericAsyncHandler));
        }


        /// <summary>
        /// 此方法创建为委托，用于调用泛型(Task)
        /// </summary>
        private static void HandleTaskAsyncWithResult<TResult>(IInvocation invocation, IAsyncInterceptor asyncInterceptor)
        {
            asyncInterceptor.InterceptTaskAsynchronous<TResult>(invocation);
        }


        /// <summary>
        /// 此方法创建为委托，用于调用泛型(ValueTask)
        /// </summary>
        private static void HandleValueTaskAsyncWithResult<TResult>(IInvocation invocation, IAsyncInterceptor asyncInterceptor)
        {
            asyncInterceptor.InterceptValueTaskAsynchronous<TResult>(invocation);
        }
    }
}
