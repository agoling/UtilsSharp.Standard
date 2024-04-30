using System;
using System.Collections.Generic;
using System.Text;
using Nest;
using UtilsSharp;
using UtilsSharp.ElasticSearch7;
using UtilsSharp.OptionConfig;

namespace TestDemoApp.ElasticSearch
{
    /// <summary>
    /// 数据源
    /// </summary>
    public class TestUserInfoDataSource : EsBaseDataSource<TestUserInfo>, ITestUserInfoDataSource
    {

        /// <summary>
        /// 按天生成
        /// </summary>
        public override EsMappingType EsMappingType { get; set; } = EsMappingType.Hour;

        /// <summary>
        /// 索引最大查询
        /// </summary>
        public override int MaxResultWindow => 20000;

        /// <summary>
        /// 这个索引配置
        /// </summary>
        public override ElasticSearchSetting Setting
        {
            get
            {
                var config = MapperHelper<ElasticSearchSetting, ElasticSearchSetting>.Map(ElasticSearchConfig.ElasticSearchSetting);
                config.DisableIdInference = true;
                return config;
            }
        }


        ///// <summary>
        ///// 这个索引配置
        ///// </summary>
        //public override ElasticSearchSetting Setting => new ElasticSearchSetting()
        //{
        //    UserName = ElasticSearchConfig.ElasticSearchSetting.UserName,
        //    Password = ElasticSearchConfig.ElasticSearchSetting.Password,
        //    EsHttpAddress = ElasticSearchConfig.ElasticSearchSetting.EsHttpAddress,
        //    EsNetworkProxy = ElasticSearchConfig.ElasticSearchSetting.EsNetworkProxy,
        //    DisableIdInference = true
        //};



        ///// <summary>
        ///// 别名索引
        ///// </summary>
        //public override string AliasIndex { get; set; } = "test_userinfo";
    }
}
