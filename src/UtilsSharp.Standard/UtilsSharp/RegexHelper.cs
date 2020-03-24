using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace UtilsSharp
{
    /// <summary>
    /// 正则匹配帮助类
    /// </summary>
    public class RegexHelper
    {
        /// <summary>
        /// 正则是否匹配
        /// </summary>
        /// <param name="input">字符串</param>
        /// <param name="pattern">正则规则</param>
        /// <returns></returns>
        public static bool IsMatch(string input,string pattern)
        {
            var match = Regex.Match(input, pattern);
            return match.Success;
        }

        /// <summary>
        /// 正则是否匹配
        /// </summary>
        /// <param name="input">字符串</param>
        /// <param name="pattern">正则规则</param>
        /// <param name="options">提供用于设置正则表达式选项的枚举值</param>
        /// <returns></returns>
        public static bool IsMatch(string input, string pattern,RegexOptions options)
        {
            var match = Regex.Match(input, pattern,options);
            return match.Success;
        }

        /// <summary>
        /// 从指定字符串中过滤出符合正则匹配的字符串
        /// </summary>
        /// <param name="input">字符串</param>
        /// <param name="pattern">正则规则</param>
        /// <returns></returns>
        public static GroupCollection Match(string input, string pattern)
        {
            var match = Regex.Match(input, pattern);
            return match.Success ? match.Groups :default;
        }

        /// <summary>
        /// 从指定字符串中过滤出符合正则匹配的字符串
        /// </summary>
        /// <param name="input">字符串</param>
        /// <param name="pattern">正则规则</param>
        /// <param name="options">提供用于设置正则表达式选项的枚举值</param>
        /// <returns></returns>
        public static GroupCollection Match(string input, string pattern, RegexOptions options)
        {
            var match = Regex.Match(input, pattern, options);
            return match.Success ? match.Groups : default;
        }

        /// <summary>
        /// 从指定字符串中过滤出所有符合正则匹配的子集
        /// </summary>
        /// <param name="input">字符串</param>
        /// <param name="pattern">正则规则</param>
        /// <returns></returns>
        public static List<GroupCollection> Matches(string input, string pattern)
        {
            var list=new List<GroupCollection>();
            var matches = Regex.Matches(input, pattern);
            if (matches.Count > 0)
            {
                list.AddRange(from Match item in matches select item.Groups);
            }
            return list;
        }

        /// <summary>
        /// 从指定字符串中过滤出所有符合正则匹配的子集
        /// </summary>
        /// <param name="input">字符串</param>
        /// <param name="pattern">正则规则</param>
        /// <param name="options">提供用于设置正则表达式选项的枚举值</param>
        /// <returns></returns>
        public static List<GroupCollection> Matches(string input, string pattern, RegexOptions options)
        {
            var list = new List<GroupCollection>();
            var matches = Regex.Matches(input, pattern, options);
            if (matches.Count > 0)
            {
                list.AddRange(from Match item in matches select item.Groups);
            }
            return list;
        }

        /// <summary>
        /// 验证IP地址是否合法,如果为空认为验证合格
        /// </summary>
        /// <param name="ip">要验证的IP地址</param>        
        public static bool IsIp(string ip)
        {
            //如果为空认为验证不合格
            if (string.IsNullOrEmpty(ip))
            {
                return false;
            }
            ip = ip.Trim();
            const string pattern = @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$";
            return IsMatch(ip, pattern);
        }

        /// <summary>
        /// 验证EMail是否合法，如果为空认为验证不合格
        /// </summary>
        /// <param name="email">要验证的Email</param>
        public static bool IsEmail(string email)
        {
            //如果为空认为验证不合格
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }
            email = email.Trim();
            const string pattern = @"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$";
            return IsMatch(email, pattern);
        }

        /// <summary>
        /// 验证是否为整数，如果为空认为验证不合格
        /// </summary>
        /// <param name="number">要验证的整数</param>        
        public static bool IsInt(string number)
        {
            //如果为空认为验证不合格
            if (string.IsNullOrEmpty(number))
            {
                return false;
            }
            number = number.Trim();
            var pattern = @"^[0-9]+[0-9]*$";
            return IsMatch(number, pattern);
        }

        /// <summary>
        /// 验证是否为数字，如果为空认为验证不合格
        /// </summary>
        /// <param name="number">要验证的数字</param>        
        public static bool IsNumber(string number)
        {
            //如果为空认为验证不合格
            if (string.IsNullOrEmpty(number))
            {
                return false;
            }
            number = number.Trim();
            const string pattern = @"^[0-9]+[0-9]*[.]?[0-9]*$";
            return IsMatch(number, pattern);
        }

        /// <summary>
        /// 验证日期是否合法,如果为空认为验证不合格
        /// </summary>
        /// <param name="date">日期</param>
        public static bool IsDate(ref string date)
        {
            //如果为空认为验证不合格
            if (string.IsNullOrEmpty(date))
            {
                return false;
            }
            date = date.Trim();
            //替换\
            date = date.Replace(@"\", "-");
            //替换/
            date = date.Replace(@"/", "-");

            //如果查找到汉字"今",则认为是当前日期
            if (date.IndexOf("今", StringComparison.Ordinal) != -1)
            {
                date = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            }
            try
            {
                //用转换测试是否为规则的日期字符
                date = Convert.ToDateTime(date).ToString("d");
                return true;
            }
            catch
            {
                //如果日期字符串中存在非数字，则返回false
                if (!IsInt(date))
                {
                    return false;
                }

                #region 对纯数字进行解析

                switch (date.Length)
                {
                    //对8位纯数字进行解析
                    case 8:
                    {
                        //获取年月日
                        var year = date.Substring(0, 4);
                        var month = date.Substring(4, 2);
                        var day = date.Substring(6, 2);

                        //验证合法性
                        if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                        {
                            return false;
                        }
                        if (Convert.ToInt32(month) > 12 || Convert.ToInt32(day) > 31)
                        {
                            return false;
                        }

                        //拼接日期
                        date = Convert.ToDateTime(year + "-" + month + "-" + day).ToString("d");
                        return true;
                    }
                    //对6位纯数字进行解析
                    case 6:
                    {
                        //获取年月
                        var year = date.Substring(0, 4);
                        var month = date.Substring(4, 2);

                        //验证合法性
                        if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                        {
                            return false;
                        }
                        if (Convert.ToInt32(month) > 12)
                        {
                            return false;
                        }

                        //拼接日期
                        date = Convert.ToDateTime(year + "-" + month).ToString("d");
                        return true;
                    }
                    //对5位纯数字进行解析
                    case 5:
                    {
                        //获取年月
                        var year = date.Substring(0, 4);
                        var month = date.Substring(4, 1);

                        //验证合法性
                        if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                        {
                            return false;
                        }

                        //拼接日期
                        date = year + "-" + month;
                        return true;
                    }
                    //对4位纯数字进行解析
                    case 4:
                    {
                        //获取年
                        var year = date.Substring(0, 4);

                        //验证合法性
                        if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                        {
                            return false;
                        }

                        //拼接日期
                        date = Convert.ToDateTime(year).ToString("d");
                        return true;
                    }
                    default:
                        return false;
                    #endregion

                }
            }
        }

        /// <summary>
        /// 验证身份证是否合法，如果为空认为验证不合格
        /// </summary>
        /// <param name="idCard">要验证的身份证</param>        
        public static bool IsIdCard(string idCard)
        {
            //如果为空认为验证不合格
            if (string.IsNullOrEmpty(idCard))
            {
                return false;
            }
            idCard = idCard.Trim();
            var pattern = new StringBuilder();
            pattern.Append(@"^(11|12|13|14|15|21|22|23|31|32|33|34|35|36|37|41|42|43|44|45|46|");
            pattern.Append(@"50|51|52|53|54|61|62|63|64|65|71|81|82|91)");
            pattern.Append(@"(\d{13}|\d{15}[\dx])$");
            return IsMatch(idCard, pattern.ToString());
        }

        /// <summary>
        /// 检测客户输入的字符串是否有效,并将原始字符串修改为有效字符串或空字符串
        /// 当检测到客户的输入中有攻击性危险字符串,则返回false,有效返回true
        /// </summary>
        /// <param name="input">要检测的字符串</param>
        public static bool IsValidInput(ref string input)
        {
            try
            {
                const string testString = "and |or |exec |insert |select |delete |update |count |chr |mid |master |truncate |char |declare ";
                if (string.IsNullOrEmpty(input))
                {
                    //如果是空值,则跳出
                    return true;
                }
                //替换单引号
                input = input.Replace("'", "''").Trim();
                //检测攻击性危险字符串
                var testArray = testString.Split('|');
                foreach (var testStr in testArray)
                {
                    if (input.ToLower().IndexOf(testStr, StringComparison.Ordinal) == -1) continue;
                    //检测到攻击字符串,清空传入的值
                    input = "";
                    return false;
                }
                //未检测到攻击字符串
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
