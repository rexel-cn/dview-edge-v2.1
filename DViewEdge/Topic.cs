namespace DViewEdge
{
    public class Topic
    {
        public string DownNotice { get; }
        public string DownControl { get; }
        public string DownRestart { get; }
        public string UpData { get; }
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
