using System;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using static uPLibrary.Networking.M2Mqtt.MqttClient;

namespace DViewEdge
{
    public class MqttUtils
    {
        private static MqttUtils Instance = null;
        private MqttClient MqttClient = null;

        private MqttUtils(){}

        public static MqttUtils GetInastance(string address, string port)
        {
            if (Instance != null)
            {
                return Instance;
            }

            Int32 intPort = Int32.Parse(port);
            MqttUtils _instance = new()
            {
                MqttClient = new MqttClient(address, intPort, false, MqttSslProtocols.None, null, null)
            };
            Instance = _instance;
            return Instance;
        }

        public void AddPublishedHandler(MqttMsgPublishedEventHandler handler)
        {
            this.MqttClient.MqttMsgPublished -= handler;
            this.MqttClient.MqttMsgPublished += handler;
        }

        public void Connect(string clientId, string username, string password)
        {
            this.MqttClient.Connect(clientId, username, password);
        }

        public Boolean IsConnected()
        {
            return this.MqttClient.IsConnected;
        }

        public void Subscribe(string[] topics, byte[] qosLevels, MqttClient.MqttMsgPublishEventHandler handler)
        {
            this.MqttClient.MqttMsgPublishReceived -= handler;
            this.MqttClient.MqttMsgPublishReceived += handler;
            this.MqttClient.Subscribe(topics, qosLevels);
        }

        public void Public(string topic, byte[] message)
        {
            this.MqttClient.Publish(topic, message, MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, true);
        }

    }
}
