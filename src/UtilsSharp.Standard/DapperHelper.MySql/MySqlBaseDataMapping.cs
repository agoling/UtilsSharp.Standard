using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Dapper;
using MySql.Data.MySqlClient;

namespace DapperHelper.MySql
{
    /// <summary>
    /// MySql基础操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MySqlBaseDataMapping<T> where T : IBaseEntity
    {
        //ReSharper disable once StaticMemberInGenericType
        public static readonly string MySqlConnection = ConfigurationManager.AppSettings["MySqlConnection"];
        static MySqlBaseDataMapping()
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
            return new MySqlConnection(MySqlConnection);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="t"></param>
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
        /// <param name="t"></param>
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
        /// <param name="t"></param>
        /// <returns></returns>
        public virtual int Save(T t)
        {
            if (t.Id > 0)
            {
                return Update(t);
            }
            else
            {
                return Add(t);
            }

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="t"></param>
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
        /// <param name="id"></param>
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
