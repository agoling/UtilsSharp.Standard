using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using MySql.Data.MySqlClient;
using OptionConfig;

namespace MySql
{
    /// <summary>
    /// MySql基础操作
    /// </summary>
    /// <typeparam name="T">模型</typeparam>
    public abstract class MySqlBaseDataMapping<T> where T : IBaseEntity
    {
        /// <summary>
        /// 初始化
        /// </summary>
        private static void Init()
        {
            //标识使用MySql
            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.MySQL);
        }

        /// <summary>
        /// 初始化链接
        /// </summary>
        /// <returns></returns>
        public IDbConnection GetDbConnection()
        {
            Init();
            return new MySqlConnection(MySqlConfig.MySqlConnection);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="t">模型</param>
        /// <returns></returns>
        public virtual int Add(T t)
        {
            using (var conn = GetDbConnection())
            {
                t.CreateTime = DateTime.Now;
                return conn.Insert(t).GetValueOrDefault();
            }
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="t">模型</param>
        /// <returns></returns>
        public virtual int Update(T t)
        {
            using (var conn = GetDbConnection())
            {
                return conn.Update(t);
            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="t">模型</param>
        /// <returns></returns>
        public virtual int Save(T t)
        {
            return t.Id > 0 ? Update(t) : Add(t);
        }

        /// <summary>
        /// 删除
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
        /// 根据id获取
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public virtual T Get(long id)
        {
            using (var conn = GetDbConnection())
            {
                return conn.Get<T>(id);
            }
        }

        /// <summary>
        /// 获取所有的列表
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<T> GetAllList()
        {
            using (var conn = GetDbConnection())
            {
                return conn.GetList<T>();
            }
        }


        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="entitys"></param>
        public virtual void BulkAdd(List<T> entitys)
        {
            using (var conn = GetDbConnection())
            {
                entitys.ForEach(entity =>
                {
                    conn.Insert(entity);
                });
            }
        }
    }

}
