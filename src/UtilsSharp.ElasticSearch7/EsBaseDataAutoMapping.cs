using System;
using System.Collections.Generic;
using System.Reflection;
using Nest;

namespace UtilsSharp.ElasticSearch7
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
        public override void Create(ElasticClient client, string index)
        {
            var exists = client.Indices.Exists(index).Exists;
            if (exists) return;
            client.Indices.Create(index, c => c
                .Map<T>(m => m.AutoMap(new AllStringToKeywordValuesPropertyVisitor()))
                .Settings(s => s
                    .NumberOfReplicas(0)
                    .NumberOfShards(NumberOfShards)
                    .Setting(UpdatableIndexSettings.MaxResultWindow, MaxResultWindow)
                ));
        }

        /// <summary>
        /// 实体映射
        /// </summary>
        /// <param name="client">es客户端</param>
        /// <param name="index">索引名称</param>
        public override void EntityMapping(ElasticClient client, string index)
        {
            client.Map<T>(m => m.AutoMap(new AllStringToKeywordValuesPropertyVisitor()).Index(index));
        }
    }

    /// <summary>
    /// es字段映射string类型观察者
    /// </summary>
    public class AllStringToKeywordValuesPropertyVisitor : NoopPropertyVisitor
    {
        /// <summary>
        /// 访客模式
        /// </summary>
        public override void Visit(
            ITextProperty type,
            PropertyInfo propertyInfo,
            ElasticsearchPropertyAttributeBase attribute)
        {
            var isTextAttribute = propertyInfo.GetCustomAttribute<TextAttribute>(inherit: true) != null;
            if (!isTextAttribute)
            {
                type.Type = "keyword";
                type.Fields = null;
            }
        }
    }
}
