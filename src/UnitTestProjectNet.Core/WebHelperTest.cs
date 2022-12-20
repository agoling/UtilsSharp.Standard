using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using UtilsSharp;
using UtilsSharp.Shared.Standard;

namespace UnitTestProjectNet.Core
{
    [TestClass]
    public class WebHelperAsyncTest
    {

        [TestMethod]
        public async Task GetCaptchaTest()
        {
            await GetCaptcha();
        }
        public async Task GetCaptcha()
        {
            using (var webHelper = new WebHelper())
            {
                var dic = new Dictionary<string, object> { { "sign", "b486a1fa5e024be0a45c096ca5f6cfec" } };
                var url = $"https://www.baidu.com/api/xxxxxx";
                webHelper.Headers.Add("Content-Type", "application/json;charset=UTF-8");
                webHelper.Proxy=new WebProxy("127.0.0.1:98"){Credentials = new NetworkCredential("username","password")};//代理请求
                webHelper.Encoding = Encoding.UTF8;
                webHelper.Timeout = 10;//请求超时时间 单位秒
                var requestResult = webHelper.DoPost<BaseResult<string>>(url, dic).HandleResult();
            }
        }


        [TestMethod]
        public void Http1Async()
        {
            using (var webHelper = new WebHelper())
            {
                var json= "{\"TaskId\":\"f140d4535e4a41468b9c9923115130aa\",\"CustomerId\":\"b2b-1624786331\",\"BusId\":null,\"Name\":\"快哦批量违禁词检测2021-05-11 14:53:32\",\"Tag\":\"快哦批量违禁词检测\",\"State\":0,\"IsSuccess\":false,\"Progress\":0,\"Message\":\"\",\"Parameter\":\"{\\\"PlanNo\\\":\\\"f140d4535e4a41468b9c9923115130aa\\\",\\\"Classify\\\":null,\\\"OfferIds\\\":[644120245956],\\\"DraftBoxIds\\\":null,\\\"Status\\\":2,\\\"PageIndex\\\":1,\\\"PageSize\\\":20,\\\"IndustryCategorys\\\":[\\\"通用\\\"],\\\"AppKey\\\":\\\"2721863\\\",\\\"AliId\\\":\\\"b2b-1624786331\\\",\\\"SourcePlatform\\\":1,\\\"KuaiOId\\\":12}\",\"Result\":null,\"CreateTime\":\"2021-05-11T14:53:32.5630457+08:00\",\"BeginTime\":\"0001-01-01T00:00:00\",\"CompleteTime\":\"0001-01-01T00:00:00\",\"DeleteTime\":\"0001-01-01T00:00:00\",\"LogInfo\":null,\"ClientId\":null}";
                object obj = JsonConvert.DeserializeObject<object>(json);
                var r = webHelper.DoPost($"http://192.168.0.17:8012/test/task/api/task/IssuedTask", obj);
                var aa= r.Result;
            }

        }


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


            JsonRequest j=new JsonRequest();

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
