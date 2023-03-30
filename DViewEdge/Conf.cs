using System;

namespace DViewEdge
{
    public partial class Conf
    {
        /// <summary>
        /// 私有属性
        /// </summary>
        private string Path;

        /// <summary>
        /// 公开属性
        /// </summary>
        public string SelectTag { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Repeate { get; set; }
        public string Offset { get; set; }
        public string Address { get; set; }
        public string Port { get; set; }
        public string UserClientId { get; set; }
        public string DeviceDescribe { get; set; }

        /// <summary>
        /// 内部常量
        /// </summary>
        // 段落名
        private const string SectionConfig = "config";
        // 配置项：变量类型
        private const string KeySelectTag = "selectTag";
        // 配置项：账号
        private const string KeyUsername = "username";
        // 配置项：密码
        private const string KeyPassword = "password";
        // 配置项：采集频率
        private const string KeyRepeate = "repeate";
        // 配置项：时间偏移
        private const string KeyOffset = "offset";
        // 配置项：地址
        private const string KeyAddress = "address";
        // 配置项：端口
        private const string KeyPort = "port";
        // 配置项：自定义客户端
        private const string KeyUserClientId = "userClientId";
        // 配置项：设备名称描述
        private const string KeyDeviceDescribe = "deviceDescribe";

        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <param name="path">配置文件路径</param>
        public Conf()
        {
            string BasePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string path = BasePath + Constants.ConfFile;

            IniFile ini = new(path);
            Path = path;
            SelectTag = ini.Read(KeySelectTag, SectionConfig);
            Username = ini.Read(KeyUsername, SectionConfig);
            Password = ini.Read(KeyPassword, SectionConfig);
            Repeate = ini.Read(KeyRepeate, SectionConfig);
            Offset = ini.Read(KeyOffset, SectionConfig);
            Address = ini.Read(KeyAddress, SectionConfig);
            Port = ini.Read(KeyPort, SectionConfig);
            UserClientId = ini.Read(KeyUserClientId, SectionConfig);
            DeviceDescribe = ini.Read(KeyDeviceDescribe, SectionConfig);
        }

        /// <summary>
        /// 保存配置文件
        /// </summary>
        public void SaveConf()
        {
            IniFile ini = new(Path);
            ini.Write(KeySelectTag, SelectTag, SectionConfig);
            ini.Write(KeyUsername, Username, SectionConfig);
            ini.Write(KeyPassword, Password, SectionConfig);
            ini.Write(KeyRepeate, Repeate, SectionConfig);
            ini.Write(KeyOffset, Offset, SectionConfig);
            ini.Write(KeyAddress, Address, SectionConfig);
            ini.Write(KeyPort, Port, SectionConfig);
            ini.Write(KeyUserClientId, UserClientId, SectionConfig);
            ini.Write(KeyDeviceDescribe, DeviceDescribe, SectionConfig);
        }
    }
}
