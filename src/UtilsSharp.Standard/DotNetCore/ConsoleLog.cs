using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore
{
    /// <summary>
    /// 控制台日志输出
    /// </summary>
    public class ConsoleLog
    {
        /// <summary>
        /// 当前消息总数
        /// </summary>
        private static int _currentMsgCount;

        /// <summary>
        /// 最大消息总数（达到这个数将清屏幕消息）
        /// </summary>
        public static int MaxMsgCount { set; get; } = -1;

        /// <summary>
        /// 信息(绿色)
        /// </summary>
        /// <param name="msg">消息</param>
        public static void Info(string msg)
        {
            ClearAndShow(() =>
            {
                Console.ForegroundColor = ConsoleColor.Green;
                msg = $"{msg} {DateTime.Now}";
                Console.WriteLine(msg);
            });
        }

        /// <summary>
        /// 调试(蓝色)
        /// </summary>
        /// <param name="msg">消息</param>
        public static void Debug(string msg)
        {
            ClearAndShow(() =>
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                msg = $"{msg} {DateTime.Now}";
                Console.WriteLine(msg);
            });
        }

        /// <summary>
        /// 提醒(黄色)
        /// </summary>
        /// <param name="msg">消息</param>
        public static void Warn(string msg)
        {
            ClearAndShow(() =>
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                msg = $"{msg} {DateTime.Now}";
                Console.WriteLine(msg);
            });
        }

        /// <summary>
        /// 错误(红色)
        /// </summary>
        /// <param name="msg">消息</param>
        public static void Error(string msg)
        {
            ClearAndShow(() =>
            {
                Console.ForegroundColor = ConsoleColor.Red;
                msg = $"{msg} {DateTime.Now}";
                Console.WriteLine(msg);
            });
        }

        /// <summary>
        /// 清理屏幕
        /// </summary>
        /// <param name="action">日志展示</param>
        private static void ClearAndShow(Action action)
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
            action?.Invoke();
        }
    }
}
