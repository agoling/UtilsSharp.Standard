using System;
using System.Collections.Generic;
using System.Threading;

namespace UtilsSharp
{
    /// <summary>
    /// 异常重试帮助类
    /// </summary>
    public static class RetryHelper
    {
        /// <summary>
        /// 重试零个参数无返回值的方法
        /// </summary>
        /// <param name="action">执行方法方法</param>
        /// <param name="retryInterval">重试间隔</param>
        /// <param name="retryCount">重试次数</param>
        public static void Execute(Action action, TimeSpan retryInterval, int retryCount = 3)
        {
            Execute<object>(() =>
            {
                action();
                return null;
            }, retryInterval, retryCount);
        }

        /// <summary>
        /// 重试一个参数无返回值的方法
        /// </summary>
        /// <typeparam name="T1">参数类型1</typeparam>
        /// <param name="action">执行方法方法</param>
        /// <param name="arg1">参数1</param>
        /// <param name="retryInterval">重试间隔</param>
        /// <param name="retryCount">重试次数</param>
        public static void Execute<T1>(Action<T1> action, T1 arg1, TimeSpan retryInterval, int retryCount = 3)
        {
            Execute<T1, object>((x1) =>
            {
                action(arg1);
                return null;
            }, arg1, retryInterval, retryCount);
        }

        /// <summary>
        /// 重试两个参数无返回值的方法
        /// </summary>
        /// <typeparam name="T1">参数类型1</typeparam>
        /// <typeparam name="T2">参数类型2</typeparam>
        /// <param name="action">执行方法方法</param>
        /// <param name="arg1">参数1</param>
        /// <param name="arg2">参数2</param>
        /// <param name="retryInterval">重试间隔</param>
        /// <param name="retryCount">重试次数</param>
        public static void Execute<T1, T2>(Action<T1, T2> action, T1 arg1, T2 arg2, TimeSpan retryInterval, int retryCount = 3)
        {
            Execute<T1, T2, object>((x1, x2) =>
            {
                action(arg1, arg2);
                return null;
            }, arg1, arg2, retryInterval, retryCount);
        }

        /// <summary>
        /// 重试三个参数无返回值的方法
        /// </summary>
        /// <typeparam name="T1">参数类型1</typeparam>
        /// <typeparam name="T2">参数类型2</typeparam>
        /// <typeparam name="T3">参数类型3</typeparam>
        /// <param name="action">执行方法方法</param>
        /// <param name="arg1">参数1</param>
        /// <param name="arg2">参数2</param>
        /// <param name="arg3">参数3</param>
        /// <param name="retryInterval">重试间隔</param>
        /// <param name="retryCount">重试次数</param>
        public static void Execute<T1, T2, T3>(Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3, TimeSpan retryInterval, int retryCount = 3)
        {
            Execute<T1, T2, T3, object>((x1, x2, x3) =>
            {
                action(arg1, arg2, arg3);
                return null;
            }, arg1, arg2, arg3, retryInterval, retryCount);
        }

        /// <summary>
        /// 重试四个参数无返回值的方法
        /// </summary>
        /// <typeparam name="T1">参数类型1</typeparam>
        /// <typeparam name="T2">参数类型2</typeparam>
        /// <typeparam name="T3">参数类型3</typeparam>
        /// <typeparam name="T4">参数类型4</typeparam>
        /// <param name="action">执行方法方法</param>
        /// <param name="arg1">参数1</param>
        /// <param name="arg2">参数2</param>
        /// <param name="arg3">参数3</param>
        /// <param name="arg4">参数4</param>
        /// <param name="retryInterval">重试间隔</param>
        /// <param name="retryCount">重试次数</param>
        public static void Execute<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, TimeSpan retryInterval, int retryCount = 3)
        {
            Execute<T1, T2, T3, T4, object>((x1, x2, x3, x4) =>
            {
                action(arg1, arg2, arg3, arg4);
                return null;
            }, arg1, arg2, arg3, arg4, retryInterval, retryCount);
        }

        /// <summary>
        /// 重试零个参数带返回值
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="func">执行的方法</param>
        /// <param name="retryInterval">重试间隔</param>
        /// <param name="retryCount">重试次数</param>
        /// <returns>返回类型T</returns>
        public static T Execute<T>(Func<T> func, TimeSpan retryInterval, int retryCount = 3)
        {
            var exceptions = new List<Exception>();

            for (int retry = 0; retry < retryCount; retry++)
            {
                try
                {
                    return func();
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                    Thread.Sleep(retryInterval);
                }
            }

            throw new AggregateException(exceptions);
        }

        /// <summary>
        /// 重试一个参数带返回值
        /// </summary>
        /// <typeparam name="T1">参数类型1</typeparam>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="func">执行的方法</param>
        /// <param name="arg1">参数1</param>
        /// <param name="retryInterval">重试间隔</param>
        /// <param name="retryCount">重试次数</param>
        /// <returns>返回类型T</returns>
        public static T Execute<T1, T>(Func<T1, T> func, T1 arg1, TimeSpan retryInterval, int retryCount = 3)
        {
            var exceptions = new List<Exception>();

            for (int retry = 0; retry < retryCount; retry++)
            {
                try
                {
                    return func(arg1);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                    Thread.Sleep(retryInterval);
                }
            }

            throw new AggregateException(exceptions);
        }

        /// <summary>
        /// 重试两个参数带返回值
        /// </summary>
        /// <typeparam name="T1">参数类型1</typeparam>
        /// <typeparam name="T2">参数类型2</typeparam>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="func">执行的方法</param>
        /// <param name="arg1">参数1</param>
        /// <param name="arg2">参数2</param>
        /// <param name="retryInterval">重试间隔</param>
        /// <param name="retryCount">重试次数</param>
        /// <returns>返回类型T</returns>
        public static T Execute<T1, T2, T>(Func<T1, T2, T> func, T1 arg1, T2 arg2, TimeSpan retryInterval, int retryCount = 3)
        {
            var exceptions = new List<Exception>();

            for (int retry = 0; retry < retryCount; retry++)
            {
                try
                {
                    return func(arg1, arg2);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                    Thread.Sleep(retryInterval);
                }
            }

            throw new AggregateException(exceptions);
        }

        /// <summary>
        /// 重试三个参数带返回值
        /// </summary>
        /// <typeparam name="T1">参数类型1</typeparam>
        /// <typeparam name="T2">参数类型2</typeparam>
        /// <typeparam name="T3">参数类型3</typeparam>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="func">执行的方法</param>
        /// <param name="arg1">参数1</param>
        /// <param name="arg2">参数2</param>
        /// <param name="arg3">参数3</param>
        /// <param name="retryInterval">重试间隔</param>
        /// <param name="retryCount">重试次数</param>
        /// <returns>返回类型T</returns>
        public static T Execute<T1, T2, T3, T>(Func<T1, T2, T3, T> func, T1 arg1, T2 arg2, T3 arg3, TimeSpan retryInterval, int retryCount = 3)
        {
            var exceptions = new List<Exception>();

            for (int retry = 0; retry < retryCount; retry++)
            {
                try
                {
                    return func(arg1, arg2, arg3);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                    Thread.Sleep(retryInterval);
                }
            }

            throw new AggregateException(exceptions);
        }

        /// <summary>
        /// 重试四个参数带返回值
        /// </summary>
        /// <typeparam name="T1">参数类型1</typeparam>
        /// <typeparam name="T2">参数类型2</typeparam>
        /// <typeparam name="T3">参数类型3</typeparam>
        /// <typeparam name="T4">参数类型4</typeparam>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="func">执行的方法</param>
        /// <param name="arg1">参数1</param>
        /// <param name="arg2">参数2</param>
        /// <param name="arg3">参数3</param>
        /// <param name="arg4">参数4</param>
        /// <param name="retryInterval">重试间隔</param>
        /// <param name="retryCount">重试次数</param>
        /// <returns>返回类型T</returns>
        public static T Execute<T1, T2, T3, T4, T>(Func<T1, T2, T3, T4, T> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, TimeSpan retryInterval, int retryCount = 3)
        {
            var exceptions = new List<Exception>();

            for (int retry = 0; retry < retryCount; retry++)
            {
                try
                {
                    return func(arg1, arg2, arg3, arg4);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                    Thread.Sleep(retryInterval);
                }
            }

            throw new AggregateException(exceptions);
        }

    }
}
