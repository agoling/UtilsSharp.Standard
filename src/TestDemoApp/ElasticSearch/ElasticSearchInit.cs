using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Newtonsoft.Json;
using UtilsSharp.DotNetCore.Autofac;
using UtilsSharp.OptionConfig;

namespace TestDemoApp.ElasticSearch
{
    /// <summary>
    /// ElasticSearch初始化
    /// </summary>
    public class ElasticSearchInit
    {

        public static void Init()
        {
            //依赖注入注册
            AutofacContainer.Register();
            //Es配置
            const string localEsSettingJson = "{ \"EsHttpAddress\": \"http://192.168.0.91:9200/\", \"UserName\": \"\", \"Password\": \"\", \"EsDefaultIndex\": \"\", \"EsConnectionLimit\": 80 }";
            var localEsSetting = JsonConvert.DeserializeObject<ElasticSearchSetting>(localEsSettingJson);
            localEsSetting.EsNetworkProxy = "http://192.168.0.141:8888";
            ElasticSearchConfig.ElasticSearchSetting = localEsSetting;
            //获取对象
            //testUserInfoDataSource = AutofacContainer.Current.Resolve<ITestUserInfoDataSource>();

            var userInfo = new TestUserInfo
            {
                UserName = "my",
                Password = "mypsd",
                Age = 15,
                Description = "mydescription"
            };
            Parallel.For(0, 50, (i) =>
            {
                Thread t = new Thread(new ParameterizedThreadStart(DoSomethings));
                userInfo.Age = i;
                userInfo.Id = Guid.NewGuid().ToString("N")+i;
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
        public static void DoSomethings(object obj)
        {
            ITestUserInfoDataSource testUserInfoDataSource = AutofacContainer.Current.Resolve<ITestUserInfoDataSource>();

            #region Save
            var userInfo = (TestUserInfo)obj;
            var r = testUserInfoDataSource.Save(new List<TestUserInfo>() { userInfo });
            Console.WriteLine(r.IsValid.ToString() + r.ApiCall);
            #endregion


            #region Get
            //var r = testUserInfoDataSource.Get("a49b4cf0df084c12917bb567cf61bb25", "test_userinfo_20220519");


            #endregion



        }

    }
}
