﻿using System;
using System.Globalization;
using System.Linq;

namespace UtilsSharp
{
    /// <summary>
    /// 时间相关类
    /// </summary>
    public class TimeHelper
    {
        /// <summary>
        /// 获取开始日期(UTC)
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <returns></returns>
        public static DateTime GetUtcStartDate(DateTime dateTime)
        {
            return GetStartDate(dateTime).ToUniversalTime();
        }

        /// <summary>
        /// 获取开始日期
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <returns></returns>
        public static DateTime GetStartDate(DateTime dateTime)
        {
            return dateTime.Date;
        }

        /// <summary>
        /// 获取结束日期(UTC)
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <returns></returns>
        public static DateTime GetUtcEndDate(DateTime dateTime)
        {
            return GetEndDate(dateTime).ToUniversalTime();
        }

        /// <summary>
        /// 获取结束日期
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <returns></returns>
        public static DateTime GetEndDate(DateTime dateTime)
        {
            return dateTime.Date.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(999);
        }

        /// <summary>
        /// 时间格式化(小时分钟前面加上0)
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
        /// 时间格式化(小时分钟去掉前面的0)
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
            DateTime lastDay = new DateTime(year, month, new GregorianCalendar().GetDaysInMonth(year, month));
            int day = lastDay.Day;
            return day;
        }

        /// <summary>
        /// 获取时间差字符串
        /// </summary>
        /// <param name="time1">起始时间</param>
        /// <param name="time2">结束时间</param>
        /// <returns></returns>
        public static string GetDateDiff(DateTime time1, DateTime time2)
        {
            string dateDiff = null;
            try
            {
                if (time1 > time2)
                {
                    throw new Exception("起始时间不能大于结束时间");
                }

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
                        if (ts.Minutes > 1)
                        {
                            dateDiff = ts.Minutes + "分钟前";
                        }
                        else
                        {
                            dateDiff = ts.Seconds + "秒前";
                        }
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

        /// <summary>
        /// 时间戳转DateTime
        /// </summary>
        /// <param name="timeStamp">时间戳</param>
        /// <param name="type">类型：秒，毫秒</param>
        /// <returns></returns>
        public static DateTime TimeStampToDateTime(string timeStamp, TimeStampType type= TimeStampType.秒)
        {
            var startTime = TimeZoneInfo.ConvertTimeFromUtc(new DateTime(1970, 1, 1), TimeZoneInfo.Local).ToLocalTime();
            var time = long.Parse(timeStamp);
            return type switch
            {
                TimeStampType.秒 => startTime.AddSeconds(time),
                TimeStampType.毫秒 => startTime.AddMilliseconds(time),
                _ => startTime.AddSeconds(time)
            };
        }

        /// <summary>
        /// DateTime转时间戳
        /// </summary>
        /// <param name="dateTime">DateTime</param>
        /// <param name="type">类型：秒，毫秒</param>
        /// <returns></returns>
        public static long DateTimeToTimeStamp(DateTime dateTime, TimeStampType type = TimeStampType.秒)
        {
            var startTime = TimeZoneInfo.ConvertTimeFromUtc(new DateTime(1970, 1, 1), TimeZoneInfo.Local).ToLocalTime();
            return type switch
            {
                TimeStampType.秒 => (long)(dateTime - startTime).TotalSeconds,
                TimeStampType.毫秒 => (long)(dateTime - startTime).TotalMilliseconds,
                _ => (long)(dateTime - startTime).TotalSeconds
            };
        }

        /// <summary>
        /// 获取指定日期，在为一年中为第几周
        /// </summary>
        /// <param name="dt">指定时间</param>
        /// <reutrn>返回第几周</reutrn>
        public static int GetWeekOfYear(DateTime dt)
        {
            var gc = new GregorianCalendar();
            var weekOfYear = gc.GetWeekOfYear(dt, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            return weekOfYear;
        }

        /// <summary>
        /// 当前时间是否周末
        /// </summary>
        /// <param name="dateTime">时间点</param>
        /// <returns></returns>
        public static bool IsWeekend(DateTime dateTime)
        {
            DayOfWeek[] weeks = { DayOfWeek.Saturday, DayOfWeek.Sunday };
            return weeks.Contains(dateTime.DayOfWeek);
        }

        /// <summary>
        /// 当前时间是否工作日
        /// </summary>
        /// <param name="dateTime">时间点</param>
        /// <returns></returns>
        public static bool IsWeekday(DateTime dateTime)
        {
            DayOfWeek[] weeks = { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday };
            return weeks.Contains(dateTime.DayOfWeek);
        }
    }

    /// <summary>
    /// 时间戳类型
    /// </summary>
    public enum TimeStampType
    {
        /// <summary>
        /// 秒类型
        /// </summary>
        秒=0,
        /// <summary>
        /// 毫秒类型
        /// </summary>
        毫秒=1
    }
}
