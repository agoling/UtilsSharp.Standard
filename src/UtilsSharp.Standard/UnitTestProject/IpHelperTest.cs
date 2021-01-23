using System;
using System.Collections.Generic;
using System.Text;
using DotNetCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UtilsSharp;

namespace UnitTestProjectNetCore
{
    [TestClass]
    public class IpHelperTest
    {

        [TestMethod]
        public void GetIpInfoTest()
        {
            var aa = IpHelper.GetClientIp();
            var bb = IpHelper.GetIpInfo("218.86.19.12");
        }
    }
}
