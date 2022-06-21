using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;
using Newtonsoft.Json;
using UtilsSharp.Logger;
using UtilsSharp.Standard;

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
        public static void Interceptor(IInvocation invocation, Exception ex)
        {
            var returnType = invocation.Method.ReturnType;
            var setErrorMethod = returnType.GetMethod("SetError", new Type[] { typeof(string), typeof(int) });
            var result = Activator.CreateInstance(returnType, true);
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
                invocation.ReturnValue = result;
                return;
            }
            var errorCode = LogHelper.Error($"{methodName} 异常", ex, parameters: $"{param}", func: $"{methodName}");
            setErrorMethod.Invoke(result, new object[] { errorCode.ToMsgException(), 5000 });
            invocation.ReturnValue = result;
        }


        /// <summary>
        /// 拦截异常信息(异步带参数)
        /// </summary>
        /// <typeparam name="TResult">TResult</typeparam>
        /// <param name="invocation">IInvocation</param>
        /// <param name="ex">Exception</param>
        /// <returns></returns>
        public static TResult InterceptorAsync<TResult>(IInvocation invocation, Exception ex)
        {
            var returnType = invocation.Method.ReturnType.GetGenericArguments().First();
            var setErrorMethod = returnType.GetMethod("SetError", new Type[] { typeof(string), typeof(int) });
            var result = Activator.CreateInstance(returnType, true);
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
                return (TResult)result;
            }
            var errorCode = LogHelper.Error($"{methodName} 异常", ex, parameters: $"{param}", func: $"{methodName}");
            setErrorMethod.Invoke(result, new object[] { errorCode.ToMsgException(), 5000 });
            return (TResult)result;
        }
        #endregion
    }
}
