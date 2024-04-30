using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using UtilsSharp.AsyncInterceptor;
using UtilsSharp.Shared.Standard;

namespace UtilsSharp.AspNetCore.Interceptor
{
    /// <summary>
    /// 日志异步拦截
    /// </summary>
    public class LoggerAsyncInterceptor : IAsyncInterceptor
    {
        /// <summary>
        /// Exception 匹配规则
        /// </summary>
        public virtual List<ExceptionRegexRule> ExceptionRegexRule { set; get; } = BaseException.GetDefaultRegexRule();

        #region 同步方法拦截时使用
        /// <summary>
        /// 同步方法拦截时使用
        /// </summary>
        /// <param name="invocation"></param>
        public virtual void InterceptSynchronous(IInvocation invocation)
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
        #endregion

        #region 异步方法返回Task拦截时使用

        /// <summary>
        /// 异步方法返回Task拦截时使用
        /// </summary>
        /// <param name="invocation">IInvocation</param>
        public virtual void InterceptTaskAsynchronous(IInvocation invocation)
        {
            //调用业务方法
            invocation.ReturnValue = InternalInterceptTaskAsynchronous(invocation);
        }

        /// <summary>
        /// 异步方法返回Task`1拦截时使用 私有方法
        /// </summary>
        /// <param name="invocation">IInvocation</param>
        /// <returns></returns>
        protected virtual async Task InternalInterceptTaskAsynchronous(IInvocation invocation)
        {
            try
            {
                //获取执行信息
                invocation.Proceed();
                var task = (Task)invocation.ReturnValue;
                await task;
                //记录日志
            }
            catch (Exception ex)
            {
                InterceptorException.Interceptor(invocation, ex, ExceptionRegexRule);
            }
        }

        #endregion

        #region 异步方法返回Task<TResult>拦截时使用

        /// <summary>
        /// 异步方法返回Task`1拦截时使用
        /// </summary>
        /// <typeparam name="TResult">返回参数</typeparam>
        /// <param name="invocation">IInvocation</param>
        public virtual void InterceptTaskAsynchronous<TResult>(IInvocation invocation)
        {
            //调用业务方法
            invocation.ReturnValue = InternalInterceptTaskAsynchronous<TResult>(invocation);
        }

        /// <summary>
        /// 异步方法返回Task`1拦截时使用 私有方法
        /// </summary>
        /// <typeparam name="TResult">返回参数</typeparam>
        /// <param name="invocation">IInvocation</param>
        /// <returns></returns>
        protected virtual async Task<TResult> InternalInterceptTaskAsynchronous<TResult>(IInvocation invocation)
        {
            try
            {
                //获取执行信息
                invocation.Proceed();
                var task = (Task<TResult>)invocation.ReturnValue;
                TResult result = await task;
                //记录日志
                return result;
            }
            catch (Exception ex)
            {
                return InterceptorException.InterceptorAsync<TResult>(invocation, ex, ExceptionRegexRule);
            }
        }

        #endregion

        #region 异步方法返回ValueTask拦截时使用

        /// <summary>
        /// 异步方法返回ValueTask拦截时使用
        /// </summary>
        /// <param name="invocation">IInvocation</param>
        public virtual void InterceptValueTaskAsynchronous(IInvocation invocation)
        {
            //调用业务方法
            invocation.ReturnValue = InternalInterceptValueTaskAsynchronous(invocation);
        }

        /// <summary>
        /// 异步方法返回ValueTask拦截时使用 私有方法
        /// </summary>
        /// <param name="invocation">IInvocation</param>
        /// <returns></returns>
        protected virtual async ValueTask InternalInterceptValueTaskAsynchronous(IInvocation invocation)
        {
            try
            {
                //获取执行信息
                invocation.Proceed();
                var task = (ValueTask)invocation.ReturnValue;
                await task;
                //记录日志
            }
            catch (Exception ex)
            {
                InterceptorException.Interceptor(invocation, ex, ExceptionRegexRule);
            }
        }

        #endregion

        #region 异步方法返回ValueTask<TResult>拦截时使用

        /// <summary>
        /// 异步方法返回ValueTask`1拦截时使用
        /// </summary>
        /// <typeparam name="TResult">返回参数</typeparam>
        /// <param name="invocation">IInvocation</param>
        public virtual void InterceptValueTaskAsynchronous<TResult>(IInvocation invocation)
        {
            //调用业务方法
            invocation.ReturnValue = InternalInterceptValueTaskAsynchronous<TResult>(invocation);
        }

        /// <summary>
        /// 异步方法返回ValueTask`1拦截时使用 私有方法
        /// </summary>
        /// <typeparam name="TResult">返回参数</typeparam>
        /// <param name="invocation">IInvocation</param>
        /// <returns></returns>
        protected virtual async ValueTask<TResult> InternalInterceptValueTaskAsynchronous<TResult>(IInvocation invocation)
        {
            try
            {
                //获取执行信息
                invocation.Proceed();
                var task = (ValueTask<TResult>)invocation.ReturnValue;
                TResult result = await task;
                //记录日志
                return result;
            }
            catch (Exception ex)
            {
                return InterceptorException.InterceptorAsync<TResult>(invocation, ex, ExceptionRegexRule);
            }

        }

        #endregion

    }
}
