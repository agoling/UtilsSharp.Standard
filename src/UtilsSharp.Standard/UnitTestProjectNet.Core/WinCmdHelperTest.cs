using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UtilsSharp;

namespace UnitTestProjectNet.Core
{
    [TestClass]
    public class WinCmdHelperTest
    {
        [TestMethod]
        public void RunCommandTest()
        {
            var result= WinCmdHelper.RunCommand("help", true, true);
        }
    }
}
