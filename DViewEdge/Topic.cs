namespace DViewEdge
{
    public class Topic
    {
        /// <summary>
        /// 下行通知Topic
        /// </summary>
        public string DownNotice { get; }
        /// <summary>
        /// 命令下发Topic
        /// </summary>
        public string DownControl { get; }
        /// <summary>
        /// 重启通知Topic
        /// </summary>
        public string DownRestart { get; }
        /// <summary>
        /// 上报数据Topic
        /// </summary>
        public string UpData { get; }
        /// <summary>
        /// 设备元数据Topic
        /// </summary>
        public string UpMeta { get; }

        /// <summary>
        /// 获取Topic信息
        /// </summary>
        /// <param name="clientId">客户端ID</param>
        public Topic(string clientId)
        {
            DownNotice = string.Format(Constants.TopicDownNotice, clientId);
            DownControl = string.Format(Constants.TopicDownControl, clientId);
            DownRestart = string.Format(Constants.TopicDownRestart, clientId);
            UpData = string.Format(Constants.TopicUpData, clientId);
            UpMeta = string.Format(Constants.TopicUpMeta, clientId);
        }
    }
}
