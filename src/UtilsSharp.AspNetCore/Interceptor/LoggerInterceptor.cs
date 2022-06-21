using Castle.DynamicProxy;
using System;

namespace UtilsSharp.AspNetCore.Interceptor
{
    /// <summary>
    /// 日志拦截器
    /// </summary>
    public class LoggerInterceptor : IInterceptor
    {
        /// <summary>
        /// Aop拦截
        /// </summary>
        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
            }
            catch (Exception ex)
            {
                InterceptorException.Interceptor(invocation, ex);
            }
        }
    }
}
