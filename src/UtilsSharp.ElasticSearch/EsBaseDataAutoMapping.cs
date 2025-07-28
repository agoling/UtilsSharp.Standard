using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Nest;

namespace UtilsSharp.ElasticSearch
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
            var exists = client.IndexExists(index).Exists;
            if (exists) return;
            client.CreateIndex(index, c => c
                .Mappings(m => m
                    .Map<T>(mm => mm
                        .AutoMap(new AllStringToKeywordValuesPropertyVisitor()).AllField(a => a.Enabled(false))
                    )
                )
                .Settings(s => s
                    .NumberOfReplicas(0)
                    .NumberOfShards(NumberOfShards)
                    .Setting("index.max_result_window", MaxResultWindow)
                )
            );
        }

        /// <summary>
        /// 实体映射
        /// </summary>
        /// <param name="client">es客户端</param>
        /// <param name="index">索引名称</param>
        public override void EntityMapping(ElasticClient client, string index)
        {
            client.Map<T>(m => m.AutoMap(new AllStringToKeywordValuesPropertyVisitor()).AllField(a => a.Enabled(false)).Index(index));
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
            else
            {
                type.Type = "text"; // ES5 中需要显式设置
            }
        }
    }
}
