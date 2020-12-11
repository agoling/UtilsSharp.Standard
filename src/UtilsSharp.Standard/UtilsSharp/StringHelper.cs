using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace UtilsSharp
{
    /// <summary>
    /// 字符串帮助类
    /// </summary>
    public class StringHelper
    {
        /// <summary>
        /// 转义正则表达式特殊符号
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static string TransferenceRegex(string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
            return str.Replace("\\", "\\\\").Replace("*", "\\*")
                .Replace("+", "\\+").Replace("|", "\\|")
                .Replace("{", "\\{").Replace("}", "\\}")
                .Replace("(", "\\(").Replace(")", "\\)")
                .Replace("^", "\\^").Replace("$", "\\$")
                .Replace("[", "\\[").Replace("]", "\\]")
                .Replace("?", "\\?").Replace(",", "\\,")
                .Replace(".", "\\.").Replace("&", "\\&");
        }

        /// <summary>
        /// HTML转行成TEXT
        /// </summary>
        /// <param name="htmlStr">html字符串</param>
        /// <returns></returns>
        public static string HtmlToTxt(string htmlStr)
        {
            if (string.IsNullOrEmpty(htmlStr)) return htmlStr;
            string[] aryReg ={
                @"<script[^>]*?>.*?</script>",
                @"<(\/\s*)?!?((\w+:)?\w+)(\w+(\s*=?\s*(([""'])(\\[""'tbnr]|[^\7])*?\7|\w+)|.{0})|\s)*?(\/\s*)?>",
                @"([\r\n])[\s]+",
                @"&(quot|#34);",
                @"&(amp|#38);",
                @"&(lt|#60);",
                @"&(gt|#62);",
                @"&(nbsp|#160);",
                @"&(iexcl|#161);",
                @"&(cent|#162);",
                @"&(pound|#163);",
                @"&(copy|#169);",
                @"&#(\d+);",
                @"-->",
                @"<!--.*\n"
            };
            var strOutput = aryReg.Select(item => new Regex(item, RegexOptions.IgnoreCase)).Aggregate(htmlStr, (current, regex) => regex.Replace(current, string.Empty));
            strOutput = strOutput.Replace("<", "");
            strOutput = strOutput.Replace(">", "");
            strOutput=strOutput.Replace("\r\n", "");
            return strOutput;
        }

        /// <summary>
        /// 得到字符串长度
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static double GetCharLength(string str)
        {
            if (str.Length == 0) return 0;
            var ascii = new ASCIIEncoding();
            double strLength = 0;
            var strBytes = ascii.GetBytes(str);
            foreach (var item in strBytes)
            {
                if (item == 63)
                {
                    strLength += 1;
                }
                else
                {
                    strLength += 0.5;
                }
            }
            return Math.Floor(strLength);
        }

        /// <summary>
        /// 按字符长度截取字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="charLength">要截取的字符长度</param>
        /// <returns></returns>
        public static string CutChar(string str, int charLength)
        {
            if (string.IsNullOrEmpty(str) || charLength <= 0) return str;
            var bytes = Encoding.Unicode.GetBytes(str);
            var n = 0;  //  表示当前的字节数
            var i = 0;  //  要截取的字节数
            for (; i < bytes.GetLength(0) && n < charLength; i++)
            {
                //  偶数位置，如0、2、4等，为UCS2编码中两个字节的第一个字节
                if (i % 2 == 0)
                {
                    n++;      //  在UCS2第一个字节时n加1
                }
                else
                {
                    //  当UCS2编码的第二个字节大于0时，该UCS2字符为汉字，一个汉字算两个字节
                    if (bytes[i] > 0)
                    {
                        n++;
                    }
                }
            }
            //  如果i为奇数时，处理成偶数
            if (i % 2 != 1) return Encoding.Unicode.GetString(bytes, 0, i);

            if (bytes[i] > 0)
            {
                //  该UCS2字符是汉字时，去掉这个截一半的汉字
                i = i - 1;
            }
            else
            {
                //  该UCS2字符是字母或数字，则保留该字符
                i = i + 1;
            }
            return Encoding.Unicode.GetString(bytes, 0, i);
        }


        /// <summary>
        /// 按字长度截取字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="strCount">要截取的字数（含）</param>
        /// <returns></returns>
        public static string CutString(string str, int strCount)
        {
            if (string.IsNullOrEmpty(str) || strCount <= 0) return str;
            return str.Length >= strCount ? str.Substring(0, strCount) : str;
        }


        /// <summary>
        /// 字符串分割获取项
        /// </summary>
        /// <param name="str">例如："苹果,香蕉,猕猴桃,凤梨,枇杷,葡萄,柠檬,橘子,火龙果"</param>
        /// <param name="splitChar">,</param>
        /// <param name="returnItemCount">2</param>
        /// <returns>苹果,香蕉</returns>
        public static string SplitString(string str, char splitChar, int returnItemCount)
        {
            if (string.IsNullOrEmpty(str)) return str;
            if (returnItemCount < 1) returnItemCount = 1;
            str = str.TrimEnd(splitChar);
            int index = 0;
            for (int i = 1; i <= returnItemCount; i++)
            {
                index = str.IndexOf(splitChar, index + 1);
                if (index < 0) break;
            }
            return index >= 0 ? str.Substring(0, index) : str;
        }

        /// <summary>
        /// 反转字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Reverse(string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
            var array = str.ToCharArray();
            var cs = array.Reverse<char>();
            var array1 = cs.ToArray<char>();
            var result = new string(array1);
            return result;
        }

        /// <summary>
        /// 压缩字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static string Compress(string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
            var compressBeforeByte = Encoding.GetEncoding("UTF-8").GetBytes(str);
            var ms = new MemoryStream();
            var zip = new GZipStream(ms, CompressionMode.Compress, true);
            zip.Write(compressBeforeByte, 0, compressBeforeByte.Length);
            zip.Close();
            var compressAfterByte = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(compressAfterByte, 0, compressAfterByte.Length);
            ms.Close();
            var compressString = Convert.ToBase64String(compressAfterByte);
            return compressString;
        }

        /// <summary>
        ///  字符串解压缩
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static string DeCompress(string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
            var decompressBeforeByte = Convert.FromBase64String(str);
            var ms = new MemoryStream(decompressBeforeByte);
            var zip = new GZipStream(ms, CompressionMode.Decompress, true);
            var msReader = new MemoryStream();
            var decompressAfterByte = new byte[0x1000];
            while (true)
            {
                var reader = zip.Read(decompressAfterByte, 0, decompressAfterByte.Length);
                if (reader <= 0)
                {
                    break;
                }
                msReader.Write(decompressAfterByte, 0, reader);
            }
            zip.Close();
            ms.Close();
            msReader.Position = 0;
            decompressAfterByte = msReader.ToArray();
            msReader.Close();

            var decompressString = Encoding.GetEncoding("UTF-8").GetString(decompressAfterByte);
            return decompressString;
        }

        /// <summary>
        /// 隐藏手机号(手机号加星星如：136****8568)
        /// </summary>
        /// <param name="mobilePhone">手机号</param>
        /// <returns></returns>
        public static string HideMobilePhone(string mobilePhone)
        {
            if (string.IsNullOrEmpty(mobilePhone)||mobilePhone.Length!=11) return mobilePhone;
            var first = mobilePhone.Substring(0, 3);
            var third = mobilePhone.Substring(7, 4);
            return $"{first}****{third}";
        }
    }
}
