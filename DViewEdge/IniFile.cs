using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace DViewEdge
{
    class IniFile
    {
        readonly string Path;

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        /// <summary>
        /// 创建句柄
        /// </summary>
        /// <param name="iniPath"></param>
        public IniFile(string iniPath = null)
        {
            Path = new FileInfo(iniPath).FullName;
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="Key">项目</param>
        /// <param name="Section">段落</param>
        /// <returns>内容</returns>
        public string Read(string Key, string Section = null)
        {
            var RetVal = new StringBuilder(255);
            _ = GetPrivateProfileString(Section, Key, "", RetVal, 255, Path);
            return RetVal.ToString();
        }

        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="Key">项目</param>
        /// <param name="Value">内容</param>
        /// <param name="Section">段落</param>
        public void Write(string Key, string Value, string Section = null)
        {
            WritePrivateProfileString(Section, Key, Value, Path);
        }
    }
}