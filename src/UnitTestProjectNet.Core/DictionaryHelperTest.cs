using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using UtilsSharp;

namespace UnitTestProjectNet.Core
{
    [TestClass]
    public class DictionaryHelperTest
    {


        [TestMethod]
        public void aa()
        {

            UserInfo u=new UserInfo();

            u.UserName = "lili";
            u.UserPassword = null;
            u.Age = 16;
            u.UserFamilly = new UserFamilly
            {
                MumName = "lucy"
            };

          var aa=  DictionaryHelper.ToDictionaryStringValue(u);

          var bb = DictionaryHelper.ToDictionary(u);

          var bb1=  DictionaryHelper.ToDictionaryStringValue(bb);

          var cc = DictionaryHelper.ToEntity<UserInfo>(bb);

          var dd = DictionaryHelper.ToDictionaryStringValue(cc);

        }


    }


    public class UserInfo
    {

        public string UserName { set; get; }

        public string UserPassword { set; get; }


        public int Age { set; get; }

        public UserFamilly UserFamilly { set; get; }

    }

    public class UserFamilly
    {

      public string MumName { set; get;}

    }


}
