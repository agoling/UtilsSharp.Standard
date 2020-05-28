using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UtilsSharp;

namespace UnitTestProjectNet
{
    [TestClass]
    public class DingTalkRobotTest
    {
        [TestMethod]
        public void SendTextMessage()
        {
            var content = "【日常提醒】测试提醒";
            var webhook = "https://oapi.dingtalk.com/robot/send?access_token=908ebb34b51208e42319d55bee8e08fa7b3fa20ef269e6095dbf6b35cb54b757";
            var r = DingTalkRobot.SendTextMessage(webhook, content, null, false);
        }

        [TestMethod]
        public void SendTextMessage1()
        {
            var aa = AppsettingsHelper.GetValue("DingTalkWebhook");
            
        }
    }
}
