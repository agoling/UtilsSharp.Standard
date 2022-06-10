using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Kafka
{
    public class ListenResult : IDisposable
    {
        CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// CancellationToken
        /// </summary>
        public CancellationToken Token
        {
            get { return cancellationTokenSource.Token; }
        }

        /// <summary>
        /// 是否已停止
        /// </summary>
        public bool Stoped
        {
            get { return cancellationTokenSource.IsCancellationRequested; }
        }

        public ListenResult()
        {
            cancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// 停止监听
        /// </summary>
        public void Stop()
        {
            cancellationTokenSource.Cancel();
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
