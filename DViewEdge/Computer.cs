using System;
using System.Security.Cryptography;
using System.Text;

namespace DViewEdge
{
    /// <summary>
    /// Generates a 16 byte Unique Identification code of a computer
    /// Example: 4876-8DB5-EE85-69D3-FE52-8CF7-395D-2EA9
    /// </summary>
    public class FingerPrint
    {
        private static string fingerPrint = string.Empty;

        /// <summary>
        /// 获取机器唯一标识
        /// </summary>
        /// <returns>机器唯一标识</returns>
        public static string Value()
        {
            if (string.IsNullOrEmpty(fingerPrint))
            {
                fingerPrint = GetHash(
                    "CPU >> " + CpuId() +
                    "\nBIOS >> " + BiosId() +
                    "\nBASE >> " + BaseId() +
                    "\nDISK >> " + DiskId() +
                    "\nVIDEO >> " + VideoId() +
                    "\nMAC >> " + MacId());
            }
            return "rexel:" + fingerPrint;
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        private static string GetHash(string str)
        {
            MD5 sec = new MD5CryptoServiceProvider();
            ASCIIEncoding enc = new ();
            byte[] bt = enc.GetBytes(str);
            return GetHexString(sec.ComputeHash(bt));
        }

        /// <summary>
        /// 转换16进制
        /// </summary>
        /// <param name="bt">字节数组</param>
        /// <returns>字符串</returns>
        private static string GetHexString(byte[] bt)
        {
            string s = string.Empty;
            for (int i = 0; i < bt.Length; i++)
            {
                byte b = bt[i];
                int n, n1, n2;
                n = b;
                n1 = n & 15;
                n2 = (n >> 4) & 15;
                if (n2 > 9)
                {
                    s += ((char)(n2 - 10 + 'A')).ToString();
                }
                else
                {
                    s += n2.ToString();
                }
                if (n1 > 9)
                {
                    s += ((char)(n1 - 10 + 'A')).ToString();
                }
                else
                {
                    s += n1.ToString();
                }
                if ((i + 1) != bt.Length && (i + 1) % 2 == 0)
                {
                    s += "-";
                }
            }
            return s;
        }

        /// <summary>
        /// Return a hardware identifier
        /// </summary>
        /// <param name="wmiClass">wmiClass</param>
        /// <param name="wmiProperty">wmiProperty</param>
        /// <param name="wmiMustBeTrue">wmiMustBeTrue</param>
        /// <returns>string</returns>
        private static string Identifier(string wmiClass, string wmiProperty, string wmiMustBeTrue)
        {
            string result = "";
            System.Management.ManagementClass mc = new(wmiClass);
            System.Management.ManagementObjectCollection moc = mc.GetInstances();
            foreach (System.Management.ManagementObject mo in moc)
            {
                if (mo[wmiMustBeTrue].ToString() == "True")
                {
                    try
                    {
                        result = mo[wmiProperty].ToString();
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Return a hardware identifier
        /// </summary>
        /// <param name="wmiClass">wmiClass</param>
        /// <param name="wmiProperty">wmiProperty</param>
        /// <returns>string</returns>
        private static string Identifier(string wmiClass, string wmiProperty)
        {
            string result = "";
            System.Management.ManagementClass mc = new(wmiClass);
            System.Management.ManagementObjectCollection moc = mc.GetInstances();
            foreach (System.Management.ManagementObject mo in moc)
            {
                try
                {
                    if (mo[wmiProperty] != null)
                    {
                        result = mo[wmiProperty].ToString();
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            return result;
        }

        /// <summary>
        /// CPU相关参数
        /// </summary>
        /// <returns>string</returns>
        private static string CpuId()
        {
            string retVal = Identifier("Win32_Processor", "UniqueId");
            if (retVal != "")
            {
                return retVal;
            }

            retVal = Identifier("Win32_Processor", "ProcessorId");
            if (retVal != "")
            {
                return retVal;
            }

            retVal = Identifier("Win32_Processor", "Name");
            if (retVal == "")
            {
                return Identifier("Win32_Processor", "Manufacturer");
            }
            else
            {
                return Identifier("Win32_Processor", "MaxClockSpeed");
            }
        }

        /// <summary>
        /// BIOS相关参数
        /// </summary>
        /// <returns>string</returns>
        private static string BiosId()
        {
            return Identifier("Win32_BIOS", "Manufacturer")
            + Identifier("Win32_BIOS", "SMBIOSBIOSVersion")
            + Identifier("Win32_BIOS", "IdentificationCode")
            + Identifier("Win32_BIOS", "SerialNumber")
            + Identifier("Win32_BIOS", "ReleaseDate")
            + Identifier("Win32_BIOS", "Version");
        }

        /// <summary>
        /// 磁盘相关参数
        /// </summary>
        /// <returns>结果</returns>
        private static string DiskId()
        {
            return Identifier("Win32_DiskDrive", "Model")
            + Identifier("Win32_DiskDrive", "Manufacturer")
            + Identifier("Win32_DiskDrive", "Signature")
            + Identifier("Win32_DiskDrive", "TotalHeads");
        }

        /// <summary>
        /// 主板相关参数
        /// </summary>
        /// <returns>结果</returns>
        private static string BaseId()
        {
            return Identifier("Win32_BaseBoard", "Model")
            + Identifier("Win32_BaseBoard", "Manufacturer")
            + Identifier("Win32_BaseBoard", "Name")
            + Identifier("Win32_BaseBoard", "SerialNumber");
        }

        /// <summary>
        /// 视频驱动相关参数
        /// </summary>
        /// <returns>结果</returns>
        private static string VideoId()
        {
            return Identifier("Win32_VideoController", "DriverVersion")
            + Identifier("Win32_VideoController", "Name");
        }

        /// <summary>
        /// 网络适配器相关参数
        /// </summary>
        /// <returns>结果</returns>
        private static string MacId()
        {
            return Identifier("Win32_NetworkAdapterConfiguration", "MACAddress", "IPEnabled");
        }
    }
}
