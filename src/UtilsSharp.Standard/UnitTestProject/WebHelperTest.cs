using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UtilsSharp;

namespace UnitTestProjectNetCore
{
    [TestClass]
    public class WebHelperAsyncTest
    {
        [TestMethod]
        public void HttpAsync()
        {
            List<string> ipList = new List<string>
            {
                "39.99.250.141",
                "39.99.250.142",
                "39.99.250.143",
                "39.99.250.144",
                "39.99.250.145",
                "39.99.250.146",
                "39.99.250.147",
                "39.99.250.148",
                "39.99.250.149"
            };
            Stopwatch sw1=new Stopwatch();
            sw1.Start();
            List<Task> tasks1 = new List<Task>();
            foreach (var item in ipList)
            {
                tasks1.Add(Task.Run(() =>
                {
                    Send(item);
                }));
            }
            Task.WaitAll(tasks1.ToArray());
            sw1.Stop();
            Stopwatch sw2 = new Stopwatch();
            sw2.Start();
            List<Task> tasks=new List<Task>();
            ConcurrentBag<string> ips=new ConcurrentBag<string>();
            foreach (var item in ipList)
            {
                tasks.Add(Send1(item));
            }
            Task.WaitAll(tasks.ToArray());
            sw2.Stop();
            //http://httpbin.org/get?ip=39.99.250.145

            string aa = $"同步:{sw1.Elapsed.TotalSeconds},异步:{sw2.Elapsed.TotalSeconds}";

        }


        private string Send(string ip)
        {
            using (WebHelper httpHelper = new WebHelper())
            {
                Dictionary<string,string> dic=new Dictionary<string, string>();
                dic.Add("ip",ip);
                var r= httpHelper.DoGet("http://httpbin.org/get",dic);
                return r.Result;
            }
        }

        private async Task<string> Send1(string ip)
        {
            using (WebHelper httpHelper = new WebHelper())
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("ip", ip);
                var r = await httpHelper.DoGetAsync("http://httpbin.org/get", dic);
                return r.Result;
            }
        }

    }

    [TestClass]
    public class WebHelperTest
    {
        [TestMethod]
        public void Http()
        {

           WebHelper httpHelper =new WebHelper();
            //httpHelper.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            model m = new model();
            m.loginType = 2;
            m.passwordOrCode = "ED0D1411-B93A-4B7B-AABA-F5C2CC232399";

            object requestObj = new
            {
                Age = 0591,
                Name = "xiujie",
                Ys = 运算符号.B,
                M = m,
                dt = DateTime.Now
            };


            JsonRequest request = new JsonRequest();
            request.Age = 0591;
            request.Name = "xiujie";
            request.Ys = 运算符号.B;
            request.M = m;
            request.dt = DateTime.Now;

            var dic = DictionaryHelper.ObjToDictionary(null, null,null);

            JsonRequest j=new JsonRequest();
            JsonRequest obj = DictionaryHelper.DictionaryToObj<JsonRequest>(dic, null, null);
            
            var r= httpHelper.DoGet("http://localhost:15892/home/json?name=123&age=456", request);
        }


    }

    public class model
    {
        public long loginType { set; get; }

        public string passwordOrCode { set; get; }
    }

    public class model1
    {
        public string result { set; get; }

        public string code { set; get; }

        public string msg { set; get; }
    }

    public class JsonRequest
    {
        public string Name { set; get; }

        public long Age { set; get; }

        public 运算符号 Ys { set; get; }

        public model M { set; get; }

        public DateTime dt { set; get; }
    }

    public enum 运算符号
    {
        A=0,
        B=1
    }
}
