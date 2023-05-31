using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DViewEdge
{
    public class Log
    {
        // 文件路径
        private readonly string Path;

        public Log()
        {
            string BasePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            Path = BasePath + Constants.LogFile;
        }

        /// <summary>
        /// 写入日志内容
        /// </summary>
        /// <param name="msg">msg</param>
        public void WriteAppend(string msg)
        {
            StreamWriter sw = null;
            try
            {
                sw = File.AppendText(Path);
                sw.WriteLine(Utils.TimeNow() + " " + msg);
                sw.Flush();
                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }
            }
        }
    }
}
