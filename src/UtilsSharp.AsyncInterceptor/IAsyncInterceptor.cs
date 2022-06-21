using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace UtilsSharp.AsyncInterceptor
{
    /// <summary>
    /// 异步拦截
    /// </summary>
    public interface IAsyncInterceptor
    {
        /// <summary>
        /// 同步拦截
        /// </summary>
        /// <param name="invocation">IInvocation</param>
        void InterceptSynchronous(IInvocation invocation);

        /// <summary>
        /// 异步Task拦截
        /// </summary>
        /// <param name="invocation">IInvocation</param>
        void InterceptTaskAsynchronous(IInvocation invocation);

        /// <summary>
        /// 异步Task`1拦截
        /// </summary>
        /// <typeparam name="TResult">返回参数</typeparam>
        /// <param name="invocation">IInvocation</param>
        void InterceptTaskAsynchronous<TResult>(IInvocation invocation);

        /// <summary>
        /// 异步ValueTask拦截
        /// </summary>
        /// <param name="invocation">IInvocation</param>
        void InterceptValueTaskAsynchronous(IInvocation invocation);

        /// <summary>
        /// 异步ValueTask`1拦截
        /// </summary>
        /// <typeparam name="TResult">返回参数</typeparam>
        /// <param name="invocation">IInvocation</param>
        void InterceptValueTaskAsynchronous<TResult>(IInvocation invocation);
    }
}
