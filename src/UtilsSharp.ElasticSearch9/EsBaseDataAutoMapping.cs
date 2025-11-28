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
                    .MaxResultWindow(MaxResultWindow)
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
            if (Type.GetTypeCode(underlyingType) == TypeCode.Object)
            {
                MapObjectProperty(descriptor, property, propertyName, underlyingType, seenTypes);
                return;
            }
            var tryMapSimpleType = TryMapSimpleType(descriptor, property, propertyName, underlyingType);
            if (!tryMapSimpleType)
            {
                descriptor.Object(propertyName);
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
            var hasNestedAttribute = property.GetCustomAttribute<NestedAttribute>(inherit: true) != null;
            var simpleMapper = hasNestedAttribute
                ? new Action<string>(name => descriptor.Nested(name))
                : new Action<string>(name => descriptor.Object(name));
            var complexMapper = hasNestedAttribute
                ? new Action<string, Action<PropertiesDescriptor<T>>>((name, mapAction) => descriptor.Nested(name, obj => obj.Properties(mapAction)))
                : new Action<string, Action<PropertiesDescriptor<T>>>((name, mapAction) => descriptor.Object(name, obj => obj.Properties(mapAction)));
            MapObjectPropertyCore(descriptor, property, propertyName, objectType, seenTypes, simpleMapper, complexMapper);
        }

        private static void MapObjectPropertyCore<T>(PropertiesDescriptor<T> descriptor, PropertyInfo property, string propertyName, Type objectType, ConcurrentDictionary<Type, int> seenTypes, Action<string> simpleMapper, Action<string, Action<PropertiesDescriptor<T>>> complexMapper) where T : class
        {
            if (objectType.IsClass && objectType != typeof(string) && !objectType.IsPrimitive)
            {
                if (objectType.IsArray || typeof(System.Collections.IEnumerable).IsAssignableFrom(objectType))
                {
                    var elementType = GetEnumerableElementType(objectType);
                    if (elementType != null)
                    {
                        var underlyingElementType = Nullable.GetUnderlyingType(elementType) ?? elementType;
                        if (TryMapSimpleType(descriptor, property, propertyName, underlyingElementType))
                        {
                            return;
                        }
                        if (underlyingElementType.IsClass && underlyingElementType != typeof(string) && !underlyingElementType.IsPrimitive)
                        {
                            var mapArrayPropertiesAction = CreateMapPropertiesAction<T>(underlyingElementType, seenTypes);
                            complexMapper(propertyName, mapArrayPropertiesAction);
                            return;
                        }
                    }
                    simpleMapper(propertyName);
                    return;
                }
                var mapPropertiesAction = CreateMapPropertiesAction<T>(objectType, seenTypes);
                complexMapper(propertyName, mapPropertiesAction);
                return;
            }
            simpleMapper(propertyName);
        }

        private static bool TryMapSimpleType<T>(PropertiesDescriptor<T> descriptor, PropertyInfo property, string propertyName, Type type) where T : class
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean:
                    descriptor.Boolean(propertyName);
                    return true;
                case TypeCode.Char:
                    descriptor.Keyword(propertyName);
                    return true;
                case TypeCode.SByte:
                case TypeCode.Byte:
                    descriptor.Binary(propertyName);
                    return true;
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                    descriptor.IntegerNumber(propertyName);
                    return true;
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    descriptor.LongNumber(propertyName);
                    return true;
                case TypeCode.Single:
                    descriptor.FloatNumber(propertyName);
                    return true;
                case TypeCode.Double:
                case TypeCode.Decimal:
                    descriptor.DoubleNumber(propertyName);
                    return true;
                case TypeCode.DateTime:
                    descriptor.Date(propertyName);
                    return true;
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
                    return true;
                default:
                    return false;
            }
        }

        private static Type GetEnumerableElementType(Type enumerableType)
        {
            if (enumerableType.IsArray)
            {
                return enumerableType.GetElementType();
            }
            if (enumerableType.IsGenericType)
            {
                var genericArguments = enumerableType.GetGenericArguments();
                if (genericArguments.Length == 1)
                {
                    return genericArguments[0];
                }
            }
            var interfaceType = enumerableType.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(System.Collections.Generic.IEnumerable<>));
            return interfaceType?.GetGenericArguments().FirstOrDefault();
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class TextAttribute : Attribute { }
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class NestedAttribute : Attribute { }
}
