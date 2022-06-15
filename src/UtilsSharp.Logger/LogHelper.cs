using System;
using NLog;

namespace UtilsSharp.Logger
{
    /// <summary>
    /// 日志管理系统
    /// </summary>
    public class LogHelper
    {
        /// <summary>
        /// 追踪
        /// </summary>
        /// <param name="message">日志标题</param>
        /// <param name="logId">日志Id</param>
        /// <param name="parameters">入参</param>
        /// <param name="userId">用户Id</param>
        /// <param name="func">功能名称</param>
        /// <param name="requestUrl">请求地址</param>
        /// <returns></returns>
        public static string Trace(string message, string logId = "", string parameters = "", string userId = "", string func = "", string requestUrl = "")
        {
            return WriteLog(LogLevel.Trace, message, "",null, logId, requestUrl, parameters, userId, func);
        }

        /// <summary>
        /// 追踪
        /// </summary>
        /// <param name="entity">日志信息实体</param>
        /// <returns></returns>
        public static string Trace(BaseLogEntity entity)
        {
            return WriteLog(LogLevel.Trace, entity.Message, entity.DetailTrace, null, entity.LogId, entity.RequestUrl, entity.Params, entity.UserId, entity.Func);
        }

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="message">日志标题</param>
        /// <param name="logId">日志Id</param>
        /// <param name="parameters">入参</param>
        /// <param name="userId">阿里Id</param>
        /// <param name="func">功能名称</param>
        /// <param name="requestUrl">请求地址</param>
        /// <returns></returns>
        public static string Info(string message, string logId = "", string parameters = "", string userId = "", string func = "", string requestUrl = "")
        {
            return WriteLog(LogLevel.Info, message, "",null, logId, requestUrl, parameters, userId, func);
        }

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="entity">日志信息实体</param>
        /// <returns></returns>
        public static string Info(BaseLogEntity entity)
        {
            return WriteLog(LogLevel.Info, entity.Message, entity.DetailTrace, null, entity.LogId, entity.RequestUrl, entity.Params, entity.UserId, entity.Func);
        }

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="message">日志标题</param>
        /// <param name="logId">日志Id</param>
        /// <param name="parameters">入参</param>
        /// <param name="userId">阿里Id</param>
        /// <param name="func">功能名称</param>
        /// <param name="requestUrl">请求地址</param>
        /// <returns></returns>
        public static string Debug(string message, string logId = "", string parameters = "", string userId = "", string func = "", string requestUrl = "")
        {
            return WriteLog(LogLevel.Debug, message, "",null, logId, requestUrl, parameters, userId, func);
        }

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="entity">日志信息实体</param>
        /// <returns></returns>
        public static string Debug(BaseLogEntity entity)
        {
            return WriteLog(LogLevel.Debug, entity.Message, entity.DetailTrace, null, entity.LogId, entity.RequestUrl, entity.Params, entity.UserId, entity.Func);
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="message">日志标题</param>
        /// <param name="logId">日志Id</param>
        /// <param name="parameters">入参</param>
        /// <param name="userId">用户Id</param>
        /// <param name="func">功能名称</param>
        /// <param name="requestUrl">请求地址</param>
        /// <returns></returns>
        public static string Warn(string message, string logId = "", string parameters = "", string userId = "", string func = "", string requestUrl = "")
        {
            return WriteLog(LogLevel.Warn, message, "",null, logId, requestUrl, parameters, userId, func);
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="entity">日志信息实体</param>
        /// <returns></returns>
        public static string Warn(BaseLogEntity entity)
        {
            return WriteLog(LogLevel.Warn, entity.Message, entity.DetailTrace, null, entity.LogId, entity.RequestUrl, entity.Params, entity.UserId, entity.Func);
        }

        /// <summary>
        /// 异常错误
        /// </summary>
        /// <param name="message">日志标题</param>
        /// <param name="ex">Exception</param>
        /// <param name="logId">日志Id</param>
        /// <param name="errCode">错误码</param>
        /// <param name="parameters">入参</param>
        /// <param name="userId">阿里Id</param>
        /// <param name="func">功能名称</param>
        /// <param name="requestUrl">请求地址</param>
        /// <returns></returns>
        public static string Error(string message, Exception ex, string logId = "", string errCode = "",string parameters = "", string userId = "", string func = "", string requestUrl = "")
        {
            return WriteLog(LogLevel.Error, message, "",ex, logId, requestUrl, parameters, userId, func, errCode);
        }

        /// <summary>
        /// 异常错误
        /// </summary>
        /// <param name="entity">日志信息实体</param>
        /// <returns></returns>
        public static string Error(ErrorEntity entity)
        {
            return WriteLog(LogLevel.Error, entity.Message, entity.DetailTrace, entity.Ex, entity.LogId, entity.RequestUrl, entity.Params, entity.UserId, entity.Func);
        }


        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="message">日志标题</param>
        /// <param name="logId">日志Id</param>
        /// <param name="parameters">入参</param>
        /// <param name="userId">用户Id</param>
        /// <param name="func">功能名称</param>
        /// <param name="requestUrl">请求地址</param>
        /// <returns></returns>
        public static string Fatal(string message, string logId = "", string parameters = "", string userId = "", string func = "", string requestUrl = "")
        {
            return WriteLog(LogLevel.Fatal, message, "",null, logId, requestUrl, parameters, userId, func);
        }

        /// <summary>
        /// 致命
        /// </summary>
        /// <param name="entity">日志信息实体</param>
        /// <returns></returns>
        public static string Fatal(BaseLogEntity entity)
        {
            return WriteLog(LogLevel.Fatal, entity.Message, entity.DetailTrace, null, entity.LogId, entity.RequestUrl, entity.Params, entity.UserId, entity.Func);
        }


        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="message">日志标题</param>
        /// <param name="logId">日志Id</param>
        /// <param name="parameters">入参</param>
        /// <param name="userId">用户Id</param>
        /// <param name="func">功能名称</param>
        /// <param name="requestUrl">请求地址</param>
        /// <returns></returns>
        public static string Off(string message, string logId = "", string parameters = "", string userId = "", string func = "", string requestUrl = "")
        {
            return WriteLog(LogLevel.Off, message, "",null, logId, requestUrl, parameters, userId, func);
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="entity">日志信息实体</param>
        /// <returns></returns>
        public static string Off(BaseLogEntity entity)
        {
            return WriteLog(LogLevel.Off, entity.Message, entity.DetailTrace, null, entity.LogId, entity.RequestUrl, entity.Params, entity.UserId, entity.Func);
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="level">日志等级</param>
        /// <param name="message">日志标题</param>
        /// <param name="detailTrace">日志详情</param>
        /// <param name="ex">Exception</param>
        /// <param name="logId">日志Id</param>
        /// <param name="requestUrl">请求地址</param>
        /// <param name="parameters">入参</param>
        /// <param name="userId">用户Id</param>
        /// <param name="func">功能名称</param>
        /// <param name="errCode">错误码</param>
        /// <returns></returns>
        public static string WriteLog(LogLevel level, string message,string detailTrace,Exception ex = null, string logId = "", string requestUrl = "", string parameters = "", string userId = "", string func = "", string errCode = "")
        {
            return WriteLog(new LogEntity()
            {
                Level = level,
                Message = message,
                DetailTrace= detailTrace,
                LogId = logId,
                Params = parameters,
                UserId = userId,
                RequestUrl = requestUrl,
                Ex = ex,
                Func = func,
                ErrorCode = errCode
            });
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="model">日志模型</param>
        /// <returns></returns>
        public static string WriteLog(LogEntity model)
        {
            if (string.IsNullOrEmpty(model.LogId))
            {
                model.LogId = Guid.NewGuid().ToString("N");
            }
            var log = LogManager.GetCurrentClassLogger();
            var logInfo = new LogEventInfo(model.Level, "", "")
            {
                Message = model.Message, 
                Exception = model.Ex,
                Properties =
                {
                    ["LogId"] = model.LogId,
                    ["DetailTrace"] = model.DetailTrace,
                    ["RequestUrl"] = model.RequestUrl,
                    ["Params"] = model.Params,
                    ["UserId"] = model.UserId,
                    ["Func"] = model.Func,
                    ["ErrorCode"] = model.ErrorCode
                }
            };
            log.Log(logInfo);
            return model.LogId;
        }
    }
}
