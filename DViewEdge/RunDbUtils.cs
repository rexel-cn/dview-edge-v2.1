using FMDMOLib;
using System.Collections.Generic;

namespace DViewEdge
{
    public class RunDbUtils
    {
        /// <summary>
        /// 运行数据
        /// </summary>
        public class ReportData
        {
            public string Time { get; set; }
            public string DataType { get; set; }
            public string PointType { get; set; }
            public List<PointData> Data { get; set; }
        }

        /// <summary>
        /// 测点信息
        /// </summary>
        public class PointData
        {
            public string PointId { get; set; }
            public object PointValue { get; set; }
            public string Qty { get; set; }
        }

        private static RunDbUtils Instance = null;
        private Rundb Read { get; set; }
        private Rundb Write { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        private RunDbUtils() { }

        /// <summary>
        /// 创建实例
        /// </summary>
        /// <returns>实例</returns>
        public static RunDbUtils GetInstance()
        {
            if (Instance != null)
            {
                return Instance;
            }

            RunDbUtils _instance = new()
            {
                Read = new Rundb(),
                Write = new Rundb(),
            };
            Instance = _instance;
            return _instance;
        }

        /// <summary>
        /// 获取读数据实例
        /// </summary>
        /// <returns>Rundb</returns>
        public Rundb GetRead()
        {
            return Read;
        }

        /// <summary>
        /// 获取写数据实例
        /// </summary>
        /// <returns>Rundb</returns>
        public Rundb GetWrite()
        {
            return Write;
        }
    }
}
