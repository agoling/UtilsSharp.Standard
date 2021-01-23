using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UtilsSharp;

namespace UnitTestProjectNetCore
{
    [TestClass]
    public class TimeHelperTest
    {

        [TestMethod]
        public void SendTextMessage1()
        {
            var aa = TimeHelper.TimeStampToDateTime("1592537851689",TimeStampType.毫秒);

            var cc = DateTime.Now.AddDays(1);
            var expire = TimeHelper.GetTimeSpan(DateTime.Now,cc);
            var m = expire.TotalSeconds;

            var bb = TimeHelper.DateTimeToTimeStamp(DateTime.Now,TimeStampType.毫秒);

        }
    }
}
