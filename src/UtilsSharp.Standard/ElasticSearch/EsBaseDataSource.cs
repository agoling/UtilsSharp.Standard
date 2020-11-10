using System;
using System.Collections.Generic;
using Nest;

namespace ElasticSearch
{
    /// <summary>
    /// Es基础数据源
    /// </summary>
    public abstract class EsBaseDataSource<T>: EsBaseDataAutoMapping<T>, IEsBaseDataSource<T> where T : class, new()
    {
        /// <summary>
        /// 单条保存
        /// </summary>
        /// <param name="t">参数</param>
        public virtual void Save(T t)
        {
            EsClient.Index(t, i => i.Index(CurrentIndex).Refresh(Elasticsearch.Net.Refresh.True));
        }

        /// <summary>
        /// 批量保存
        /// </summary>
        /// <param name="entitys">参数</param>
        public virtual void Save(List<T> entitys)
        {
            if (entitys == null || entitys.Count == 0) return;
            EsClient.IndexMany(entitys, CurrentIndex);
            EsClient.Refresh(CurrentIndex);
        }

        /// <summary>
        /// 增量更新
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="incrementModifyParams">增量参数：key-字段,value-修改的值</param>
        public virtual void IncrementModify(string id, Dictionary<string, object> incrementModifyParams)
        {
            if(incrementModifyParams == null) return;
            var updatePath = new DocumentPath<T>(id);
            EsClient.Update<T, object>(updatePath, u => u.Doc(incrementModifyParams).Refresh(Elasticsearch.Net.Refresh.True));
        }

        /// <summary>
        /// 获取一条数据
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        public virtual T Get(string id)
        {
            var esResult = EsClient.Get<T>(id, i => i.Index(CurrentIndex));
            return esResult.Found ? esResult.Source : null;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">Id</param>
        public virtual void Delete(string[] ids)
        {
            var esResult = EsClient.MultiGet(m => m.GetMany<T>(ids).Index(CurrentIndex));
            if (esResult.Documents.Count <= 0) return;
            foreach (var esResultDocument in esResult.Documents)
            {
                if (!esResultDocument.Found) continue;
                EsClient.Delete<T>(esResultDocument.Id, f => f.Index(CurrentIndex));
            }
            EsClient.Refresh(CurrentIndex);
        }
        
    }
}
