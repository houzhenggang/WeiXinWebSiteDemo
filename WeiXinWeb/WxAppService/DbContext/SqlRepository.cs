using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SqlSugar;

namespace WxAppService.DbContext
{
    public class SqlRepository<T>  where T : class, new()
    {
        protected readonly SqlSugarClient _dbClient;
        public SqlRepository()
        {

            //SqlSugarClient sqlSugarClient = new SqlSugarClient("WxDBConnection");
            _dbClient=SqlDbContext.GetInstance();
        }

        #region 添加：单条、多条
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="isIdentity">是否包含主键</param>
        /// <returns></returns>
        public T Add(T entity, bool isIdentity = true)
        {
            using (_dbClient)
            {
                return _dbClient.Insert<T>(entity, isIdentity) as T;
            }
        }

        public object InsertEntity(T entity)
        {
            using (_dbClient)
            {
                return _dbClient.Insert(entity);
            }
        }
        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="entitys">实体集合</param>
        /// <param name="isIdentity">是否包含主键</param>
        /// <returns></returns>
        public void AddEntity(List<T> entitys, bool isIdentity = true)
        {
            using (_dbClient)
            {
                try
                {
                    _dbClient.BeginTran();
                    _dbClient.SqlBulkCopy<T>(entitys);
                }
                catch (Exception ex)
                {
                    _dbClient.RollbackTran();
                    throw ex;
                }
            }
        }
        #endregion

        #region 修改：单条、多条
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="T">数据实体</typeparam>
        /// <returns></returns>
        public bool Update(T entity)
        {
            using (_dbClient)
            {
                return _dbClient.Update(entity);
            }
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="model">更新指定列如：new { name = "数据" }</param>
        /// <param name="expression">Lambda表达式</param>
        /// <returns></returns>
        public bool Update(object model, Expression<Func<T, bool>> expression)
        {
            using (_dbClient)
            {
                try
                {
                    _dbClient.BeginTran();
                    return _dbClient.Update<T>(model, expression);
                }
                catch (Exception ex)
                {
                    _dbClient.RollbackTran();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 同时更新多条数据（事务处理）
        /// </summary>
        /// <param name="entitys">List数据实体</param>
        /// <returns>是否成功</returns>
        public bool Update(List<T> entitys)
        {
            using (_dbClient)
            {
                try
                {
                    _dbClient.BeginTran();
                    return _dbClient.SqlBulkReplace(entitys);
                }
                catch (Exception ex)
                {
                    _dbClient.RollbackTran();
                    throw ex;
                }
            }
        }
        #endregion

        #region 删除：单条，多条
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="predicate">拉姆达表达式</typeparam>
        /// <returns></returns>
        public bool Delete(Expression<Func<T, bool>> predicate)
        {
            using (_dbClient)
            {
                try
                {
                    _dbClient.BeginTran();
                    return _dbClient.Delete<T>(predicate);
                }
                catch (Exception ex)
                {
                    _dbClient.RollbackTran();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <typeparam name="entitys">List数据实体</typeparam>
        /// <returns></returns>
        public bool Delete(List<T> entitys)
        {
            using (_dbClient)
            {
                try
                {
                    _dbClient.BeginTran();
                    return _dbClient.Delete<T>(entitys);
                }
                catch (Exception ex)
                {
                    _dbClient.RollbackTran();
                    throw ex;
                }
            }
        }
        #endregion

        #region 查询 单条+原生sql写法
        /// <summary>
        /// 查询一条数据
        /// </summary>
        /// <param name="predicate">拉姆达表达式</param>
        /// <returns></returns>
        public T GetModel(Expression<Func<T, bool>> predicate)
        {
            using (_dbClient)
            {
                return _dbClient.Queryable<T>().Single(predicate);
            }
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="anyLambda">查询表达式</param>
        /// <returns>布尔值</returns>
        public bool Exist(Expression<Func<T, bool>> anyLambda)
        {
            using (_dbClient)
            {
                return _dbClient.Queryable<T>().Any(anyLambda);
            }
        }

        /// <summary>
        /// 查询记录数
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns>记录数</returns>
        public int Count(Expression<Func<T, bool>> predicate)
        {
            using (_dbClient)
            {
                return _dbClient.Queryable<T>().Where(predicate).Count();
            }
        }
        #endregion

        #region 查询 多条数据+原生sql写法
        /// <summary>
        /// 查询多条数据
        /// </summary>
        /// <param name="whereLamdba"></param>
        /// <returns></returns>
        public Queryable<T> GetList(Expression<Func<T, bool>> whereLamdba)
        {
            using (_dbClient)
            {
                return _dbClient.Queryable<T>().Where(whereLamdba);
            }
        }

        /// <summary>
        /// 查找分页数据列表 支持字符串Where
        /// </summary>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="rows">总行数</param>
        /// <param name="totalPage">分页总数</param>
        /// <param name="whereLambda">查询表达式</param>
        /// <param name="isAsc">是否升序</param>
        /// <param name="orderBy">orderBy 拉姆达表达式</param>
        /// <returns></returns>
        public List<T> GetPageList(int pageIndex, int pageSize, out int rows, out int totalPage,
            Expression<Func<T, bool>> whereLambda, bool isAsc,
            Expression<Func<T, Object>> orderBy)
        {
            using (_dbClient)
            {
                var temp = _dbClient.Queryable<T>().Where(whereLambda);
                rows = temp.Count();
                totalPage = rows % pageSize == 0 ? rows / pageSize : rows / pageSize + 1;
                temp = isAsc ? temp.OrderBy(orderBy) : temp.OrderBy(orderBy,OrderByType.Desc);
                return temp.Skip<T>(pageSize * (pageIndex - 1)).Take<T>(pageSize).ToList();
            }
        }

        /// <summary>
        /// 查找分页数据列表
        /// </summary>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="rows">总行数</param>
        /// <param name="totalPage">分页总数</param>
        /// <param name="whereLambda">查询表达式</param>
        /// <param name="orderBy">排序表达式</param>
        /// <returns></returns>
        public Queryable<T> GetPageList(int pageIndex, int pageSize, out int rows, out int totalPage, Expression<Func<T, bool>> whereLambda,
            string orderBy)
        {
            using (_dbClient)
            {
                var temp = _dbClient.Queryable<T>().Where(whereLambda);
                rows = temp.Count();
                totalPage = rows % pageSize == 0 ? rows / pageSize : rows / pageSize + 1;
                temp = temp.OrderBy(orderBy);
                return temp.Skip<T>(pageSize * (pageIndex - 1)).Take<T>(pageSize);
            }
        }

        /// <summary>
        /// 查找分页数据列表 支持字符串Where
        /// </summary>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="rows">总行数</param>
        /// <param name="totalPage">分页总数</param>
        /// <param name="whereLambda">查询表达式</param>
        /// <param name="orderBy">排序id desc,name asc</param>
        /// <returns></returns>
        public Queryable<T> GetPageList(int pageIndex, int pageSize, out int rows, out int totalPage, string whereLambda, string orderBy)
        {
            using (_dbClient)
            {
                var temp = _dbClient.Queryable<T>().Where(whereLambda);
                rows = temp.Count();
                totalPage = rows % pageSize == 0 ? rows / pageSize : rows / pageSize + 1;
                temp = temp.OrderBy(orderBy);
                return temp.Skip<T>(pageSize * (pageIndex - 1)).Take<T>(pageSize);
            }
        }
        #endregion

        #region 查询：单挑、多条数据+原生sql写法，以及存储过程
        /// <summary>
        /// 返回一条或多条动态转换dynamic
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public dynamic GetList(string strSql)
        {
            using (_dbClient)
            {
                return _dbClient.SqlQueryDynamic(strSql);
            }
        }

        /// <summary>
        /// 返回一条或多条转换json
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public string GetJson(string strSql)
        {
            using (_dbClient)
            {
                return _dbClient.SqlQueryJson(strSql);
            }
        }

        /// <summary>
        /// 可以是存储过程也可以根据语句查询集合
        /// </summary>
        /// <param name="sql">exec sp_school @p1,@p2</param>
        /// <param name="whereObj">new { p1 = 1, p2 = 2 }</param>
        /// <returns></returns>
        public List<T> GetSqlList(string sql, object whereObj = null)
        {
            using (_dbClient)
            {
                return _dbClient.SqlQuery<T>(sql, whereObj);
            }
        }

        /// <summary>
        /// 可以是存储过程也可以根据语句查询集合
        /// </summary>
        /// <param name="sql">exec sp_school @p1,@p2</param>
        /// <returns></returns>
        public List<T> GetSqlList(string sql)
        {
            using (_dbClient)
            {
                return _dbClient.SqlQuery<T>(sql);
            }
        }

        #endregion
        
    }
}
