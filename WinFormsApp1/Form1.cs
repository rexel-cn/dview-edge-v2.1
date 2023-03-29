using FMDMOLib;
using System.Text.Json;
using System;
using System.Collections.Concurrent;
using System.Text;
using System.Windows.Forms;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private bool MqttIsAvailable { get; set; }
        private string FormArgs_selectTag { get; }
        private string FormArgs_username { get; }
        private string FormArgs_password { get; }
        private string FormArgs_clientId { get; set; }
        private string FormArgs_timer_t { get; }
        private string FormArgs_ectime_t { get; }
        private string FormArgs_broker { get; }
        private string FormArgs_port { get; }
        private string FormArgs_machine_code { get; }
        private string AutoStartCheck { get; set; }
        private bool UseGB { get; set; }
        private bool UseMB { get; set; }
        private bool LoadOnce { get; set; }
        private string BasePath { get; }
        private string DownNotice { get; }
        private string DownControl { get; }
        private string DownRestart { get; }
        private string UpData { get; }
        private string UpMeta { get; }
        private string ConfigFilePath { get; }
        private static Int64 netWorkBytes;
        private static readonly byte qos0 = 0x00;

        const int GB = 1024 * 1024 * 1024;
        const int MB = 1024 * 1024;
        const int KB = 1024;

        public Form1()
        {
            InitializeComponent();
            this.TopMost = true;
            CheckForIllegalCrossThreadCalls = false;
            BasePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            ConfigFilePath = BasePath + "conf.ini";

            IniFile MyIni = new IniFile(ConfigFilePath);
            FormArgs_selectTag = MyIni.Read("selectTag", "config");
            FormArgs_username = MyIni.Read("username", "config");
            FormArgs_password = MyIni.Read("password", "config");
            FormArgs_timer_t = MyIni.Read("timer", "config");
            FormArgs_ectime_t = MyIni.Read("ectime", "config");
            FormArgs_broker = MyIni.Read("broker", "config");
            FormArgs_port = MyIni.Read("port", "config");
            FormArgs_clientId = MyIni.Read("clientId", "config");
            AutoStartCheck = MyIni.Read("autostart", "config");

            this.user_client_id.Text = FormArgs_clientId;
            this.machine_code.Text = FingerPrint.Value();
            if (!string.IsNullOrEmpty(FormArgs_clientId))
            {
                FormArgs_machine_code = this.user_client_id.Text;
            }
            else
            {
                FormArgs_machine_code = this.machine_code.Text;
            }

            DownNotice = string.Format("/rexel/d500/{0}/down/notice", FormArgs_machine_code);
            DownControl = string.Format("/rexel/d500/{0}/down/control", FormArgs_machine_code);
            DownRestart = string.Format("/rexel/d500/{0}/down/restart", FormArgs_machine_code);
            UpData = string.Format("/rexel/d500/{0}/up/data", FormArgs_machine_code);
            UpMeta = string.Format("/rexel/d500/{0}/up/meta", FormArgs_machine_code);

            LogView.AddLogBox(this.richTextBox1);

            LoadOnce = false;

            user_t.Text = FormArgs_username;
            pass_t.Text = FormArgs_password;
            pinlv.Text = FormArgs_timer_t;

            selectTag_t.Text = FormArgs_selectTag;
            ectime.Text = FormArgs_ectime_t;
            broker_t.Text = FormArgs_broker;
            port_t.Text = FormArgs_port;
            if (AutoStartCheck == "Checked")
            {
                checkBox1.Checked = true;
            }
            else
            {
                checkBox1.Checked = false;
            }

            MqttIsAvailable = false;
            netWorkBytes = 0;

            Task connectMonitor = new(() => { ConnectMonitor(); });
            connectMonitor.Start();

            Task topicSubscribe = new(() => { TopicSubscribe(); });
            topicSubscribe.Start();

            Task publishData = new(() => { PublishData(selectTag_t.Text.Split(",")); });
            publishData.Start();
        }

        public void AddLogToFrom(string logs)
        {
            Action<string> action = (logs) =>
            {
                network_bytes.Text = ByteConversionGBMBKB(netWorkBytes);
                Log.Pop(logs, 0);
            };
            Invoke(action, logs);
        }

        public void AddPackageOnece(long size)
        {
            Action<long> action = (size) =>
            {
                // 计算速率
                package_once.Text = ByteConversionGBMBKB(size);
                bool b1 = float.TryParse(sim_car_network_package.Text, out float SimCarNetworkPackageData);

                float packages = 0;
                long useTIme = 0;
                if (b1)
                {
                    useTIme =
                    ((DateTime.Now.AddDays(1 - DateTime.Now.Day).Date.AddMonths(1).AddSeconds(-1).ToUniversalTime().Ticks - 621355968000000000) / 10000000) -
                    ((DateTime.Now.AddDays(1 - DateTime.Now.Day).Date.ToUniversalTime().Ticks - 621355968000000000) / 10000000);
                    if (UseGB)
                    {
                        packages = SimCarNetworkPackageData * 1000 * 1000 * 1000;
                    }

                    if (UseMB)
                    {
                        packages = SimCarNetworkPackageData * 1000 * 1000;
                    }

                    var prespeed = useTIme / (packages / size);
                    // 在原有基础上加上30%
                    prespeed = (float)(prespeed + (prespeed * 0.3));
                    predict_speed.Text = "推荐：" + prespeed + "秒";
                }
            };
            Invoke(action, size);
        }

        public void TopicSubscribe()
        {
            MqttClient mqttClient = GLData.Instance.getMqtt();
            mqttClient.Connect(FormArgs_machine_code, FormArgs_username, FormArgs_password);
            CreateSubscribe(mqttClient);
        }

        private void CreateSubscribe(MqttClient mqttClient)
        {
            if (mqttClient.IsConnected)
            {
                string[] topics = new string[3] { DownNotice, DownControl, DownRestart };
                byte[] qosLevels = new byte[3] { qos0, qos0, qos0 };
                mqttClient.MqttMsgPublishReceived -= MqttMsgPublishReceived;
                mqttClient.MqttMsgPublishReceived += MqttMsgPublishReceived;
                mqttClient.Subscribe(topics, qosLevels);
                AddLogToFrom("创建订阅关系。");
            }
        }

        public void MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string topic = e.Topic;
            if (topic == DownControl)
            {
                AddLogToFrom("收到下发命令。");
                DoModifyPoint(e);
            }
            if (topic == DownNotice)
            {
                AddLogToFrom("收到平台通知。");
                LoadOnce = false;
            }
        }

        private void DoModifyPoint(MqttMsgPublishEventArgs e)
        {
            string originalRtf = System.Text.Encoding.UTF8.GetString(e.Message);
            var deviceModifyVal = JsonSerializer.Deserialize<DeviceModifyVal>(originalRtf);
            String point = deviceModifyVal.pointType + "." + deviceModifyVal.pointId;

            try
            {
                Rundb runbdb = GLData.Instance.getModifyDb();
                object n = runbdb.Open();
                if (Convert.ToInt16(n) == 1)
                {
                    runbdb.SetVarValueEx(point, deviceModifyVal.beforeValue);
                    AddLogToFrom("修改测点：" + point + "=" + deviceModifyVal.beforeValue);
                }

                if (deviceModifyVal.duration > 0)
                {
                    Thread.Sleep(deviceModifyVal.duration * 1000);
                    runbdb.SetVarValueEx(point, deviceModifyVal.afterValue);
                    AddLogToFrom("脉冲更新：" + point + "=" + deviceModifyVal.afterValue);
                }
                runbdb.Close();
            }
            catch (Exception ex)
            {
                AddLogToFrom("fmCDOM异常。" + ex.Message);
            }
        }

        public void ConnectMonitor()
        {
            while (true)
            {
                if (string.IsNullOrEmpty(FormArgs_timer_t))
                {
                    continue;
                }

                int time = (int)Math.Floor(Decimal.Parse(FormArgs_timer_t) / 2);
                if (time < 5)
                {
                    Thread.Sleep(1000 * 5);
                }
                else if (time > 60)
                {
                    Thread.Sleep(1000 * 60);
                }
                else
                {
                    Thread.Sleep(1000 * time);
                }

                if (MqttIsAvailable)
                {
                    continue;
                }

                MqttClient mqttClient = GLData.Instance.getMqtt();
                if (mqttClient.IsConnected)
                {
                    continue;
                }

                try
                {
                    var result = mqttClient.Connect(FormArgs_machine_code, FormArgs_username, FormArgs_password);
                    CreateSubscribe(mqttClient);
                    LoadOnce = false;
                    AddLogToFrom("MQTT重连成功。");
                    MqttIsAvailable = true;
                }
                catch (Exception ex)
                {
                    AddLogToFrom("MQTT重连异常。" + ex.Message);
                    MqttIsAvailable = false;
                }
            }
        }

        public class ReportData
        {
            public string time { get; set; }
            public string dataType { get; set; }
            public string pointType { get; set; }
            public List<PointData> data { get; set; }
        }

        public class RecortMeta
        {
            public string time { get; set; }
            public string dataType { get; set; }
            public string pointType { get; set; }
            public List<PointData> data { get; set; }
        }

        public class DeviceModifyVal
        {
            public string deviceId { get; set; }
            public string pointId { get; set; }
            public string pointType { get; set; }
            public double beforeValue { get; set; }
            public double afterValue { get; set; }
            public int duration { get; set; }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            IniFile MyIni = new(ConfigFilePath);
            MyIni.Write("timer", pinlv.Text, "config");
            MyIni.Write("username", user_t.Text, "config");
            MyIni.Write("password", pass_t.Text, "config");
            MyIni.Write("ectime", ectime.Text, "config");
            MyIni.Write("broker", broker_t.Text, "config");
            MyIni.Write("port", port_t.Text, "config");
            MyIni.Write("selectTag", selectTag_t.Text, "config");
            MyIni.Write("clientId", user_client_id.Text, "config");
        }

        public static string ByteConversionGBMBKB(Int64 KSize)
        {
            if (KSize / GB >= 1)
            {
                // 如果当前Byte的值大于等于1GB
                // 将其转换成GB
                return (Math.Round(KSize / (float)GB, 2)).ToString() + "GB";
            }
            else if (KSize / MB >= 1)
            {
                // 如果当前Byte的值大于等于1MB
                // 将其转换成MB
                return (Math.Round(KSize / (float)MB, 2)).ToString() + "MB";
            }
            else if (KSize / KB >= 1)
            {
                // 如果当前Byte的值大于等于1KB
                // 将其转换成KB
                return (Math.Round(KSize / (float)KB, 2)).ToString() + "KB";
            }
            else
            {
                // 显示Byte值
                return KSize.ToString() + "Byte";
            }
        }

        public void PublishData(String[] selectTagList)
        {
            while (true)
            {
                var longOnecePackageLen = 0;
                var pointCount = 0;
                foreach (string selectTag in selectTagList)
                {
                    Rundb runbdb = GLData.Instance.getRunDb();
                    List<PointData> result = null;
                    try
                    {
                        object n = runbdb.Open();
                        if (Convert.ToInt16(n) == 1)
                        {
                            var data = "";
                            if (this.point_filter.Text == "ALL")
                            {
                                data = runbdb.ReadFilterVarValues(selectTag, "*");
                            }
                            else
                            {
                                data = runbdb.ReadFilterVarValues(selectTag, selectTag + this.point_filter.Text);
                            }

                            result = Tools.GetPointDataList(data, selectTag);
                            runbdb.Close();
                        }
                        else
                        {
                            AddLogToFrom("fmDOM无法打开。");
                            continue;
                        }
                    }
                    catch (Exception ex)
                    {
                        AddLogToFrom("RtData fmCDOM异常。" + ex.Message);
                        continue;
                    }

                    if (result.Count > 0)
                    {
                        pointCount += result.Count;

                        double ectTime = 0.0;
                        if (FormArgs_ectime_t.Contains("-"))
                        {
                            ectTime = ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000) -
                                (double.Parse(ectime.Text.Replace("-", "")) * 1000);
                        }
                        if (FormArgs_ectime_t.Contains("+"))
                        {
                            ectTime = ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000) +
                                (double.Parse(ectime.Text.Replace("+", "")) * 1000);
                        }

                        var reportData = new ReportData
                        {
                            time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            dataType = GetTypeForInstance(selectTag),
                            pointType = selectTag,
                            data = result,
                        };

                        MqttClient mqttClient = GLData.Instance.getMqtt();
                        if (!mqttClient.IsConnected)
                        {
                            AddLogToFrom("No Connected wait 1s.");
                            MqttIsAvailable = false;
                            Thread.Sleep(1000);
                            continue;
                        }

                        try
                        {
                            ushort msgId = 0;
                            if (!LoadOnce)
                            {
                                var reportMeta = GetALLPointMetaInfo();
                                string metaJson = JsonSerializer.Serialize(reportMeta);
                                byte[] pointsMeta = Encoding.UTF8.GetBytes(metaJson);
                                msgId = mqttClient.Publish(UpMeta, pointsMeta, MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, true);
                                Thread.Sleep(3000);
                                LoadOnce = true;
                                AddLogToFrom("上报数据测点。count=" + result.Count);
                            }

                            string jsonString = JsonSerializer.Serialize(reportData);
                            byte[] netWorkBytesSender = Encoding.UTF8.GetBytes(jsonString);
                            msgId = mqttClient.Publish(UpData, netWorkBytesSender, MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, true);
                            Interlocked.Add(ref netWorkBytes, netWorkBytesSender.Length);
                            Interlocked.Add(ref longOnecePackageLen, netWorkBytesSender.Length);
                            AddLogToFrom("上报运行数据。" + selectTag + ", count = " + result.Count);
                        }
                        catch (Exception ex)
                        {
                            AddLogToFrom("mqtt Publish exception." + ex.Message);
                        }
                    }
                }

                this.point_count.Text = String.Format("{0}", pointCount);
                AddPackageOnece(longOnecePackageLen);
                Thread.Sleep(Int32.Parse(FormArgs_timer_t) * 1000);
            }
        }

        private List<RecortMeta> GetALLPointMetaInfo()
        {
            List<RecortMeta> result = new();
            foreach (string selectTag in FormArgs_selectTag.Split(","))
            {
                Rundb runbdb = GLData.Instance.getRunDb();
                string CODE = selectTag;

                try
                {
                    object n = runbdb.Open();
                    if (Convert.ToInt16(n) == 1)
                    {
                        var data = "";
                        if (this.point_filter.Text == "ALL")
                        {
                            data = runbdb.ReadFilterVarValues(CODE, "*");
                        }
                        else
                        {
                            data = runbdb.ReadFilterVarValues(CODE, CODE + this.point_filter.Text);
                        }

                        List<PointData> dataList = Tools.GetPointDataList(data, CODE);
                        var reportMeta = new RecortMeta
                        {
                            time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            dataType = GetTypeForInstance(selectTag),
                            pointType = selectTag,
                            data = dataList
                        };
                        result.Add(reportMeta);
                        runbdb.Close();
                    }
                }
                catch (Exception ex)
                {
                    AddLogToFrom("Metadata fmCDOM异常。" + ex.Message);
                }
            }
            return result;
        }

        public static string GetTypeForInstance(string codeType)
        {
            string result = codeType switch
            {
                "AR" or "AO" or "AI" or "VA" => "double",
                "DI" or "DO" or "DR" or "VD" => "int",
                _ => "string",
            };
            return result;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("确定要退出吗?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                IniFile MyIni = new(ConfigFilePath);
                MyIni.Write("timer", pinlv.Text, "config");
                MyIni.Write("username", user_t.Text, "config");
                MyIni.Write("password", pass_t.Text, "config");
                MyIni.Write("ectime", ectime.Text, "config");
                MyIni.Write("broker", broker_t.Text, "config");
                MyIni.Write("port", port_t.Text, "config");
                MyIni.Write("selectTag", selectTag_t.Text, "config");
                MyIni.Write("clientId", user_client_id.Text, "config");
                MyIni.Write("autostart", checkBox1.CheckState.ToString(), "config");
                System.Environment.Exit(0);
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).CheckState.ToString() == "Checked")
            {
                Utils.SetMeStart(true);
            }
            else
            {
                Utils.SetMeStart(false);
            }
        }

        private void C_gb_CheckedChanged(object sender, EventArgs e)
        {
            UseGB = true;
            UseMB = false;
        }

        private void C_mb_CheckedChanged(object sender, EventArgs e)
        {
            UseMB = true;
            UseGB = false;
        }

        private void Sim_car_network_package_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 判断按键是不是要输入的类型。
            if (((int)e.KeyChar < 48 || (int)e.KeyChar > 57) && (int)e.KeyChar != 8 && (int)e.KeyChar != 46)
            {
                e.Handled = true;
            }

            // 小数点的处理。
            if ((int)e.KeyChar == 46)
            {
                if (sim_car_network_package.Text.Length <= 0)
                {
                    // 小数点不能在第一位
                    e.Handled = true;
                }
                else
                {
                    bool b1 = float.TryParse(sim_car_network_package.Text, out _);
                    bool b2 = float.TryParse(sim_car_network_package.Text + e.KeyChar.ToString(), out _);
                    if (b2 == false)
                    {
                        if (b1 == true)
                        {
                            e.Handled = true;
                        }
                        else
                        {
                            e.Handled = false;
                        }
                    }
                }
            }
        }

        private void Button4_Click_1(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("确定要重启吗?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                IniFile MyIni = new(ConfigFilePath);
                MyIni.Write("timer", pinlv.Text, "config");
                MyIni.Write("username", user_t.Text, "config");
                MyIni.Write("password", pass_t.Text, "config");
                MyIni.Write("ectime", ectime.Text, "config");
                MyIni.Write("broker", broker_t.Text, "config");
                MyIni.Write("port", port_t.Text, "config");
                MyIni.Write("selectTag", selectTag_t.Text, "config");
                MyIni.Write("clientId", user_client_id.Text, "config");

                ProcessStartInfo processStartInfo = new ProcessStartInfo();
                processStartInfo.WorkingDirectory = BasePath;
                processStartInfo.FileName = @"WinFormsApp1.exe";
                processStartInfo.CreateNoWindow = true;
                _ = Process.Start(processStartInfo);
                Process.GetCurrentProcess()?.Kill();
            }
        }
    }
}
