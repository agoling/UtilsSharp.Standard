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
            Console.ReadKey();
        }
    }
}
