using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ProtoBuf;
using ProtoBuf.Grpc.Configuration;
using ProtoBuf.Meta;
using UtilsSharp.Standard.Interface;

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
        private static Dictionary<string, MarshallerFactory> _dic;

        /// <summary>
        /// AllMarshallerFactory
        /// </summary>
        public static Dictionary<string, MarshallerFactory> AllMarshallerFactory => _dic ?? (_dic = Initialize());

        /// <summary>
        /// 根据程序集注册ProtoContract、ProtoInclude、ProtoMember
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, MarshallerFactory> Initialize()
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
            IDictionary<string, RuntimeTypeModel> _dllRuntimeTypeModels = new Dictionary<string, RuntimeTypeModel>();
            var assemblyNameDic = types.GroupBy(g => g.Assembly.FullName).ToDictionary(t=>t.Key,t=>t.ToList());
            foreach (var assemblyNameKeyValuePair in assemblyNameDic)
            {
                #region 得到主类类型
                var currentAssemblyName= assemblyNameKeyValuePair.Key;
                var currentAssemblyTypes = new List<Type>();
                currentAssemblyTypes.AddRange(assemblyNameKeyValuePair.Value);
                #endregion
                
                #region 得到基类类型
                var baseTypeAssemblyNameDic = new Dictionary<string, string>();
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
                                baseTypeAssemblyNameDic.Add(baseTypeAssemblyName, baseTypeAssemblyName);
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
                    currentAssemblyTypes.AddRange(values);
                }
                var fieldsTag = new Dictionary<MetaType, int>();
                //注册ProtoContract、ProtoInclude、ProtoMember
                foreach (var t in currentAssemblyTypes)
                {
                    #region RuntimeTypeModel
                    RuntimeTypeModel runtimeTypeModel;
                    if (!_dllRuntimeTypeModels.ContainsKey(currentAssemblyName))
                    {
                        runtimeTypeModel = RuntimeTypeModel.Create(currentAssemblyName);
                        _dllRuntimeTypeModels.Add(currentAssemblyName, runtimeTypeModel);
                    }
                    else
                    {
                        runtimeTypeModel = _dllRuntimeTypeModels[currentAssemblyName];
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
                            fieldsTag.Add(meta, fields.Length + 1);
                            meta.Add(fieldsTag[meta], p.Name);
                        }
                    }
                    #endregion
                }
            }
            var marshallerFactory = new Dictionary<string, MarshallerFactory>();
            foreach (var item in _dllRuntimeTypeModels.Values)
            {
                var newMarshallerFactory = ProtoBufMarshallerFactory.Create(item);
                if (!marshallerFactory.ContainsKey(item.ToString()))
                {
                    marshallerFactory.Add(item.ToString(), newMarshallerFactory);
                }
                
            }
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
            if (subtypes.All(t => t.DerivedType.Type != type))
            {
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
            }
            return baseType;
        }
    }

    /// <summary>
    /// 根据程序集注册ProtoContract、ProtoInclude、ProtoMember
    /// </summary>
    public static partial class ProtobufRunTime
    {
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
            if (marshallerFactories.Count>0)
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
            return ClientFactory.Create(BinderConfiguration.Create(marshallerFactories));
        }

        /// <summary>
        /// GetClientFactory
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <returns></returns>
        public static ClientFactory GetClientFactory(string assemblyName)
        {
            var marshallerFactories = GetMarshallerFactory(assemblyName);
            return ClientFactory.Create(BinderConfiguration.Create(marshallerFactories));
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
