using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using Microsoft.Extensions.DependencyModel;

namespace AspNetCore
{
    /// <summary>
    /// 程序集帮助类
    /// </summary>
    public class AssemblyHelper
    {
        /// <summary>
        /// 获取类以及类实现的接口键值对
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <returns>类以及类实现的接口键值对</returns>
        public static Dictionary<Type, List<Type>> GetClassInterfacePairs(string assemblyName)
        {
            //存储 实现类 以及 对应接口
            var dic = new Dictionary<Type, List<Type>>();
            var assembly = GetAssembly(assemblyName);
            if (assembly == null) return dic;
            var types = assembly.GetTypes();
            foreach (var item in types.AsEnumerable().Where(x => !x.IsAbstract && !x.IsInterface && !x.IsGenericType))
            {
                dic.Add(item, item.GetInterfaces().Where(x => !x.IsGenericType).ToList());
            }
            return dic;
        }

        /// <summary>
        /// 获取所有的程序集
        /// </summary>
        /// <returns>程序集集合</returns>
        public static List<Assembly> GetAllAssemblies()
        {
            var list = new List<Assembly>();
            var deps = DependencyContext.Default;
            //排除所有的系统程序集、Nuget下载包
            var libs = deps.CompileLibraries.Where(lib => !lib.Serviceable && lib.Type != "package");
            foreach (var lib in libs)
            {
                try
                {
                    var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(lib.Name));
                    list.Add(assembly);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            return list;
        }

        /// <summary>
        /// 获取指定的程序集
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <returns>程序集</returns>
        public static Assembly GetAssembly(string assemblyName)
        {
            return GetAllAssemblies().FirstOrDefault(assembly => assembly.FullName.Contains(assemblyName));
        }

    }
}
