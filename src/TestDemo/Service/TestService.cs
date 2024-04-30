using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UtilsSharp.Logger;
using UtilsSharp.Shared.Dependency;
using UtilsSharp.Shared.Standard;

namespace TestDemo.Service
{
    public class TestService:ITestService
    {
        private readonly ITest2 _test2;

        public TestService(ITest2 test2)
        {
            _test2 = test2;
        }

        public async Task<string> Pay1()
        {
            try
            {
                var r = await PayMethod1();
                //var r = await _test2.PayMethod1();
                return r;

            }
            catch (Exception ex)
            {
                return "";
            }
            

        }

        public async Task<string> PayMethod1()
        {
            int i = 1;
            int j = 0;

            await Task.Run(() => { });

            return (i / j).ToString();
        }


        public string Pay2()
        {
            try
            {
                var r = PayMethod2();
                //var r = _test2.PayMethod2();
                return r;
                //int i = 1;
                //int j = 0;
                //return (i / j).ToString();
            }
            catch
            {
                return "";
            }
            
        }

        public string PayMethod2()
        {
            int i = 1;
            int j = 0;
            return (i / j).ToString();
        }

        public BaseResult<string> Pay3()
        {
            var r = _test2.PayMethod3();
            //var r = PayMethod2();
            throw new Exception("db error");
            return r;
        }

        

        public BaseResult<string> PayMethod3()
        {
            BaseResult<string> result = new BaseResult<string>();
            int i = 1;
            int j = 0;
            result.Result = (i / j).ToString();
            return result;
        }

        public async Task<BaseResult<string>> Pay4()
        {

            throw new Exception("Unauthorized");
            var r = await PayMethod4();
            return r;

        }

        public async Task<BaseResult<string>> PayMethod4()
        {
            BaseResult<string> result = new BaseResult<string>();
            int i = 1;
            int j = 0;
            result.Result= (i / j).ToString();
            return result;
        }
    }


    public interface ITest2 : IUnitOfWorkDependency
    {
        Task<string> PayMethod1();

        string PayMethod2();
        BaseResult<string> PayMethod3();
    }


    public class Test2: ITest2
    {
        public async Task<string> PayMethod1()
        {
            int i = 1;
            int j = 0;

            await Task.Run(() => { });
            //var a = 10;
            var a = i / j;
            return a.ToString();
        }

        public string PayMethod2()
        {
            int i = 1;
            int j = 0;
            return (i / j).ToString();
        }

        public BaseResult<string> PayMethod3()
        {
            BaseResult<string> result = new BaseResult<string>();
            int i = 1;
            int j = 0;
            result.Result = (i / j).ToString();
            return result;
        }
    }


}
