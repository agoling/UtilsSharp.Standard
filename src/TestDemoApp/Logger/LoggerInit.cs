using System;
using System.Collections.Generic;
using System.Text;
using UtilsSharp.Logger;

namespace TestDemoApp.Logger
{
    public class LoggerInit
    {


        public static void LoggerTest()
        {
            ConsoleHelper.Success("进来了!");
            LogHelper.Info("您好啊！");
            LogHelper.Info(new BaseLogEntity() {LogId = Guid.NewGuid().ToString("N"), Message = "Hello",DetailTrace = "Hello详情"});

        }


    }
}
