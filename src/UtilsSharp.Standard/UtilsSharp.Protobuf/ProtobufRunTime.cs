using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ProtoBuf;
using ProtoBuf.Meta;
using UtilsSharp.Standard.Interface;

namespace UtilsSharp.Protobuf
{
    /// <summary>
    /// 注册ProtoContract、ProtoInclude、ProtoMember
    /// </summary>
    public class ProtobufRunTime
    {
        public static void Initialize()
        {
            //获取基于ProtoEntity的类
            var enumerable = AssemblyHelper.GetAllAssemblies();
            var fieldsTag = new Dictionary<MetaType, int>();
            var types = new List<Type>();
            foreach (var ts in enumerable.Select(item => item.GetTypes()
                         .Where(x => typeof(IProtobufEntity).IsAssignableFrom(x) && typeof(IProtobufEntity) != x)
                         .ToList()).Where(ts => ts.Count > 0))
            {
                types.AddRange(ts);
            }

            //注册ProtoContract、ProtoInclude、ProtoMember
            foreach (var t in types)
            {
                var meta = RuntimeTypeModel.Default.Add(t, false);
                //注册基类
                var baseType = t.BaseType;
                if (baseType != null && baseType != typeof(object))
                {
                    var baseTypeMeta = RuntimeTypeModel.Default.Add(baseType, false);
                    if (fieldsTag.ContainsKey(baseTypeMeta))
                    {
                        fieldsTag[baseTypeMeta] += 1;
                        baseTypeMeta.AddSubType(fieldsTag[baseTypeMeta], t);
                    }
                    else
                    {
                        var fields = baseTypeMeta.GetFields();
                        fieldsTag.Add(baseTypeMeta, fields.Length + 1);
                        baseTypeMeta.AddSubType(fieldsTag[baseTypeMeta], t);
                    }
                }

                //注册字段
                foreach (var p in t.GetProperties(BindingFlags.Instance | BindingFlags.Public |
                                                  BindingFlags.DeclaredOnly).Where(x => x.GetSetMethod() != null))
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

            }
        }
    }
}
