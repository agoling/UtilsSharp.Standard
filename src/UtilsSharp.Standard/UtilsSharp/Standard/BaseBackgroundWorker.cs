using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace UtilsSharp.Standard
{
    /// <summary>
    /// BackgroundWorker后台操作基类
    /// </summary>
    public abstract class BaseBackgroundWorker
    {
        /// <summary>
        /// 后台操作对象
        /// </summary>
        protected readonly BackgroundWorker BgWorker;

        /// <summary>
        /// 线程名称
        /// </summary>
        public readonly string ThreadName;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="threadName">线程名称</param>
        protected BaseBackgroundWorker(string threadName = "")
        {
            ThreadName = !string.IsNullOrEmpty(threadName) ? threadName : Guid.NewGuid().ToString().Replace("-", "");
            BgWorker = new BackgroundWorker();
            BgWorker.DoWork += BgWorker_DoWork;
            BgWorker.ProgressChanged += BgWorker_ProgressChanged;
            BgWorker.RunWorkerCompleted += BgWorker_RunWorkerCompleted;
            BgWorker.Disposed += BgWorker_Disposed;
        }

        /// <summary>
        /// 开始执行后台操作
        /// </summary>
        public void RunWorkerAsync()
        {
            BgWorker.RunWorkerAsync();
        }

        /// <summary>
        /// 开始执行后台操作
        /// </summary>
        /// <typeparam name="T">参数模型</typeparam>
        /// <param name="workerContext">参数</param>
        public void RunWorkerAsync<T>(T workerContext)
        {
            BgWorker.RunWorkerAsync(workerContext);
        }

        /// <summary>
        /// 报告进度引发ProgressChanged事件
        /// </summary>
        /// <param name="percentProgress">进度</param>
        public void ReportProgress(int percentProgress)
        {
            BgWorker.ReportProgress(percentProgress);
        }

        /// <summary>
        /// 指示是否正在运行异步操作
        /// </summary>
        /// <returns></returns>
        public bool IsBusy()
        {
            return BgWorker.IsBusy;
        }

        /// <summary>
        /// 取消挂起的后台操作
        /// </summary>
        public void CancelAsync()
        {
            BgWorker.CancelAsync();
        }

        /// <summary>
        /// 指示应用程序是否已请求取消后台操作
        /// </summary>
        /// <returns></returns>
        public bool CancellationPending()
        {
            return BgWorker.CancellationPending;
        }

        /// <summary>
        /// 开始执行后台操作事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public abstract void BgWorker_DoWork(object sender, DoWorkEventArgs e);

        /// <summary>
        /// 进度改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public abstract void BgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e);

        /// <summary>
        /// 执行完成后台操作事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public abstract void BgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e);

        /// <summary>
        /// 销毁后台操作事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public abstract void BgWorker_Disposed(object sender, EventArgs e);
    }
}
