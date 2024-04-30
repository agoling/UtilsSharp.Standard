using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using UtilsSharp.Shared.Standard;

namespace UtilsSharp.AspNetCore.Interceptor
{
    /// <summary>
    /// 日志拦截器
    /// </summary>
    public class LoggerInterceptor : IInterceptor
    {
        /// <summary>
        /// Exception 匹配规则
        /// </summary>
        public virtual List<ExceptionRegexRule> ExceptionRegexRule { set; get; } = BaseException.GetDefaultRegexRule();


        /// <summary>
        /// Aop拦截
        /// </summary>
        public virtual void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
            }
            catch (Exception ex)
            {
                InterceptorException.Interceptor(invocation, ex, ExceptionRegexRule);
            }
        }
    }
}
