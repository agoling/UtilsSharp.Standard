using System;
using System.Collections.Generic;
using System.Text;

namespace Dependency.Core
{
    /// <summary>
    /// 表示实现者是一个瞬态依赖
    /// </summary>
    public interface ITransientDependency: IDependency
    {
    }
}
