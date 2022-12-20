using System;
using System.Globalization;
using UtilsSharp.Shared.Standard;

namespace UtilsSharp
{
    /// <summary>
    /// 数字帮助类
    /// </summary>
    public class NumberHelper
    {

        /// <summary>
        /// 阿拉伯数字转中文数字(仅支持0~10)
        /// </summary>
        /// <param name="num">数字(仅支持0~10)</param>
        /// <param name="capitalization">是否返回大写中文数字</param>
        /// <returns></returns>
        public static string ToCnNumber(int num, bool capitalization = false)
        {
            if (num < 0 || num > 10)
            {
                return "";
            }
            string[] numStr = { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九", "十" };
            if (capitalization)
            {
                numStr = new[] { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖", "拾" };
            }
            return numStr[num];
        }

        /// <summary>
        /// 金额转中文大写整数支持到万亿,小数部分支持到分(超过两位将进行Banker舍入法处理)
        /// </summary>
        /// <param name="num">需要转换的双精度浮点数</param>
        /// <returns></returns>
        public static BaseResult<string> ToCnMoney(double num)
        {
            var result = new BaseResult<string>();
            try
            {
                string[] lsShZ = { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖", "拾" };
                string[] lsDwZh = { "元", "拾", "佰", "仟", "万", "拾", "佰", "仟", "亿", "拾", "佰", "仟", "万" };
                string[] lsDwX = { "角", "分" };
                var iXShBool = false;//是否含有小数，默认没有(0则视为没有)
                var iZhShBool = true;//是否含有整数,默认有(0则视为没有)

                var numSrX = "";//小数部分
                string numStrDq;//当前的数字字符
                var numStrR = "";//返回的字符串
                num = Math.Round(num, 2);//四舍五入取两位
                                         //各种非正常情况处理
                if (num < 0)
                {
                    result.SetError("参数必须大于0", 6010);
                    return result;
                }
                if (num > 9999999999999.99)
                {
                    result.SetError("参数不能大于9999999999999.99", 6010);
                    return result;
                }
                if (!(num < 0) && !(num > 0))
                {
                    result.Result = lsShZ[0];
                    return result;
                }
                //判断是否有整数
                if (num < 1.00) iZhShBool = false;
                var numStr = num.ToString(CultureInfo.InvariantCulture);
                var numStrZh = numStr;
                if (numStrZh.Contains("."))
                {
                    //分开整数与小数处理
                    numStrZh = numStr.Substring(0, numStr.IndexOf(".", StringComparison.Ordinal));
                    numSrX = numStr.Substring((numStr.IndexOf(".", StringComparison.Ordinal) + 1), (numStr.Length - numStr.IndexOf(".", StringComparison.Ordinal) - 1));
                    iXShBool = true;
                }
                if (numSrX == "" || int.Parse(numSrX) <= 0)
                {
                    //判断是否含有小数部分
                    iXShBool = false;
                }
                if (iZhShBool)
                {
                    //整数部分处理
                    numStrZh = StringHelper.Reverse(numStrZh);//反转字符串
                    for (int a = 0; a < numStrZh.Length; a++)
                    {//整数部分转换
                        numStrDq = numStrZh.Substring(a, 1);
                        if (int.Parse(numStrDq) != 0)
                            numStrR = lsShZ[int.Parse(numStrDq)] + lsDwZh[a] + numStrR;
                        else if (a == 0 || a == 4 || a == 8)
                        {
                            if (numStrZh.Length > 8 && a == 4)
                                continue;
                            numStrR = lsDwZh[a] + numStrR;
                        }
                        else if (int.Parse(numStrZh.Substring(a - 1, 1)) != 0)
                            numStrR = lsShZ[int.Parse(numStrDq)] + numStrR;

                    }
                    if (!iXShBool)
                    {
                        result.Result = numStrR + "整";
                        return result;
                    }
                }
                for (int b = 0; b < numSrX.Length; b++)
                {
                    //小数部分转换
                    numStrDq = numSrX.Substring(b, 1);
                    if (int.Parse(numStrDq) != 0)
                        numStrR += lsShZ[int.Parse(numStrDq)] + lsDwX[b];
                    else if (b != 1 && iZhShBool)
                        numStrR += lsShZ[int.Parse(numStrDq)];
                }
                result.Result = numStrR;
                return result;
            }
            catch (Exception ex)
            {
                result.SetError(ex.Message, 5000);
                return result;
            }

        }

        /// <summary>
        /// 金额以元和万元为单位
        /// </summary>
        /// <param name="num">需转换的金额</param>
        /// <returns></returns>
        public static string ToUnitMoney(decimal num)
        {
            if (10000 <= num)
            {
                return (num / 10000) + "万元";
            }
            else
            {
                return num + "元";
            }
        }

    }
}
