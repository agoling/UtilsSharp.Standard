﻿using System;
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
using System.IO;
using ProtoBuf.Grpc.Reflection;

namespace UtilsSharp.Grpc
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
        public static string AllProtoJson { get; private set; }

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
            var enumerable = GetAllAssemblies();
            var types = new List<Type>();
            foreach (var ts in enumerable.Select(item => item.GetTypes()
                         .Where(x => typeof(IProtobufEntity).IsAssignableFrom(x) && typeof(IProtobufEntity) != x)
                         .ToList()).Where(ts => ts.Count > 0))
            {
                types.AddRange(ts);
            }

            //程序集下的所有RuntimeTypeModels
            var dllRuntimeTypeModels = new ConcurrentDictionary<string, RuntimeTypeModel>();
            var assemblyNameDic = types.GroupBy(g => g.Assembly.FullName).ToDictionary(t => t.Key, t => t.ToList());
            foreach (var assemblyNameKeyValuePair in assemblyNameDic)
            {
                #region 得到主类类型
                var currentAssemblyName = assemblyNameKeyValuePair.Key;
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
                currentAssemblyTypes = currentAssemblyTypes.OrderBy(o => o.FullName).ToList();
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
        private static IEnumerable<Assembly> GetAllAssemblies()
        {
            var list = new List<Assembly>();
            var dependency = DependencyContext.Default;
            //排除所有的系统程序集、Nuget下载包
            var libs = dependency.CompileLibraries;
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
        private static readonly ConcurrentDictionary<string, ClientFactory> ClientFactoryDic = new ConcurrentDictionary<string, ClientFactory>();
        /// <summary>
        /// BinderConfiguration 字典
        /// </summary>
        private static readonly ConcurrentDictionary<string, BinderConfiguration> BinderConfigDic = new ConcurrentDictionary<string, BinderConfiguration>();
        /// <summary>
        /// proto脚本生成工具
        /// </summary>
        private static readonly SchemaGenerator SchemaGenerator = new SchemaGenerator();

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

        /// <summary>
        /// 生成proto文件文本内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string GenerateProtoFile<T>()
        {
            var result = string.Empty;
            var marshallerFactories = GetMarshallerFactory<T>();
            if (marshallerFactories == null || !marshallerFactories.Any())
            {
                return result;
            }
            var key = typeof(T).FullName;
            BinderConfigDic.TryGetValue(key, out var binderConfig);
            if (binderConfig == null)
            {
                binderConfig = BinderConfiguration.Create(marshallerFactories);
                BinderConfigDic.TryAdd(key, binderConfig);
            }
            SchemaGenerator.BinderConfiguration = binderConfig;
            result = SchemaGenerator.GetSchema<T>();
            if (result.Contains("import \"protobuf-net/bcl.proto\"; // schema for protobuf-net's handling of core .NET types"))
            {
                result = result.Replace("import \"protobuf-net/bcl.proto\"; // schema for protobuf-net's handling of core .NET types", "import \"Public/bcl.proto\";");
            }
            return result;
        }

        /// <summary>
        /// 生成proto文件文本内容
        /// </summary>
        /// <param name="assemblyNameKeyword">程序集名称关键词</param>
        /// <param name="types">types</param>
        /// <param name="exportDirPath">导出目录</param>
        /// <param name="exportProtoType">导出类型</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static void GenerateProtoFile(string assemblyNameKeyword, List<Type> types = null, string exportDirPath = "protoFiles", ExportProtoType exportProtoType = ExportProtoType.AllIn)
        {
            if (string.IsNullOrWhiteSpace(assemblyNameKeyword))
            {
                return;
            }
            if (types == null || !types.Any())
            {
                return;
            }
            BinderConfigDic.TryGetValue(assemblyNameKeyword, out var binderConfig);
            if (binderConfig == null)
            {
                var marshallerFactories = GetMarshallerFactoryByKeyword(assemblyNameKeyword);
                binderConfig = BinderConfiguration.Create(marshallerFactories);
                BinderConfigDic.TryAdd(assemblyNameKeyword, binderConfig);
            }
            SchemaGenerator.BinderConfiguration = binderConfig;
            var dirPath = $"{AppContext.BaseDirectory}{(string.IsNullOrWhiteSpace(exportDirPath) ? "protoFiles" : exportDirPath)}".TrimEnd('\\') + "\\";
            var blcProtoFilePath = $"{AppContext.BaseDirectory}bcl.proto";
            var copyBlcProtoFilePath = $"{dirPath}Public\\bcl.proto";
            string protoFilePath;
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            if (!Directory.Exists($"{dirPath}Public\\"))
            {
                Directory.CreateDirectory($"{dirPath}Public\\");
            }
            //按命名空间/类名建文件
            if (exportProtoType == ExportProtoType.TypeName)
            {
                types.ForEach(type => {
                    protoFilePath = $"{dirPath}{type.Namespace}\\";
                    if (!Directory.Exists(protoFilePath))
                    {
                        Directory.CreateDirectory(protoFilePath);
                    }
                    protoFilePath += $"{type.Name}.proto";
                    var protoStr = SchemaGenerator.GetSchema(type);
                    if (protoStr.Contains("import \"protobuf-net/bcl.proto\"; // schema for protobuf-net's handling of core .NET types") && File.Exists(blcProtoFilePath) && !File.Exists(copyBlcProtoFilePath))
                    {
                        protoStr = protoStr.Replace("import \"protobuf-net/bcl.proto\"; // schema for protobuf-net's handling of core .NET types", "import \"Public/bcl.proto\";");
                        File.Copy(blcProtoFilePath, copyBlcProtoFilePath, true);
                    }
                    File.WriteAllText(protoFilePath, protoStr);
                });
            }
            else
            {
                //同个命名空间下的放同个文件里
                var nameSpaceTypes = types.GroupBy(g => g.Namespace).Select(sel => new { NameSpace = sel.Key, Types = sel.ToList() }).ToList();
                nameSpaceTypes.ForEach(type =>
                {
                    if (type.Types == null || !type.Types.Any()) return;
                    protoFilePath = $"{dirPath}{assemblyNameKeyword}\\";
                    if (!Directory.Exists(protoFilePath))
                    {
                        Directory.CreateDirectory(protoFilePath);
                    }
                    protoFilePath += $"{type.NameSpace}.proto";
                    var protoStr = SchemaGenerator.GetSchema(type.Types.ToArray());
                    if (protoStr.Contains("import \"protobuf-net/bcl.proto\"; // schema for protobuf-net's handling of core .NET types") && File.Exists(blcProtoFilePath) && !File.Exists(copyBlcProtoFilePath))
                    {
                        protoStr = protoStr.Replace("import \"protobuf-net/bcl.proto\"; // schema for protobuf-net's handling of core .NET types", "import \"Public/bcl.proto\";");
                        File.Copy(blcProtoFilePath, copyBlcProtoFilePath, true);
                    }
                    File.WriteAllText(protoFilePath, protoStr);
                });

            }
        }

    }
}
