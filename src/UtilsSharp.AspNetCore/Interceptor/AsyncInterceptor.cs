using System;
using System.Collections.Generic;
using System.Text;
using Castle.DynamicProxy;
using UtilsSharp.AsyncInterceptor;

namespace UtilsSharp.AspNetCore.Interceptor
{
    /// <summary>
    /// 异步拦截
    /// </summary>
    /// <typeparam name="TAsyncInterceptor">异步拦截服务</typeparam>
    public class AsyncInterceptor<TAsyncInterceptor> : IInterceptor where TAsyncInterceptor : IAsyncInterceptor
    {

        private readonly TAsyncInterceptor _t;

        /// <summary>
        /// 异步拦截
        /// </summary>
        public AsyncInterceptor(TAsyncInterceptor t)
        {
            _t = t;
        }

        /// <summary>
        /// 拦截主方法
        /// </summary>
        /// <param name="invocation"></param>
        public void Intercept(IInvocation invocation)
        {
            _t.ToInterceptor().Intercept(invocation);
        }

    }
}
