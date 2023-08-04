using System;
using System.Collections.Generic;
using System.Text;
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

        #region 生成随机汉字

        /// <summary>
        /// 生成随机汉字
        /// </summary>
        /// <param name="length">生成长度,默认10</param>
        /// <param name="encoding">Encoding</param>
        /// <returns></returns>
        public static string ChineseCharacters(int length = 10, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            StringHelper.EncodingRegister();
            Random random = new Random();
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                // 生成随机汉字的Unicode编码范围为0x4E00到0x9FBF
                int codePoint = random.Next(0x4E00, 0x9FBF + 1);

                // 将Unicode编码转换为对应的汉字字符
                string character = char.ConvertFromUtf32(codePoint);

                // 将汉字字符转换为UTF-8字节序列
                byte[] utf8Bytes = encoding.GetBytes(character);

                // 将UTF-8字节序列转换为字符串并拼接
                string utf8String = encoding.GetString(utf8Bytes);
                stringBuilder.Append(utf8String);
            }

            string chineseCharacters = stringBuilder.ToString();
            return chineseCharacters;
        }


        /// <summary>
        /// 生成随机汉字(简体字)
        /// </summary>
        /// <param name="length">生成长度,默认10</param>
        /// <returns></returns>
        public static string SimpleChineseCharacters(int length = 10)
        {
            StringHelper.EncodingRegister();
            var encoding = Encoding.GetEncoding("gb2312");
            Random rnd = new Random();
            StringBuilder stringBuilder = new StringBuilder();

            //定义一个字符串数组储存汉字编码的组成元素
            string[] rBase = new String[16] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f" };

            /**//*每循环一次产生一个含两个元素的十六进制字节数组，并将其放入bject数组中
             每个汉字有四个区位码组成
             区位码第1位和区位码第2位作为字节数组第一个元素
             区位码第3位和区位码第4位作为字节数组第二个元素
            */
            for (int i = 0; i < length; i++)
            {
                //区位码第1位
                int r1 = rnd.Next(11, 14);
                string str_r1 = rBase[r1].Trim();

                //区位码第2位
                rnd = new Random(r1 * unchecked((int)DateTime.Now.Ticks) + i);//更换随机数发生器的

                //种子避免产生重复值
                int r2;
                if (r1 == 13)
                {
                    r2 = rnd.Next(0, 7);
                }
                else
                {
                    r2 = rnd.Next(0, 16);
                }
                string str_r2 = rBase[r2].Trim();

                //区位码第3位
                rnd = new Random(r2 * unchecked((int)DateTime.Now.Ticks) + i);
                int r3 = rnd.Next(10, 16);
                string str_r3 = rBase[r3].Trim();

                //区位码第4位
                rnd = new Random(r3 * unchecked((int)DateTime.Now.Ticks) + i);
                int r4;
                if (r3 == 10)
                {
                    r4 = rnd.Next(1, 16);
                }
                else if (r3 == 15)
                {
                    r4 = rnd.Next(0, 15);
                }
                else
                {
                    r4 = rnd.Next(0, 16);
                }
                string str_r4 = rBase[r4].Trim();

                //定义两个字节变量存储产生的随机汉字区位码
                byte byte1 = Convert.ToByte(str_r1 + str_r2, 16);
                byte byte2 = Convert.ToByte(str_r3 + str_r4, 16);
                //将两个字节变量存储在字节数组中
                byte[] str_r = new byte[] { byte1, byte2 };

                var str=encoding.GetString((byte[])Convert.ChangeType(str_r, typeof(byte[])));

                stringBuilder.Append(str);

            }
            string chineseCharacters = stringBuilder.ToString();
            return chineseCharacters;
        }


        #endregion
    }
}
