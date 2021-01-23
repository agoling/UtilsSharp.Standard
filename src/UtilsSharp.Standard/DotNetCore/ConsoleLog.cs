using System;

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
        /// 最大消息总数（达到这个数后将清理屏幕消息）
        /// </summary>
        public static int MaxMsgCount { set; get; } = -1;

        /// <summary>
        /// 标题(灰色)
        /// </summary>
        /// <param name="msg">消息</param>
        public static void Title(string msg)
        {
            ClearAndShow((obj) =>
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                obj = $"{DateTime.Now:HH:mm:ss} {obj}";
                Console.WriteLine("----------------------------------------------------------------------");
                Console.WriteLine(obj);
                Console.WriteLine("----------------------------------------------------------------------");
            },msg);
        }

        /// <summary>
        /// 信息(灰色)
        /// </summary>
        /// <param name="msg">消息</param>
        public static void Info(string msg)
        {
            ClearAndShow((obj) =>
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                obj = $"{DateTime.Now:HH:mm:ss} {obj}";
                Console.WriteLine(obj);
            },msg);
        }

        /// <summary>
        /// 调试(青色)
        /// </summary>
        /// <param name="msg">消息</param>
        public static void Debug(string msg)
        {
            ClearAndShow((obj) =>
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                obj = $"{DateTime.Now:HH:mm:ss} {obj}";
                Console.WriteLine(obj);
            },msg);
        }

        /// <summary>
        /// 提醒(黄色)
        /// </summary>
        /// <param name="msg">消息</param>
        public static void Warn(string msg)
        {
            ClearAndShow((obj) =>
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                obj = $"{DateTime.Now:HH:mm:ss} {obj}";
                Console.WriteLine(obj);
            },msg);
        }

        /// <summary>
        /// 错误(红色)
        /// </summary>
        /// <param name="msg">消息</param>
        public static void Error(string msg)
        {
            ClearAndShow((obj) =>
            {
                Console.ForegroundColor = ConsoleColor.Red;
                obj = $"{DateTime.Now:HH:mm:ss} {obj}";
                Console.WriteLine(obj);
            },msg);
        }

        /// <summary>
        /// 成功(绿色)
        /// </summary>
        /// <param name="msg">消息</param>
        public static void Success(string msg)
        {
            ClearAndShow((obj) =>
            {
                Console.ForegroundColor = ConsoleColor.Green;
                obj = $"{DateTime.Now:HH:mm:ss} {obj}";
                Console.WriteLine(obj);
            },msg);
        }

        /// <summary>
        /// 清理屏幕
        /// </summary>
        /// <param name="action">日志展示</param>
        /// <param name="msg">消息</param>
        private static void ClearAndShow(Action<string> action,string msg)
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
            action?.Invoke(msg);
        }
    }

}
