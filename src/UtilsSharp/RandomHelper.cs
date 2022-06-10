using System;
using System.Collections.Generic;
using System.Threading;
using static System.Int32;

namespace UtilsSharp
{
    /// <summary>
    /// 随机相关帮助类
    /// </summary>
    public class RandomHelper
    {
        #region 生成随机数字

        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="length">生成长度</param>
        public static string Number(int length)
        {
            return Number(length, false);
        }

        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="length">生成长度</param>
        /// <param name="sleep">是否要在生成前将当前线程阻止以避免重复</param>
        public static string Number(int length, bool sleep)
        {
            if (sleep) Thread.Sleep(3);
            var result = "";
            var random = new Random();
            for (int i = 0; i < length; i++)
            {
                result += random.Next(10).ToString();
            }

            return result;
        }

        #endregion

        #region 生成随机数字与字母

        /// <summary>
        /// 生成随机数字与字母
        /// </summary>
        /// <param name="length">生成长度</param>
        public static string NumberAndLetters(int length)
        {
            return NumberAndLetters(length, false);
        }

        /// <summary>
        /// 生成随机数字与字母
        /// </summary>
        /// <param name="length">生成长度</param>
        /// <param name="sleep">是否要在生成前将当前线程阻止以避免重复</param>
        public static string NumberAndLetters(int length, bool sleep)
        {
            if (sleep) Thread.Sleep(3);
            char[] pattern = {
                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K',
                'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
            };
            string result = "";
            int n = pattern.Length;
            Random random = new Random(~unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < length; i++)
            {
                int rnd = random.Next(0, n);
                result += pattern[rnd];
            }

            return result;
        }

        #endregion

        #region 生成随机字母(只有字母)

        /// <summary>
        /// 生成随机字母(只有字母)
        /// </summary>
        /// <param name="length">生成长度</param>
        public static string Letters(int length)
        {
            return Letters(length, false);
        }

        /// <summary>
        /// 生成随机字母(只有字母)
        /// </summary>
        /// <param name="length">生成长度</param>
        /// <param name="sleep">是否要在生成前将当前线程阻止以避免重复</param>
        public static string Letters(int length, bool sleep)
        {
            if (sleep) Thread.Sleep(3);
            char[] pattern = {
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U',
                'V', 'W', 'X', 'Y', 'Z'
            };
            string result = "";
            int n = pattern.Length;
            var random = new Random(~unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < length; i++)
            {
                int rnd = random.Next(0, n);
                result += pattern[rnd];
            }

            return result;
        }

        #endregion

        #region 随机排序list数据

        /// <summary>
        ///  随机排序list数据
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public static List<T> ListData<T>(List<T> data)
        {
            if (data == null || data.Count == 0)
            {
                return data;
            }
            var randomData = new List<T>();
            var indexes = NumbersNoRepeating(0, data.Count - 1);
            foreach (var index in indexes)
            {
                randomData.Add(data[index]);
            }
            return randomData;
        }


        #endregion

        #region 生成不重复的随机数值

        /// <summary>
        /// 生成不重复的随机数值
        /// </summary>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        /// <returns></returns>
        public static int[] NumbersNoRepeating(int minValue, int maxValue)
        {
            if (minValue > maxValue)
            {
                throw new Exception("最小值minValue不能大于最大值maxValue");
            }
            int length = maxValue - minValue + 1;
            int seed = Guid.NewGuid().GetHashCode();
            Random random = new Random(seed);
            int[] data = new int[length];
            int index = 0;
            for (int i = minValue; i <= maxValue; i++)
            {
                data[index] = i;
                index++;
            }
            int[] array = new int[length]; // 用来保存随机生成的不重复的数 
            int site = length;             // 设置上限 
            for (int j = 0; j < length; j++)
            {
                int idx = random.Next(0, site - 1);
                array[j] = data[idx];          // 在随机索引位置取出一个数，保存到结果数组 
                data[idx] = data[site - 1];   // 作废当前索引位置数据，并用数组的最后一个数据代替之
                site--;                         // 索引位置的上限减一（弃置最后一个数据）
            }
            return array;
        }

        #endregion

        #region 生成随机时间

        /// <summary>
        /// 生成随机时间
        /// </summary>
        /// <param name="time1">起始时间</param>
        /// <param name="time2">结束时间</param>
        /// <returns>间隔时间之间的 随机时间</returns>
        public static DateTime Time(DateTime time1, DateTime time2)
        {
            Random random = new Random();
            DateTime minTime;

            TimeSpan ts = new TimeSpan(time1.Ticks - time2.Ticks);

            // 获取两个时间相隔的秒数
            double dTotalSeconds = ts.TotalSeconds;
            int iTotalSeconds;

            if (dTotalSeconds > MaxValue)
            {
                iTotalSeconds = MaxValue;
            }
            else if (dTotalSeconds < MinValue)
            {
                iTotalSeconds = MinValue;
            }
            else
            {
                iTotalSeconds = (int)dTotalSeconds;
            }
            if (iTotalSeconds > 0)
            {
                minTime = time2;
            }
            else if (iTotalSeconds < 0)
            {
                minTime = time1;
            }
            else
            {
                return time1;
            }
            int maxValue = iTotalSeconds;

            if (iTotalSeconds <= MinValue)
                maxValue = MinValue + 1;

            int i = random.Next(Math.Abs(maxValue));

            return minTime.AddSeconds(i);
        }

        #endregion
    }
}
