using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace UtilsSharp.Kafka
{
    public class ListenResult : IDisposable
    {
        readonly CancellationTokenSource _cancellationTokenSource;

        /// <summary>
        /// CancellationToken
        /// </summary>
        public CancellationToken Token => _cancellationTokenSource.Token;

        /// <summary>
        /// 是否已停止
        /// </summary>
        public bool Stoped
        {
            get { return _cancellationTokenSource.IsCancellationRequested; }
        }

        public ListenResult()
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// 停止监听
        /// </summary>
        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
