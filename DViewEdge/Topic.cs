using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// <param name="clientId">机器码</param>
        public Topic(string clientId)
        {
            this.DownNotice = string.Format(Constants.TopicDownNotice, clientId);
            this.DownControl = string.Format(Constants.TopicDownControl, clientId);
            this.DownRestart = string.Format(Constants.TopicDownRestart, clientId);
            this.UpData = string.Format(Constants.TopicUpData, clientId);
            this.UpMeta = string.Format(Constants.TopicUpMeta, clientId);
        }
    }
}
