using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UtilsSharp;

namespace UnitTestProjectNetCore
{
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
