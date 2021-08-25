using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UtilsSharp.Standard;

namespace UtilsSharp
{
    /// <summary>
    /// windows命令帮助类
    /// </summary>
    public class WinCmdHelper
    {
        #region 执行Dos命令
        /// <summary>
        /// 执行Dos命令
        /// </summary>
        /// <param name="cmd">Dos命令及参数</param>
        /// <param name="isShowCmdWindow">是否显示cmd窗口</param>
        /// <param name="isCloseCmdProcess">执行完毕后是否关闭cmd进程</param>
        /// <returns></returns>
        public static BaseResult<string> RunCommand(string cmd, bool isShowCmdWindow, bool isCloseCmdProcess)
        {
            var result=new BaseResult<string>();
            try
            {
                var strBuilder=new StringBuilder();
                var p = new Process
                {
                    StartInfo =
                    {
                        FileName = "cmd",
                        UseShellExecute = false,
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        CreateNoWindow = !isShowCmdWindow
                    }
                };
                p.OutputDataReceived += delegate (object sender, DataReceivedEventArgs e) {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        strBuilder.Append($"{e.Data}\n");
                    }
                };
                p.Start();
                var cmdWriter = p.StandardInput;
                p.BeginOutputReadLine();
                if (!string.IsNullOrEmpty(cmd))
                {
                    cmdWriter.WriteLine(cmd);
                }
                cmdWriter.Close();
                p.WaitForExit();
                result.Result = strBuilder.ToString();
                if (isCloseCmdProcess)
                {
                    p.Close();
                }
                result.Msg = $"成功执行Dos命令[{cmd}]!";
                return result;
            }
            catch (Exception ex)
            {
                result.SetError($"执行命令失败，请检查输入的命令是否正确:{ex.Message}",5000);
                return result;
            }
        }

        #endregion

        #region 判断指定的进程是否在运行中

        /// <summary>
        /// 判断指定的进程是否在运行中
        /// </summary>
        /// <param name="processName">要判断的进程名称，不包括扩展名exe</param>
        /// <param name="processFileName">进程文件的完整路径</param>
        /// <returns>存在返回true，否则返回false</returns>
        public static bool IsProcessExists(string processName, string processFileName)
        {
            var processes = Process.GetProcessesByName(processName);
            foreach (var p in processes)
            {
                if (!string.IsNullOrEmpty(processFileName))
                {
                    if (p.MainModule != null && processFileName == p.MainModule.FileName)
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region 结束指定的windows进程

        /// <summary>
        /// 结束指定的windows进程如果进程存在
        /// </summary>
        /// <param name="processName">进程名称，不包含扩展名</param>
        /// <param name="processFileName">进程文件完整路径，如果为空则删除所有进程名为processName参数值的进程</param>
        public static BaseResult<string> KillProcess(string processName, string processFileName)
        {
            var result=new BaseResult<string>();
            try
            {
                var processes = Process.GetProcessesByName(processName);
                foreach (var p in processes)
                {
                    if (!string.IsNullOrEmpty(processFileName))
                    {
                        if (p.MainModule == null || processFileName != p.MainModule.FileName) continue;
                        p.Kill();
                        p.Close();
                    }
                    else
                    {
                        p.Kill();
                        p.Close();
                    }
                }

                result.Msg = "成功结束进程!";
                return result;
            }
            catch (Exception ex)
            {
                result.SetError($"结束指定的Widnows进程异常:{ex.Message}",5000);
                return result;
            }
        }

        #endregion
    }
}
