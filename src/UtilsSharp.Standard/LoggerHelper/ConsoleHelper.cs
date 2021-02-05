using System;
using System.Collections.Generic;
using System.Text;

namespace LoggerHelper
{
    /// <summary>
    /// 控制台日志输出
    /// </summary>
    public class ConsoleHelper
    {
        /// <summary>
        /// 当前消息总数
        /// </summary>
        private static int _currentMsgCount;

        /// <summary>
        /// 最大消息总数（达到这个数后将清理屏幕消息）
        /// </summary>
        public static int MaxMsgCount { set; get; } = -1;

        /// <summary>
        /// 标题(深灰色)
        /// </summary>
        /// <param name="message">消息</param>
        public static void Title(string message)
        {
            ClearAndShow(obj=>
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                obj = $"{DateTime.Now:HH:mm:ss} {obj}";
                Console.WriteLine("----------------------------------------------------------------------");
                Console.WriteLine(obj);
                Console.WriteLine("----------------------------------------------------------------------");
            }, message);
        }

        /// <summary>
        /// 标题并写入日志(深灰色)
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="logId">日志Id</param>
        /// <param name="parameters">入参</param>
        /// <param name="userId">阿里Id</param>
        /// <param name="func">功能名称</param>
        /// <param name="requestUrl">请求地址</param>
        public static void TitleWithLog(string message, string logId = "", string parameters = "", string userId = "", string func = "", string requestUrl = "")
        {
            Title(message);
            LogHelper.Info(message, logId, parameters, userId, func, requestUrl);
        }

        /// <summary>
        /// 信息(灰色)
        /// </summary>
        /// <param name="message">消息</param>
        public static void Info(string message)
        {
            ClearAndShow(obj=>
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                obj = $"{DateTime.Now:HH:mm:ss} {obj}";
                Console.WriteLine(obj);
            }, message);
        }

        /// <summary>
        ///  信息并写入日志(灰色)
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <param name="logId">日志Id</param>
        /// <param name="parameters">入参</param>
        /// <param name="userId">阿里Id</param>
        /// <param name="func">功能名称</param>
        /// <param name="requestUrl">请求地址</param>
        public static void InfoWithLog(string message, string logId = "", string parameters = "", string userId = "", string func = "", string requestUrl = "")
        {
            Info(message);
            LogHelper.Info(message, logId, parameters, userId, func, requestUrl);
        }

        /// <summary>
        /// 追踪(深青色)
        /// </summary>
        /// <param name="message">消息</param>
        public static void Trace(string message)
        {
            ClearAndShow(obj =>
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                obj = $"{DateTime.Now:HH:mm:ss} {obj}";
                Console.WriteLine(obj);
            }, message);
        }

        /// <summary>
        ///  追踪并写入日志(深青色)
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <param name="logId">日志Id</param>
        /// <param name="parameters">入参</param>
        /// <param name="userId">阿里Id</param>
        /// <param name="func">功能名称</param>
        /// <param name="requestUrl">请求地址</param>
        public static void TraceWithLog(string message, string logId = "", string parameters = "", string userId = "", string func = "", string requestUrl = "")
        {
            Trace(message);
            LogHelper.Trace(message, logId, parameters, userId, func, requestUrl);
        }

        /// <summary>
        /// 调试(青色)
        /// </summary>
        /// <param name="message">消息</param>
        public static void Debug(string message)
        {
            ClearAndShow(obj=>
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                obj = $"{DateTime.Now:HH:mm:ss} {obj}";
                Console.WriteLine(obj);
            }, message);
        }

        /// <summary>
        ///  调试并写入日志(青色)
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <param name="logId">日志Id</param>
        /// <param name="parameters">入参</param>
        /// <param name="userId">阿里Id</param>
        /// <param name="func">功能名称</param>
        /// <param name="requestUrl">请求地址</param>
        public static void DebugWithLog(string message, string logId = "", string parameters = "", string userId = "", string func = "", string requestUrl = "")
        {
            Debug(message);
            LogHelper.Debug(message, logId, parameters, userId, func, requestUrl);
        }

        /// <summary>
        /// 警告(黄色)
        /// </summary>
        /// <param name="message">消息</param>
        public static void Warn(string message)
        {
            ClearAndShow(obj=>
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                obj = $"{DateTime.Now:HH:mm:ss} {obj}";
                Console.WriteLine(obj);
            }, message);
        }

        /// <summary>
        ///  警告并写入日志(黄色)
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <param name="logId">日志Id</param>
        /// <param name="parameters">入参</param>
        /// <param name="userId">阿里Id</param>
        /// <param name="func">功能名称</param>
        /// <param name="requestUrl">请求地址</param>
        public static void WarnWithLog(string message, string logId = "", string parameters = "", string userId = "", string func = "", string requestUrl = "")
        {
            Warn(message);
            LogHelper.Warn(message, logId, parameters, userId, func, requestUrl);
        }

        /// <summary>
        /// 错误(红色)
        /// </summary>
        /// <param name="message">消息</param>
        public static void Error(string message)
        {
            ClearAndShow(obj=>
            {
                Console.ForegroundColor = ConsoleColor.Red;
                obj = $"{DateTime.Now:HH:mm:ss} {obj}";
                Console.WriteLine(obj);
            }, message);
        }

        /// <summary>
        ///  错误并写入日志(红色)
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <param name="ex">Exception</param>
        /// <param name="logId">日志Id</param>
        /// <param name="errCode">错误码</param>
        /// <param name="parameters">入参</param>
        /// <param name="userId">阿里Id</param>
        /// <param name="func">功能名称</param>
        /// <param name="requestUrl">请求地址</param>
        public static void ErrorWithLog(string message, Exception ex, string logId = "", string errCode = "", string parameters = "", string userId = "", string func = "", string requestUrl = "")
        {
            Error(message);
            LogHelper.Error(message, ex, logId, errCode,parameters, userId, func, requestUrl);
        }

        /// <summary>
        /// 成功(绿色)
        /// </summary>
        /// <param name="message">消息</param>
        public static void Success(string message)
        {
            ClearAndShow(obj=>
            {
                Console.ForegroundColor = ConsoleColor.Green;
                obj = $"{DateTime.Now:HH:mm:ss} {obj}";
                Console.WriteLine(obj);
            }, message);
        }

        /// <summary>
        ///  成功并写入日志(绿色)
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <param name="logId">日志Id</param>
        /// <param name="parameters">入参</param>
        /// <param name="userId">阿里Id</param>
        /// <param name="func">功能名称</param>
        /// <param name="requestUrl">请求地址</param>
        public static void SuccessWithLog(string message, string logId = "", string parameters = "", string userId = "", string func = "", string requestUrl = "")
        {
            Success(message);
            LogHelper.Info(message, logId, parameters, userId, func, requestUrl);
        }

        /// <summary>
        /// 清理屏幕
        /// </summary>
        /// <param name="action">日志展示</param>
        /// <param name="message">消息</param>
        private static void ClearAndShow(Action<string> action, string message)
        {
            if (MaxMsgCount > 0)
            {
                if (_currentMsgCount > MaxMsgCount)
                {
                    Console.Clear();
                    _currentMsgCount = 0;
                }
                else
                {
                    _currentMsgCount += 1;
                }
            }
            action?.Invoke(message);
        }
    }
}
