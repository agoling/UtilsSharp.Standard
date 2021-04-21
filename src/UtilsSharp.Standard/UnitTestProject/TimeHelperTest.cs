using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.VisualBasic;
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
            var dt = DateTime.Now;

            var w= TimeHelper.GetWeekOfYear(dt);

            var i = TimeHelper.GetUtcStartDate(dt);
            var i1 = TimeHelper.GetUtcEndDate(dt);

            var i3=dt.Date.ToUniversalTime();
            var i4=dt.Date.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(999).ToUniversalTime();


            var aa = TimeHelper.TimeStampToDateTime("1592537851689",TimeStampType.毫秒);

            var cc = DateTime.Now.AddDays(1);
            var expire = TimeHelper.GetTimeSpan(DateTime.Now,cc);
            var m = expire.TotalSeconds;

            var bb = TimeHelper.DateTimeToTimeStamp(DateTime.Now,TimeStampType.毫秒);

        }

       
        [TestMethod]
        public void bb()
        {

            var dt1 = TimeHelper.TimeStampToDateTime("1609603200000", TimeStampType.毫秒);
            var i = GetWeekOfYear(dt1);
            var dt2 = TimeHelper.TimeStampToDateTime("1608998400000", TimeStampType.毫秒);
            var i2 = GetWeekOfYear(dt2);
            var dt3 = TimeHelper.TimeStampToDateTime("1609603200000", TimeStampType.毫秒);
            var i3 = GetWeekOfYear(dt3);

        }

        /// <summary>
        /// 返回所在年的第几周
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public int GetWeekOfYear(DateTime datetime)
        {
            //声明存储结果的变量
            int intWeekOfYear = 0;
            //获取该年1月1日的日期
            DateTime dtFirstDay = new DateTime(datetime.Year, 1, 1);
            //目标日期距离该年第一天的天数
            int intDaysCount = Convert.ToInt32((datetime - dtFirstDay).TotalDays);
            //目标日期距离该年第一周第一天的天数（sunday为0，monday为1）
            intDaysCount += Convert.ToInt32(dtFirstDay.DayOfWeek);
            //目标日期所在的周(此处做上取整运算)
            intWeekOfYear = int.Parse(Math.Ceiling(intDaysCount / 7.0).ToString());
            //返回计算结果
            return intWeekOfYear;

        }
    }
}
