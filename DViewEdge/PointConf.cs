using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace DViewEdge
{
    public partial class PointConf
    {
        public class PointJson
        {
            public string PointType { get; set; }
            public List<PointRepeate> PointList { get; set; }
        }

        public class PointRepeate
        {
            public string PointId { get; set; }
            public int Repeate { get; set; }
        }

        /// <summary>
        /// 私有属性
        /// </summary>
        private string Path;

        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <param name="path">配置文件路径</param>
        public PointConf()
        {
            string BasePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string path = BasePath + Constants.PointFile;
            Path = path;
        }

        public List<PointJson> Load()
        {
            string json = File.ReadAllText(Path);
            return JsonConvert.DeserializeObject<List<PointJson>>(json);
        }

        /// <summary>
        /// 保存配置文件
        /// </summary>
        public void Save(List<PointJson> pointJson)
        {
            string content = JsonConvert.SerializeObject(pointJson);
            File.WriteAllText(Path, content);
        }
    }
}
