using System;
using System.Collections.Generic;
using System.Text;
using Nest;

namespace TestDemoApp.ElasticSearch
{
    /// <summary>
    /// UserInfo
    /// </summary>
    [ElasticsearchType(RelationName = "testuserinfo", IdProperty = "Id")]
    public class TestUserInfo
    {
        public string Id { set; get; }

        public string UserName { set; get; }

        public string Password { set; get; }

        public int Age { set; get; }

        public string Description { set; get; }
    }
}
