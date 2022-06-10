using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UtilsSharp;

namespace UnitTestProjectNet.Core
{
    /// <summary>
    /// 中国日历帮助类
    /// </summary>
    [TestClass]
    public class ChineseCalendarTest
    {
        /// <summary>
        /// 公历
        /// </summary>
        [TestMethod]
        public void GongLi()
        {
            //调用:
            ChineseCalendar gc = new ChineseCalendar(new DateTime(1990, 01, 15));
            StringBuilder dayInfo = new StringBuilder();
            dayInfo.Append("阳历：" + gc.DateString + "\r\n");
            dayInfo.Append("农历：" + gc.ChineseDateString + "\r\n");
            dayInfo.Append("星期：" + gc.WeekDayStr);
            dayInfo.Append("时辰：" + gc.ChineseHour + "\r\n");
            dayInfo.Append("属相：" + gc.AnimalString + "\r\n");
            dayInfo.Append("节气：" + gc.ChineseTwentyFourDay + "\r\n");
            dayInfo.Append("前一个节气：" + gc.ChineseTwentyFourPrevDay + "\r\n");
            dayInfo.Append("下一个节气：" + gc.ChineseTwentyFourNextDay + "\r\n");
            dayInfo.Append("节日：" + gc.DateHoliday + "\r\n");
            dayInfo.Append("干支：" + gc.GanZhiDateString + "\r\n");
            dayInfo.Append("星宿：" + gc.ChineseConstellation + "\r\n");
            dayInfo.Append("星座：" + gc.Constellation + "\r\n");
        }


        /// <summary>
        /// 公历
        /// </summary>
        [TestMethod]
        public void NongLi()
        {
            //调用:
            ChineseCalendar nc = new ChineseCalendar(1990,1,15,false);
            StringBuilder dayInfo = new StringBuilder();
            dayInfo.Append("阳历：" + nc.DateString + "\r\n");
            dayInfo.Append("农历：" + nc.ChineseDateString + "\r\n");
            dayInfo.Append("星期：" + nc.WeekDayStr);
            dayInfo.Append("时辰：" + nc.ChineseHour + "\r\n");
            dayInfo.Append("属相：" + nc.AnimalString + "\r\n");
            dayInfo.Append("节气：" + nc.ChineseTwentyFourDay + "\r\n");
            dayInfo.Append("前一个节气：" + nc.ChineseTwentyFourPrevDay + "\r\n");
            dayInfo.Append("下一个节气：" + nc.ChineseTwentyFourNextDay + "\r\n");
            dayInfo.Append("节日：" + nc.DateHoliday + "\r\n");
            dayInfo.Append("干支：" + nc.GanZhiDateString + "\r\n");
            dayInfo.Append("星宿：" + nc.ChineseConstellation + "\r\n");
            dayInfo.Append("星座：" + nc.Constellation + "\r\n");


        }

    }
}
