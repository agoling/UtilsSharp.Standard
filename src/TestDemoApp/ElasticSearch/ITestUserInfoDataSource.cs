using System;
using System.Collections.Generic;
using System.Text;
using UtilsSharp.Dependency;
using UtilsSharp.ElasticSearch7;

namespace TestDemoApp.ElasticSearch
{
    /// <summary>
    /// 数据源
    /// </summary>
    public interface ITestUserInfoDataSource : IEsBaseDataSource<TestUserInfo>, IUnitOfWorkDependency
    {

    }
}
