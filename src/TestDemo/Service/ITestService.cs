using System.Threading.Tasks;
using UtilsSharp.Shared.Dependency;
using UtilsSharp.Shared.Standard;

namespace TestDemo.Service
{
    public interface ITestService : IUnitOfWorkDependency
    {
        Task<string> Pay1();
        string Pay2();
        BaseResult<string> Pay3();

        Task<BaseResult<string>> Pay4();
    }
}
