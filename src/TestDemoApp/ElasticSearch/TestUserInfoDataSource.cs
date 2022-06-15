using System;
using System.Collections.Generic;
using System.Text;
using Nest;
using UtilsSharp.ElasticSearch7;

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
        public override EsMappingType EsMappingType { get; set; } = EsMappingType.Hour;

        /// <summary>
        /// 索引最大查询
        /// </summary>
        public override int MaxResultWindow => 20000;
        /// <summary>
        /// 别名索引
        /// </summary>
        public override string AliasIndex { get; set; } = "test_userinfo";
    }
}
