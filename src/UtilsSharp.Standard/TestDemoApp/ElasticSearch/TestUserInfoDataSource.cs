using System;
using System.Collections.Generic;
using System.Text;
using ElasticSearch7;
using Nest;
using OptionConfig;

namespace TestDemoApp.ElasticSearch
{
    /// <summary>
    /// 数据源
    /// </summary>
    public class TestUserInfoDataSource: EsBaseDataSource<TestUserInfo>, ITestUserInfoDataSource
    {
        /// <summary>
        /// 按天生成
        /// </summary>
        public override EsMappingType EsMappingType { get; set; } = EsMappingType.Day;
        /// <summary>
        /// 别名索引
        /// </summary>
        public override string AliasIndex { get; set; } = "test_userinfo";
    }
}
