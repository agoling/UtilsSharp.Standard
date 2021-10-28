using System;
using System.Collections.Generic;
using System.Text;
using Logger;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UtilsSharp;

namespace UnitTestProjectNet.Core
{
    /// <summary>
    /// 服务器缓存帮助类
    /// </summary>
    [TestClass]
    public class LoggerTest
    {

        [TestMethod]
        public void TraceHelper()
        {
            using (TraceHelper tr=new TraceHelper(true,true))
            {
                for (int i = 0; i < 100000; i++)
                {
                    tr.Add("0593", $"~%^&*.进来了%\"{i}");
                    tr.Add("0593", $"开始执行了\n{i}");
                    tr.Add("0593", $"进行中了*{i}");
                    tr.Add("0593", $"返回结果了#{i}");
                    tr.Add("0591", $"0591返回结果了#{i}");
                    tr.Add("0592", $"我是厦门");
                    tr.Add("0594", $"我是厦门");
                }
             
                var result1 = tr.Get("0591");
                var result2 = tr.Get("0592");
                var result3 = tr.Get("0593");
                var result4 = tr.Get("0594");

            }

        }


    }
}
