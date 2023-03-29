using FMDMOLib;
using System;
using System.Collections.Concurrent;
using uPLibrary.Networking.M2Mqtt;

namespace WinFormsApp1
{
    public class GLData
    {
        private static Lazy<GLData> _Instance = new Lazy<GLData>(() => new GLData());

        private Rundb runbdb1 { get; }
        private Rundb runbdb2 { get; }

        private MqttClient getMQTT { get; }

        private GLData()
        {
            FMDMOLib.Rundb readData = new Rundb();
            FMDMOLib.Rundb modify = new Rundb();

            string BasePath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string ConfigFilePath = BasePath + "conf.ini";

            IniFile MyIni = new IniFile(ConfigFilePath);
            string broker = MyIni.Read("broker", "config");
            int port = Int32.Parse(MyIni.Read("port", "config"));

            try
            {
                getMQTT = new MqttClient(broker, port, false, MqttSslProtocols.None, null, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            runbdb1 = readData;
            runbdb2 = modify;
        }

        public static GLData Instance
        {
            get
            {
                return _Instance.Value;
            }
        }

        public Rundb getRunDb()
        {
            return runbdb1;
        }

        public Rundb getModifyDb()
        {
            return runbdb2;
        }
        public MqttClient getMqtt()
        {
            return getMQTT;
        }
    }
}
