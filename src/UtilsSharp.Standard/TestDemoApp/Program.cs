using System;
using System.Threading;
using System.Threading.Tasks;
using DotNetCore;
using LoggerHelper;

namespace TestDemoApp
{
    class Program
    {
        static void Main(string[] args)
        {

            Logger.Trace("您好","123","canshu","","gongneng","http://www.baidu.com");
            Logger.Info("您好", "123", "canshu", "", "gongneng", "http://www.baidu.com");
            Logger.Error("您好",new Exception("wocuole"),"123");

            ConsoleLog.Title("我是Title！");
            ConsoleLog.Info("我是Info1！");
            ConsoleLog.Info("我是Info2！");
            ConsoleLog.Info("我是Info3！");
            ConsoleLog.Debug("我是Debug！");
            ConsoleLog.Error("我是Error！");
            ConsoleLog.Success("我是Success！");
            ConsoleLog.Warn("我是Warn！");
            Console.ReadKey();
        }
    }
}
