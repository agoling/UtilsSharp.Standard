using System;
using System.Collections.Generic;

namespace UtilsSharp.Shared.Standard
{
    /// <summary>
    /// Exception基础处理
    /// </summary>
    public static class BaseException
    {
        /// <summary>
        /// 获取默认规则
        /// </summary>
        /// <returns></returns>
        public static List<ExceptionRegexRule> GetDefaultRegexRule()
        {
            var rules = new List<ExceptionRegexRule>
            {
              new ExceptionRegexRule { Code = BaseStateCode.授权过期, Rules = new List<string> { "unauthorized", "request need user authorized", "授权过期", "授权失效", "未授权" },Msg="" },
              new ExceptionRegexRule { Code = BaseStateCode.未登录, Rules = new List<string> { "未登入", "登入过期", "登入失效", "未登录", "登录过期", "登录失效", "token过期", "token expires" },Msg="" }
            };
            return rules;
        }

        /// <summary>
        /// 匹配错误信息
        /// </summary>
        /// <param name="logId">日志Id</param>
        /// <param name="ex">Exception</param>
        /// <param name="rules">匹配规则</param>
        /// <returns></returns>
        public static ExceptionRegexResult Regex(this Exception ex, string logId, List<ExceptionRegexRule> rules)
        {
            if (rules != null)
            {
                var errorMessage = (ex.Message + ex.StackTrace).ToLower();

                foreach (var item in rules)
                {
                    foreach (var word in item.Rules)
                    {
                        if (errorMessage.Contains(word.ToLower()))
                        {
                            if (!string.IsNullOrWhiteSpace(item.Msg))
                            {
                                // 获取最后一个字符
                                char lastChar = item.Msg[item.Msg.Length - 1];
                                // 检查是否为标点符号
                                if (char.IsPunctuation(lastChar))
                                {
                                    // 如果是标点符号，则移除最后一个字符
                                    item.Msg = item.Msg.TrimEnd(lastChar);
                                }
                                return new ExceptionRegexResult() { Code = item.Code, Msg =$"{item.Msg}！错误码：{logId}"};
                            }
                            return new ExceptionRegexResult() { Code = item.Code, Msg = item.Code.ToMsg(logId) };
                        }
                    }
                }
            }
            return new ExceptionRegexResult() { Code = 5000, Msg = 5000.ToMsg(logId) };
        }
    }

    /// <summary>
    /// Exception匹配规则
    /// </summary>
    public class ExceptionRegexRule: ExceptionRegexResult
    {
        /// <summary>
        /// 匹配规则 如：["未登入"]
        /// </summary>
        public List<string> Rules { set; get; }
        
    }

    /// <summary>
    /// Exception匹配结果
    /// </summary>
    public class ExceptionRegexResult
    {
        /// <summary>
        /// 错误码 如：4010
        /// </summary>
        public int Code { set; get; }

        /// <summary>
        /// 提示信息 如："您还未登入"
        /// </summary>
        public string Msg { set; get; }
    }



}
