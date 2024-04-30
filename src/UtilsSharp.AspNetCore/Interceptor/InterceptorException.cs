using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using Newtonsoft.Json;
using UtilsSharp.Logger;
using UtilsSharp.Shared.Standard;

namespace UtilsSharp.AspNetCore.Interceptor
{
    /// <summary>
    /// 拦截异常信息
    /// </summary>
    public class InterceptorException
    {
        #region 拦截异常信息
        /// <summary>
        /// 拦截异常信息
        /// </summary>
        /// <param name="invocation">IInvocation</param>
        /// <param name="ex">Exception</param>
        /// <param name="rules">Exception 拦截规则</param>
        public static void Interceptor(IInvocation invocation, Exception ex, List<ExceptionRegexRule> rules)
        {
            var result = Result(invocation, ex, rules);
            invocation.ReturnValue = result;
        }


        /// <summary>
        /// 拦截异常信息(异步带参数)
        /// </summary>
        /// <typeparam name="TResult">TResult</typeparam>
        /// <param name="invocation">IInvocation</param>
        /// <param name="ex">Exception</param>
        /// <param name="rules">Exception 拦截规则</param>
        /// <returns></returns>
        public static TResult InterceptorAsync<TResult>(IInvocation invocation, Exception ex, List<ExceptionRegexRule> rules)
        {
            var result = Result(invocation, ex, rules);
            return (TResult)result;
        }

        /// <summary>
        /// 获取错误信息
        /// </summary>
        /// <param name="invocation">IInvocation</param>
        /// <param name="ex">Exception</param>
        /// <param name="rules">Exception 拦截规则</param>
        /// <returns></returns>
        private static object Result(IInvocation invocation, Exception ex, List<ExceptionRegexRule> rules)
        {
            // 判断方法是否为异步方法
            MethodInfo methodInfo = invocation.Method;
            bool isAsyncMethod = IsAsyncMethod(methodInfo);
            //同步方法
            var returnType = invocation.Method.ReturnType;
            if (isAsyncMethod)
            {   //异步方法
                returnType = invocation.Method.ReturnType.GetGenericArguments().First();
            }
            var setErrorMethod = returnType.GetMethod("SetError", new Type[] { typeof(string), typeof(int) });
            var result = returnType != typeof(string) ? Activator.CreateInstance(returnType, true) : "";
            var methodName = invocation.InvocationTarget + "." + invocation.Method.Name;
            var args = new List<string>();
            if (invocation.Arguments != null && invocation.Arguments.Length > 0)
            {
                args.AddRange(from item in invocation.Arguments where item != null select JsonConvert.SerializeObject(item));
            }
            var param = string.Join(",", args);
            if (setErrorMethod == null)
            {
                LogHelper.Error($"执行：{methodName} 异常", ex, parameters: $"{param}", func: $"{methodName}");
            }
            else
            {
                var logId = LogHelper.Error($"{methodName} 异常", ex, parameters: $"{param}", func: $"{methodName}");
                var r = ex.Regex(logId, rules);
                setErrorMethod.Invoke(result, new object[] { r.Msg, r.Code });
            }
            return result;
        }

        /// <summary>
        /// 判断是否是异步方法
        /// </summary>
        /// <param name="methodInfo">方法信息</param>
        /// <returns></returns>
        private static bool IsAsyncMethod(MethodInfo methodInfo)
        {
            return typeof(System.Threading.Tasks.Task).IsAssignableFrom(methodInfo.ReturnType) ||
                   (methodInfo.ReturnType.IsGenericType &&
                    methodInfo.ReturnType.GetGenericTypeDefinition() == typeof(System.Threading.Tasks.Task<>));
        }


        #endregion
    }
}
