using System;
using System.Collections.Generic;
using System.Text;

namespace UtilsSharp
{
    /// <summary>
    /// 时间相关类
    /// </summary>
    public class TimeHelper
    {

        /// <summary>
        /// 时间格式化（小时分钟前面加上0）
        /// </summary>
        /// <param name="intStr">小时、分钟</param>
        /// <returns>"00"</returns>
        public static string TimeWithZero(string intStr)
        {

            try
            {
                return !string.IsNullOrEmpty(intStr) ? intStr.Length == 1 ? "0" + intStr : intStr : "00";
            }
            catch (Exception)
            {
                return "00";
            }
        }


        /// <summary>
        /// 时间格式化（小时分钟去掉前面的0）
        /// </summary>
        /// <param name="intStr">小时、分钟</param>
        /// <returns>0</returns>
        public static int TimeTrimZero(string intStr)
        {
            try
            {
                return !string.IsNullOrEmpty(intStr) ? Convert.ToInt32(intStr.StartsWith("0") && intStr.Length == 2 ? intStr.Substring(1) : intStr) : 0;
            }
            catch (Exception)
            {
                return 0;
            }

        }

        /// <summary>
        /// 返回某年某月最后一天
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="month">月份</param>
        /// <returns>日</returns>
        public static int GetMonthLastDate(int year, int month)
        {
            DateTime lastDay = new DateTime(year, month, new System.Globalization.GregorianCalendar().GetDaysInMonth(year, month));
            int day = lastDay.Day;
            return day;
        }

        /// <summary>
        /// 返回时间差
        /// </summary>
        /// <param name="time1">起始日期</param>
        /// <param name="time2">结束日期</param>
        /// <returns></returns>
        public static string GetDateDiff(DateTime time1, DateTime time2)
        {
            string dateDiff = null;
            try
            {
                TimeSpan ts = time2 - time1;
                if (ts.Days >= 1)
                {
                    dateDiff = time1.Month + "月" + time1.Day + "日";
                }
                else
                {
                    if (ts.Hours > 1)
                    {
                        dateDiff = ts.Hours + "小时前";
                    }
                    else
                    {
                        dateDiff = ts.Minutes + "分钟前";
                    }
                }
            }
            catch
            {
                // ignored
            }
            return dateDiff;
        }

        /// <summary>
        /// 获得两个日期的间隔
        /// </summary>
        /// <param name="time1">起始日期</param>
        /// <param name="time2">结束日期</param>
        /// <returns>日期间隔TimeSpan。</returns>
        public static TimeSpan GetTimeSpan(DateTime time1, DateTime time2)
        {
            TimeSpan ts1 = new TimeSpan(time1.Ticks);
            TimeSpan ts2 = new TimeSpan(time2.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            return ts;
        }

        /// <summary>
        /// 中文版指定一周的某天
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <returns></returns>
        public static string ChinaDayOfWeek(DateTime dateTime)
        {
            var dayOfWeek = dateTime.DayOfWeek;
            return ChinaDayOfWeek(dayOfWeek);
        }

        /// <summary>
        /// 中文版指定一周的某天
        /// </summary>
        /// <param name="dayOfWeek">指定一周的某天</param>
        /// <returns></returns>
        public static string ChinaDayOfWeek(DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    return "星期一";
                case DayOfWeek.Tuesday:
                    return "星期二";
                case DayOfWeek.Wednesday:
                    return "星期三";
                case DayOfWeek.Thursday:
                    return "星期四";
                case DayOfWeek.Friday:
                    return "星期五";
                case DayOfWeek.Saturday:
                    return "星期六";
                case DayOfWeek.Sunday:
                    return "星期日";
                default:
                    throw new Exception("参数有误");
            }
        }

    }
}
