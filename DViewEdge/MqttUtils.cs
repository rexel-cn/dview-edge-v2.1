using System;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using static uPLibrary.Networking.M2Mqtt.MqttClient;

namespace DViewEdge
{
    public class MqttUtils
    {
        private static MqttUtils Instance = null;
        private MqttClient Client = null;
        private string Address = null;
        private string Port = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        private MqttUtils(){}

        /// <summary>
        /// 创建实例
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="port">端口</param>
        /// <returns>实例</returns>
        public static MqttUtils GetInastance(string _address, string _port)
        {
            if (string.IsNullOrEmpty(_address) || string.IsNullOrEmpty(_port))
            {
                return null;
            }
            if (Instance != null)
            {
                return Instance;
            }

            MqttUtils _instance = new MqttUtils()
            {
                Client = null,
                Address = _address,
                Port = _port
            };
            Instance = _instance;
            return Instance;
        }

        private MqttClient GetClient()
        {
            if (Client != null)
            {
                return Client;
            }
            int intPort = int.Parse(Port);

            MqttClient c = null;
            try
            {
                c = new MqttClient(Address, intPort, false, MqttSslProtocols.None, null, null);
                Client = c;
                return c;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        /// <summary>
        /// 创建发送结果监听事件
        /// </summary>
        /// <param name="handler">handler</param>
        public void AddPublishedHandler(MqttMsgPublishedEventHandler handler)
        {
            MqttClient c = GetClient();
            if (c == null)
            {
                return;
            }
            c.MqttMsgPublished -= handler;
            c.MqttMsgPublished += handler;
        }

        /// <summary>
        /// 连接Mqtt Broker
        /// </summary>
        /// <param name="clientId">客户端ID</param>
        /// <param name="username">账号</param>
        /// <param name="password">密码</param>
        public void Connect(string clientId, string username, string password)
        {
            MqttClient c = GetClient();
            if (c == null)
            {
                return;
            }
            c.Connect(clientId, username, password);
        }

        /// <summary>
        /// 返回是否连接
        /// </summary>
        /// <returns>bool</returns>
        public bool IsConnected()
        {
            MqttClient c = GetClient();
            if (c == null)
            {
                return false;
            }
            return c.IsConnected;
        }

        /// <summary>
        /// 创建订阅对象
        /// </summary>
        /// <param name="topics">主题</param>
        /// <param name="qosLevels">数据质量</param>
        /// <param name="handler">监听句柄</param>
        public void Subscribe(string[] topics, byte[] qosLevels, MqttMsgPublishEventHandler handler)
        {
            MqttClient c = GetClient();
            if (c == null)
            {
                return;
            }
            if (!c.IsConnected)
            {
                return;
            }
            c.MqttMsgPublishReceived -= handler;
            c.MqttMsgPublishReceived += handler;
            c.Subscribe(topics, qosLevels);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="topic">主题</param>
        /// <param name="message">消息</param>
        public void Public(string topic, byte[] message)
        {
            MqttClient c = GetClient();
            if (c == null)
            {
                return;
            }
            if (!c.IsConnected)
            {
                return;
            }
            c.Publish(topic, message, MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, true);
        }
    }
}
