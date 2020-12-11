using System;
using System.Collections.Generic;
using Nest;

namespace ElasticSearch7
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
        /// <returns></returns>
        public virtual IndexResponse Save(T t)
        {
            var r=EsClient.Index(t, i => i.Index(CurrentIndex).Refresh(Elasticsearch.Net.Refresh.True));
            return r;
        }

        /// <summary>
        /// 批量保存
        /// </summary>
        /// <param name="entitys">参数</param>
        public virtual void Save(List<T> entitys)
        {
            if (entitys == null || entitys.Count == 0) return;
            EsClient.IndexMany(entitys, CurrentIndex);
            EsClient.Indices.Refresh(CurrentIndex);
        }

        /// <summary>
        /// 增量更新
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="incrementModifyParams">增量参数：key-字段,value-修改的值</param>
        /// <returns></returns>
        public virtual UpdateResponse<T> IncrementModify(string id, Dictionary<string, object> incrementModifyParams)
        {
            var r=new UpdateResponse<T>();
            if(incrementModifyParams == null) return r;
            var updatePath = new DocumentPath<T>(id);
            r= EsClient.Update<T, object>(updatePath, u => u.Doc(incrementModifyParams).Refresh(Elasticsearch.Net.Refresh.True));
            return r;
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
            if (esResult.Hits.Count <= 0) return;
            foreach (var esResultHits in esResult.Hits)
            {
                if (!esResultHits.Found) continue;
                EsClient.Delete<T>(esResultHits.Id, f => f.Index(CurrentIndex));
            }
            EsClient.Indices.Refresh(CurrentIndex);
        }
        
    }
}
