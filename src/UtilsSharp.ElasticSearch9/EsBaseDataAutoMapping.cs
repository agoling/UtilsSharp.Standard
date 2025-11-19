using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Mapping;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace UtilsSharp.ElasticSearch9
{
    /// <summary>
    /// Es基础实体自动映射
    /// </summary>
    public abstract class EsBaseDataAutoMapping<T> : EsBaseDataMapping where T : class, new()
    {
        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="client">es客户端</param>
        /// <param name="index">索引名称</param>
        public override void Create(ElasticsearchClient client, string index)
        {
            var exists = client.Indices.Exists(index).Exists;
            if (exists) return;
            client.Indices.Create(index, c => c
                .Mappings<T>(m => m.Properties(p => p.AutoMap()))
                .Settings(s => s
                    .NumberOfReplicas(0)
                    .NumberOfShards(NumberOfShards)
                    .MaxRescoreWindow(MaxResultWindow)
                ));
        }

        /// <summary>
        /// 实体映射
        /// </summary>
        /// <param name="client">es客户端</param>
        /// <param name="index">索引名称</param>
        public override void EntityMapping(ElasticsearchClient client, string index)
        {
            client.Indices.PutMapping<T>(mappings => mappings.Indices(index).Properties(p => p.AutoMap()));
        }
    }

    public static class ElasticsearchMappingHelper
    {
        private static readonly MethodInfo MapPropertyGenericMethod = typeof(ElasticsearchMappingHelper)
            .GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .First(m => m.Name == nameof(MapProperty) && m.IsGenericMethodDefinition);

        public static PropertiesDescriptor<T> AutoMap<T>(this PropertiesDescriptor<T> descriptor) where T : class
        {
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var seenTypes = new ConcurrentDictionary<Type, int>();
            foreach (var property in properties)
            {
                MapProperty(descriptor, property, seenTypes);
            }
            return descriptor;
        }

        private static void MapProperty<T>(PropertiesDescriptor<T> descriptor, PropertyInfo property, ConcurrentDictionary<Type, int> seenTypes) where T : class
        {
            var propertyName = GetPropertyName(property);
            var propertyType = property.PropertyType;
            var underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;
            switch (Type.GetTypeCode(underlyingType))
            {
                case TypeCode.Object:
                    MapObjectProperty(descriptor, property, propertyName, underlyingType, seenTypes);
                    break;
                case TypeCode.Boolean:
                    descriptor.Boolean(propertyName);
                    break;
                case TypeCode.Char:
                    descriptor.Keyword(propertyName);
                    break;
                case TypeCode.SByte:
                case TypeCode.Byte:
                    descriptor.Binary(propertyName);
                    break;
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                    descriptor.IntegerNumber(propertyName);
                    break;
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    descriptor.LongNumber(propertyName);
                    break;
                case TypeCode.Single:
                    descriptor.FloatNumber(propertyName);
                    break;
                case TypeCode.Double:
                case TypeCode.Decimal:
                    descriptor.DoubleNumber(propertyName);
                    break;
                case TypeCode.DateTime:
                    descriptor.Date(propertyName);
                    break;
                case TypeCode.String:
                    var isTextAttribute = property.GetCustomAttribute<TextAttribute>(inherit: true) != null;
                    if (isTextAttribute)
                    {
                        descriptor.Text(propertyName);
                    }
                    else
                    {
                        descriptor.Keyword(propertyName);
                    }
                    break;
                default:
                    descriptor.Object(propertyName);
                    break;
            }
        }

        private static string GetPropertyName(PropertyInfo property)
        {
            var propertyName = property.Name;
            if (!string.IsNullOrEmpty(propertyName))
            {
                propertyName = char.ToLower(propertyName[0]) + propertyName.Substring(1);
            }
            return propertyName;
        }

        private static Action<PropertiesDescriptor<T>> CreateMapPropertiesAction<T>(Type objectType, ConcurrentDictionary<Type, int> seenTypes) where T : class
        {
            return p =>
            {
                if (seenTypes.TryGetValue(objectType, out var num) && num > 0)
                {
                    return;
                }
                seenTypes.AddOrUpdate(objectType, 0, (Func<Type, int, int>)((t, i) => ++i));
                var properties = objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var mapPropertyMethod = MapPropertyGenericMethod.MakeGenericMethod(typeof(T));
                foreach (var property in properties)
                {
                    if (property.GetIndexParameters().Length > 0)
                    {
                        continue;
                    }
                    mapPropertyMethod.Invoke(null, new object[] { p, property, seenTypes });
                }
            };
        }

        private static void MapObjectProperty<T>(PropertiesDescriptor<T> descriptor, PropertyInfo property, string propertyName, Type objectType, ConcurrentDictionary<Type, int> seenTypes) where T : class
        {
            var isNestedAttribute = property.GetCustomAttribute<NestedAttribute>(inherit: true) != null;
            if (isNestedAttribute)
            {
                if (ShouldRecursivelyMap(objectType))
                {
                    var mapPropertiesAction = CreateMapPropertiesAction<T>(objectType, seenTypes);
                    descriptor.Nested(propertyName, obj => obj.Properties(mapPropertiesAction));
                    return;
                }
                descriptor.Nested(propertyName);
                return;
            }
            if (ShouldRecursivelyMap(objectType))
            {
                var mapPropertiesAction = CreateMapPropertiesAction<T>(objectType, seenTypes);
                descriptor.Object(propertyName, obj => obj.Properties(mapPropertiesAction));
                return;
            }
            descriptor.Object(propertyName);
        }

        private static bool ShouldRecursivelyMap(Type type)
        {
            return type.IsClass &&
                   type != typeof(string) &&
                   !type.IsArray &&
                   !typeof(System.Collections.IEnumerable).IsAssignableFrom(type) &&
                   !type.IsPrimitive;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class TextAttribute : Attribute { }
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class NestedAttribute : Attribute { }
}
