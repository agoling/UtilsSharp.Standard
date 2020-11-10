using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using OptionConfig;

namespace MySql
{
    /// <summary>
    /// MySql基础数据源
    /// </summary>
    public abstract class MySqlBaseDataSource<T>: IMySqlBaseDataSource<T> where T : IBaseEntity
    {
        /// <summary>
        /// 初始化链接
        /// </summary>
        /// <returns></returns>
        public IDbConnection GetDbConnection()
        {
            return new MySqlConnection(MySqlConfig.MySqlConnection);
        }

        /// <summary>
        /// 单条保存
        /// </summary>
        /// <param name="t">参数</param>
        public virtual int Save(T t)
        {
            using (var conn = GetDbConnection())
            {
                if (t.Id > 0)
                {
                    return conn.Update(t);
                }
                t.CreateTime = DateTime.Now;
                return conn.Insert(t).GetValueOrDefault();
            }
        }

        /// <summary>
        /// 批量保存
        /// </summary>
        /// <param name="entitys">参数</param>
        public virtual void Save(List<T> entitys)
        {
            using (var conn = GetDbConnection())
            {
                entitys.ForEach(t =>
                {
                    if (t.Id > 0)
                    { 
                        conn.Update(t);
                    }
                    else
                    {
                        t.CreateTime = DateTime.Now;
                        conn.Insert(t);
                    }
                });
            }
        }

        /// <summary>
        /// 获取一条数据
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        public virtual T Get(long id)
        {
            using (var conn = GetDbConnection())
            {
                return conn.Get<T>(id);
            }
        }

        /// <summary>
        /// 单条删除
        /// </summary>
        /// <param name="t">模型</param>
        /// <returns></returns>
        public virtual int Delete(T t)
        {
            using (var conn = GetDbConnection())
            {
                return conn.Delete(t);
            }
        }

        /// <summary>
        /// 获取所有的列表
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<T> GetList()
        {
            using (var conn = GetDbConnection())
            {
                return conn.GetList<T>();
            }
        }
    }
}
