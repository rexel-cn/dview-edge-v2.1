using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DViewEdge
{
    static class Constants
    {
        // 配置文件名称
        public const string ConfFile = "conf.ini";

        // COM打开状态
        public const short OpenOk = 1;

        // Topic路径
        public const string TopicDownNotice = "/rexel/d500/{0}/down/notice";
        public const string TopicDownControl = "/rexel/d500/{0}/down/control";
        public const string TopicDownRestart = "/rexel/d500/{0}/down/restart";
        public const string TopicUpData = "/rexel/d500/{0}/up/data";
        public const string TopicUpMeta = "/rexel/d500/{0}/up/meta";

        // 测点类型
        public const string AR = "AR";
        public const string AI = "AI";
        public const string AO = "AO";
        public const string DR = "DR";
        public const string DI = "DI";
        public const string DO = "DO";
        public const string VA = "VA";
        public const string VD = "VD";
        public const string VT = "VT";

        // 数据质量
        public const byte Qos0 = 0x00;

        // 数据单位
        public const int KB = 1024;
        public const int MB = KB * 1024;
        public const int GB = MB * 1024;

        // 时间格式
        public const string FormatLong = "yyyy-MM-dd HH:mm:ss";
        public const string FormatLongMs = "yyyy-MM-dd HH:mm:ss.fff";

        // 支持采集的测点类型
        public static List<string> SupportType = new() { AR, AI, AO, DR, DI, DO };
    }
}
