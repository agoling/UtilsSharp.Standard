using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Newtonsoft.Json;
using ProtoBuf;
using ProtoBuf.Grpc.Configuration;
using ProtoBuf.Meta;
using UtilsSharp.Shared.Interface;
using Microsoft.Extensions.DependencyModel;

namespace UtilsSharp.Protobuf
{
    /// <summary>
    /// 根据程序集注册ProtoContract、ProtoInclude、ProtoMember
    /// </summary>
    public static partial class ProtobufRunTime
    {
        /// <summary>
        /// AllMarshallerFactory
        /// </summary>
        private static ConcurrentDictionary<string, MarshallerFactory> _dic;
        
        /// <summary>
        /// 所有ProtoJson
        /// </summary>
        public static string AllProtoJson {get; private set; }

        /// <summary>
        /// AllMarshallerFactory
        /// </summary>
        public static ConcurrentDictionary<string, MarshallerFactory> AllMarshallerFactory => _dic ?? (_dic = Initialize());

        /// <summary>
        /// 根据程序集注册ProtoContract、ProtoInclude、ProtoMember
        /// </summary>
        /// <returns></returns>
        public static ConcurrentDictionary<string, MarshallerFactory> Initialize()
        {
            //获取基于ProtoEntity的类
            var enumerable = AssemblyHelper.GetAllAssemblies();
            var types = new List<Type>();
            foreach (var ts in enumerable.Select(item => item.GetTypes()
                         .Where(x => typeof(IProtobufEntity).IsAssignableFrom(x) && typeof(IProtobufEntity) != x)
                         .ToList()).Where(ts => ts.Count > 0))
            {
                types.AddRange(ts);
            }

            //程序集下的所有RuntimeTypeModels
            var dllRuntimeTypeModels = new ConcurrentDictionary<string, RuntimeTypeModel>();
            var assemblyNameDic = types.GroupBy(g => g.Assembly.FullName).ToDictionary(t=>t.Key,t=>t.ToList());
            foreach (var assemblyNameKeyValuePair in assemblyNameDic)
            {
                #region 得到主类类型
                var currentAssemblyName= assemblyNameKeyValuePair.Key;
                var currentAssemblyTypes = new List<Type>();
                currentAssemblyTypes.AddRange(assemblyNameKeyValuePair.Value);
                #endregion
                
                #region 得到基类类型
                var baseTypeAssemblyNameDic = new ConcurrentDictionary<string, string>();
                foreach (var item in currentAssemblyTypes)
                {
                    var baseType = item.BaseType;
                    while (baseType != null && baseType != typeof(object))
                    {
                        var baseTypeAssemblyName = baseType.Assembly.FullName;
                        if (!string.IsNullOrWhiteSpace(baseTypeAssemblyName))
                        {
                            if (!baseTypeAssemblyNameDic.ContainsKey(baseTypeAssemblyName))
                            {
                                baseTypeAssemblyNameDic.TryAdd(baseTypeAssemblyName, baseTypeAssemblyName);
                            }
                        }
                        baseType = baseType.BaseType;
                    }
                }
                #endregion

                foreach (var item in baseTypeAssemblyNameDic)
                {
                    if (!assemblyNameDic.ContainsKey(item.Key)) continue;
                    var values = assemblyNameDic[item.Key];
                    if (values != null && values.Any())
                    {
                        values.ForEach(v => {
                            if (!currentAssemblyTypes.Contains(v))
                            {
                                currentAssemblyTypes.Add(v);
                            }
                        });
                    }
                }
                var fieldsTag = new ConcurrentDictionary<MetaType, int>();
                //注册ProtoContract、ProtoInclude、ProtoMember
                foreach (var t in currentAssemblyTypes)
                {
                    #region RuntimeTypeModel
                    RuntimeTypeModel runtimeTypeModel;
                    if (!dllRuntimeTypeModels.ContainsKey(currentAssemblyName))
                    {
                        runtimeTypeModel = RuntimeTypeModel.Create(currentAssemblyName);
                        dllRuntimeTypeModels.TryAdd(currentAssemblyName, runtimeTypeModel);
                    }
                    else
                    {
                        runtimeTypeModel = dllRuntimeTypeModels[currentAssemblyName];
                    }
                    var meta = runtimeTypeModel.Add(t, false);
                    #endregion

                    #region 注册基类
                    var baseType = t.BaseType;
                    var nextType = RegisterBaseType(runtimeTypeModel, fieldsTag, t, baseType);
                    var nextBaseType = nextType.BaseType;
                    while (nextBaseType != null && nextBaseType != typeof(object))
                    {
                        nextType = RegisterBaseType(runtimeTypeModel, fieldsTag, nextType, nextBaseType);
                        nextBaseType = nextType.BaseType;
                    }
                    #endregion

                    #region 注册字段
                    foreach (var p in t.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).Where(x => x.GetSetMethod() != null))
                    {
                        var isIgnore =
                            p.CustomAttributes.FirstOrDefault(c => c.AttributeType == typeof(ProtoIgnoreAttribute));
                        if (isIgnore != null) continue;
                        if (fieldsTag.ContainsKey(meta))
                        {
                            fieldsTag[meta] += 1;
                            meta.Add(fieldsTag[meta], p.Name);
                        }
                        else
                        {
                            var fields = meta.GetFields();
                            fieldsTag.TryAdd(meta, fields.Length + 1);
                            meta.Add(fieldsTag[meta], p.Name);
                        }
                    }
                    #endregion
                }
            }
            var marshallerFactory = new ConcurrentDictionary<string, MarshallerFactory>();
            foreach (var item in dllRuntimeTypeModels)
            {
                var newMarshallerFactory = ProtoBufMarshallerFactory.Create(item.Value);
                marshallerFactory.TryAdd(item.Key, newMarshallerFactory);
            }
            #region 打印所有IProtobufEntity类型的Dll下的proto文件json

            //所有IProtobufEntity类型的Dll下的proto文件字典
            var allProtoDic = new ConcurrentDictionary<string, ConcurrentDictionary<string, string>>();
            dllRuntimeTypeModels.Keys.ToList().ForEach(key =>
            {
                //每个IProtobufEntity类型的Dll下的proto文件字典
                var protoDic = new ConcurrentDictionary<string, string>();
                var i = 0;
                foreach (MetaType type in dllRuntimeTypeModels[key].GetTypes())
                {
                    try
                    {
                        //每个类下面的proto元素内容（含message、字段、枚举等）
                        var schema = dllRuntimeTypeModels[key].GetSchema(type.Type);
                        if (protoDic.ContainsKey(type.Type.FullName))
                        {
                            protoDic.TryAdd(type.Type.FullName + "_" + i.ToString(), schema);
                        }
                        else
                        {
                            protoDic.TryAdd(type.Type.FullName, schema);
                        }
                        i++;
                    }
                    catch (Exception ex)
                    {

                    }
                }
                allProtoDic.TryAdd(key, protoDic);

            });
            AllProtoJson = JsonConvert.SerializeObject(allProtoDic);
            #endregion
            return marshallerFactory;
        }

        /// <summary>
        /// 注册基类
        /// </summary>
        /// <param name="runtimeTypeModel">runtimeTypeModel</param>
        /// <param name="fieldsTag">fieldsTag</param>
        /// <param name="type">type</param>
        /// <param name="baseType">baseType</param>
        /// <returns></returns>
        private static Type RegisterBaseType(RuntimeTypeModel runtimeTypeModel, IDictionary<MetaType, int> fieldsTag, Type type, Type baseType)
        {
            if (baseType == null || baseType == typeof(object)) return baseType;
            var baseTypeMeta = runtimeTypeModel.Add(baseType, false);
            var subtypes = baseTypeMeta.GetSubtypes();
            if (subtypes.Any(t => t.DerivedType.Type == type)) return baseType;
            if (fieldsTag.ContainsKey(baseTypeMeta))
            {
                fieldsTag[baseTypeMeta] += 1;

                baseTypeMeta.AddSubType(fieldsTag[baseTypeMeta], type);
            }
            else
            {
                var fields = baseTypeMeta.GetFields();
                fieldsTag.Add(baseTypeMeta, fields.Length + 1);
                baseTypeMeta.AddSubType(fieldsTag[baseTypeMeta], type);
            }
            return baseType;
        }

        /// <summary>
        /// 获取所有的程序集(含所有系统程序集、Nuget下载包)
        /// </summary>
        /// <returns>程序集集合</returns>
        private static List<Assembly> GetAllAssemblies()
        {
            var list = new List<Assembly>();
            var deps = DependencyContext.Default;
            //排除所有的系统程序集、Nuget下载包
            var libs = deps.CompileLibraries;
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
    }

    /// <summary>
    /// 根据程序集注册ProtoContract、ProtoInclude、ProtoMember
    /// </summary>
    public static partial class ProtobufRunTime
    {
        /// <summary>
        /// ClientFactory 字典
        /// </summary>
        private static readonly ConcurrentDictionary<string,ClientFactory> ClientFactoryDic = new ConcurrentDictionary<string, ClientFactory>();

        /// <summary>
        /// GetMarshallerFactory
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns></returns>
        public static List<MarshallerFactory> GetMarshallerFactory<T>()
        {
            var result = new List<MarshallerFactory>();
            var assemblyName = typeof(T).Assembly.FullName;
            if (AllMarshallerFactory.ContainsKey(assemblyName))
            {
                result.Add(AllMarshallerFactory[assemblyName]);
            }
            return result;
        }

        /// <summary>
        /// GetMarshallerFactory
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <returns></returns>
        public static List<MarshallerFactory> GetMarshallerFactory(string assemblyName)
        {
            var result = new List<MarshallerFactory>();
            if (AllMarshallerFactory.ContainsKey(assemblyName))
            {
                result.Add(AllMarshallerFactory[assemblyName]);
            }
            return result;
        }

        /// <summary>
        /// GetMarshallerFactoryByKeyword
        /// </summary>
        /// <param name="assemblyNameKeyword">程序集名称关键词</param>
        /// <returns></returns>
        public static List<MarshallerFactory> GetMarshallerFactoryByKeyword(string assemblyNameKeyword)
        {
            var result = new List<MarshallerFactory>();
            var marshallerFactories = AllMarshallerFactory.Where(t => t.Key.Contains(assemblyNameKeyword)).ToDictionary(t => t.Key, t => t.Value).Values.ToList();
            if (marshallerFactories.Count > 0)
            {
                result.AddRange(marshallerFactories);
            }
            return result;
        }

        /// <summary>
        /// GetClientFactory
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns></returns>
        public static ClientFactory GetClientFactory<T>()
        {
            var marshallerFactories = GetMarshallerFactory<T>();
            try
            {
                var key = typeof(T).FullName;
                if (string.IsNullOrEmpty(key)) { return null; }
                if (ClientFactoryDic.ContainsKey(key)) { return ClientFactoryDic[key]; }
                var value = ClientFactory.Create(BinderConfiguration.Create(marshallerFactories));
                ClientFactoryDic.TryAdd(key, value);
                return ClientFactoryDic[key];
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// GetClientFactory
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <returns></returns>
        public static ClientFactory GetClientFactory(string assemblyName)
        {
            var marshallerFactories = GetMarshallerFactory(assemblyName);
            try
            {
                if (string.IsNullOrEmpty(assemblyName)) { return null; }
                if (ClientFactoryDic.ContainsKey(assemblyName)) { return ClientFactoryDic[assemblyName]; }
                var value = ClientFactory.Create(BinderConfiguration.Create(marshallerFactories));
                ClientFactoryDic.TryAdd(assemblyName, value);
                return ClientFactoryDic[assemblyName];
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// GetClientFactoryByKeyword
        /// </summary>
        /// <param name="assemblyNameKeyword">程序集名称关键词</param>
        /// <returns></returns>
        public static ClientFactory GetClientFactoryByKeyword(string assemblyNameKeyword)
        {
            var marshallerFactories = GetMarshallerFactoryByKeyword(assemblyNameKeyword);
            return ClientFactory.Create(BinderConfiguration.Create(marshallerFactories));
        }
    }

}
