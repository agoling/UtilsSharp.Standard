using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using DotNetCore.Autofac;
using Newtonsoft.Json;
using OptionConfig;
using TestDemoApp.ElasticSearch;

namespace TestDemoApp
{
    class Program
    {
        //private static ITestUserInfoDataSource testUserInfoDataSource;
        static void Main(string[] args)
        {
            //依赖注入注册
            AutofacContainer.Register();
            //Es配置
            const string localEsSettingJson = "{ \"EsHttpAddress\": \"http://192.168.0.221:9200/\", \"UserName\": \"\", \"Password\": \"\", \"EsDefaultIndex\": \"\", \"EsConnectionLimit\": 80 }";
            var localEsSetting = JsonConvert.DeserializeObject<ElasticSearchSetting>(localEsSettingJson);
            ElasticSearchConfig.ElasticSearchSetting = localEsSetting;
            //获取对象
           //testUserInfoDataSource = AutofacContainer.Current.Resolve<ITestUserInfoDataSource>();

            var userInfo = new TestUserInfo
            {
                UserName = "my", Password = "mypsd", Age = 15, Description = "mydescription"
            };
            Parallel.For(0, 1, (i) =>
            {
                Thread t = new Thread(new ParameterizedThreadStart(Save));
                userInfo.Age = i;
                userInfo.Id = Guid.NewGuid().ToString("N");
                t.Start(userInfo);
                Console.WriteLine($"线程{i}！");
            });

            Console.WriteLine("完成！");
            Console.ReadKey();
        }


        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="obj">参数</param>
        public static void Save(object obj)
        {
            ITestUserInfoDataSource testUserInfoDataSource = AutofacContainer.Current.Resolve<ITestUserInfoDataSource>();
            var userInfo = (TestUserInfo)obj;
            var r = testUserInfoDataSource.Save(new List<TestUserInfo>(){userInfo},false, "test_userinfo_20210626");
            Console.WriteLine(r.IsValid.ToString()  + r.ApiCall);
        }


    }
}
