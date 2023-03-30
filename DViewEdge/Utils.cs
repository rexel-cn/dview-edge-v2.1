using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static DViewEdge.EdgeForm;

namespace DViewEdge
{
    class Utils
    {
        /// <summary>
        /// 获取当前时间
        /// </summary>
        /// <returns>当前时间</returns>
        public static string TimeNow()
        {
            DateTime dt = DateTime.Now;
            return dt.ToString(Constants.FormatLong);
        }

        /// <summary>
        /// 是否为数字
        /// </summary>
        /// <param name="str">str</param>
        /// <returns>true:是、false:否</returns>
        public static bool IsNumber(string str)
        {
            Regex regex = new("^[0-9]*$");
            return regex.IsMatch(str);
        }

        /// <summary>
        /// 是否为整数
        /// </summary>
        /// <param name="str">str</param>
        /// <returns>true:是、false:否</returns>
        public static bool IsInteger(string str)
        {
            Regex regex = new("^-?\\d+$");
            return regex.IsMatch(str);
        }

        /// <summary>
        /// 是否为正整数
        /// </summary>
        /// <param name="str">str</param>
        /// <returns>true:是、false:否</returns>
        public static bool IsNaturalNumber(string str)
        {
            Regex regex = new("^\\+?[1-9][0-9]*$");
            return regex.IsMatch(str);
        }

        /// <summary>
        /// 是否为合法客户端ID
        /// 正确格式：以字母开头，长度在4~50之间，只能包含字符、数字和下划线。
        /// </summary>
        /// <param name="str">str</param>
        /// <returns></returns>
        public static bool IsClientId(string str)
        {
            Regex regex = new("^[a-zA-Z]\\w{3,49}$");
            return regex.IsMatch(str);
        }

        /// <summary>
        /// 是否为IP地址
        /// </summary>
        /// <param name="str"></param>
        /// <returns>true:是、false:否</returns>
        public static bool IsIpAddress(string str)
        {
            Regex regex = new("^[0-9]{1,3}.[0-9]{1,3}.[0-9]{1,3}.[0-9]{1,3}$");
            if (regex.IsMatch(str) != true)
            {
                return false;
            }

            string[] strTemp = str.Split(new char[] { '.' });
            int nDotCount = strTemp.Length - 1;
            if (3 != nDotCount)
            {
                return false;
            }

            for (int i = 0; i < strTemp.Length; i++)
            {
                if (Convert.ToInt32(strTemp[i]) > 255)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 对象转JSON（首字母小写）
        /// </summary>
        /// <param name="classes">classes</param>
        /// <returns></returns>
        public static string JsonToStr(object classes)
        {
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            return JsonConvert.SerializeObject(classes, Formatting.None, serializerSettings);
        }

        public static DeviceModifyVal StrToJson(string str)
        {
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            return JsonConvert.DeserializeObject<DeviceModifyVal>(str, serializerSettings);
        }

        /// <summary>
        /// 显示确认窗口
        /// </summary>
        /// <param name="str">str</param>
        /// <returns></returns>
        public static DialogResult ShowConfirm(string str)
        {
            return MessageBox.Show(str, "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        /// <summary>
        /// 显示提示窗口
        /// </summary>
        /// <param name="str">str</param>
        public static void ShowInfoBox(string str)
        {
            _ = MessageBox.Show(str, "提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }

        /// <summary>
        /// 转换数据格式
        /// </summary>
        /// <param name="size">size</param>
        /// <returns>结果</returns>
        public static string ConvertSize(Int64 size)
        {
            if (size / Constants.GB >= 1)
            {
                return (Math.Round(size / (float)Constants.GB, 2)).ToString() + " GB";
            }
            else if (size / Constants.MB >= 1)
            {
                return (Math.Round(size / (float)Constants.MB, 2)).ToString() + " MB";
            }
            else if (size / Constants.KB >= 1)
            {
                return (Math.Round(size / (float)Constants.KB, 2)).ToString() + " KB";
            }
            else
            {
                return size.ToString() + " 字节";
            }
        }
    }
}
