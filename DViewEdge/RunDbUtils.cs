using FMDMOLib;

namespace DViewEdge
{
    public class RunDbUtils
    {
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
