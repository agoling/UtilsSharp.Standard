using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UtilsSharp;

namespace UnitTestProject
{
    [TestClass]
    public class DingTalkRobotTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var content = "【日常提醒】测试提醒";
            var webhook ="https://xxxxxxxxxxx";
            var r = DingTalkRobot.SendTextMessage(webhook, content, null, false);
        }
    }
}
