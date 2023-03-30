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
        /// �������
        /// </summary>
        /// <param name="iniPath"></param>
        public IniFile(string iniPath = null)
        {
            Path = new FileInfo(iniPath).FullName;
        }

        /// <summary>
        /// ��ȡ�ļ�
        /// </summary>
        /// <param name="Key">��Ŀ</param>
        /// <param name="Section">����</param>
        /// <returns>����</returns>
        public string Read(string Key, string Section = null)
        {
            var RetVal = new StringBuilder(255);
            _ = GetPrivateProfileString(Section, Key, "", RetVal, 255, Path);
            return RetVal.ToString();
        }

        /// <summary>
        /// д���ļ�
        /// </summary>
        /// <param name="Key">��Ŀ</param>
        /// <param name="Value">����</param>
        /// <param name="Section">����</param>
        public void Write(string Key, string Value, string Section = null)
        {
            WritePrivateProfileString(Section, Key, Value, Path);
        }
    }
}