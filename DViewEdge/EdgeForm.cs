using FMDMOLib;
using System;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace DViewEdge
{
    public partial class EdgeForm : Form
    {
        private Conf EdgeConf { get; }
        private Topic MqttTopic { get; }
        private MqttUtils MqttUtils { get; }
        private RunDbUtils RunDbUtils { get; }
        private bool LoadOnce { get; set; }
        private static Int64 netWorkBytes;

        public EdgeForm()
        {
            // 初始化窗体组件
            InitializeComponent();

            // 窗体始终位于屏幕最前面
            this.TopMost = true;

            // 设置为false能安全的访问窗体控件
            CheckForIllegalCrossThreadCalls = false;

            // 加载配置文件
            this.EdgeConf = new Conf();

            // 初始化显示内容
            InitFormContentByConf(this.EdgeConf);

            // 获取客户端ID
            string clientId = GetClientIdByConf(this.EdgeConf);

            // 加载Topic信息
            this.MqttTopic = new Topic(clientId);

            // 初始化MQTT客户端
            this.MqttUtils = MqttUtils.GetInastance(this.EdgeConf.Address, this.EdgeConf.Port);

            // 初始化COM客户端
            this.RunDbUtils = RunDbUtils.GetInstance();

//            LogView.AddLogBox(this.richTextBox1);

            LoadOnce = false;
            netWorkBytes = 0;

            // 启动后台任务
            StartTask();
        }

        /// <summary>
        /// 启动后台任务
        /// </summary>
        private void StartTask()
        {
            // 监控任务
            Task connectMonitor = new(() => { ConnectMonitor(); });
            connectMonitor.Start();

            // 订阅任务
            Task topicSubscribe = new(() => { CreateSubscribe(); });
            topicSubscribe.Start();

            // 发送任务
            Task publishData = new(() => { PublishData(); });
            publishData.Start();
        }

        /// <summary>
        /// 根据配置文件获取客户端Id
        /// </summary>
        /// <param name="conf">配置文件</param>
        /// <returns>客户端ID</returns>
        private string GetClientIdByConf(Conf conf)
        {
            if (!string.IsNullOrEmpty(conf.UserClientId))
            {
                return conf.UserClientId;
            }
            else
            {
                return this.lblMachineCode.Text;
            }
        }

        /// <summary>
        /// 根据配置文件初始化显示内容
        /// </summary>
        /// <param name="conf">配置文件</param>
        private void InitFormContentByConf(Conf conf)
        {
            this.lblCount.Text = "0";
            this.lblSize.Text = "0";
            this.lblStartTime.Text = Utils.TimeNow();
            this.lblMachineCode.Text = FingerPrint.Value();

            this.txtUsername.Text = conf.Username;
            this.txtPassword.Text = conf.Password;
            this.txtAddress.Text = conf.Address;
            this.txtPort.Text = conf.Port;
            this.txtSelectTag.Text = conf.SelectTag;
            this.txtRepeate.Text = conf.Repeate;
            this.txtOffset.Text = conf.Offset;
            this.txtUserClientId.Text = conf.UserClientId;
            this.txtDeviceDescribe.Text = conf.DeviceDescribe;
        }

        //public void AddLogToFrom(string logs)
        //{
        //    Action<string> action = (logs) =>
        //    {
        //        network_bytes.Text = ByteConversionGBMBKB(netWorkBytes);
        //        Log.Pop(logs, 0);
        //    };
        //    Invoke(action, logs);
        //}

        //public void AddPackageOnece(long size)
        //{
        //    Action<long> action = (size) =>
        //    {
        //        // 计算速率
        //        package_once.Text = ByteConversionGBMBKB(size);
        //        bool b1 = float.TryParse(sim_car_network_package.Text, out float SimCarNetworkPackageData);

        //        float packages = 0;
        //        long useTIme = 0;
        //        if (b1)
        //        {
        //            useTIme =
        //            ((DateTime.Now.AddDays(1 - DateTime.Now.Day).Date.AddMonths(1).AddSeconds(-1).ToUniversalTime().Ticks - 621355968000000000) / 10000000) -
        //            ((DateTime.Now.AddDays(1 - DateTime.Now.Day).Date.ToUniversalTime().Ticks - 621355968000000000) / 10000000);
        //            if (UseGB)
        //            {
        //                packages = SimCarNetworkPackageData * 1000 * 1000 * 1000;
        //            }

        //            if (UseMB)
        //            {
        //                packages = SimCarNetworkPackageData * 1000 * 1000;
        //            }

        //            var prespeed = useTIme / (packages / size);
        //            // 在原有基础上加上30%
        //            prespeed = (float)(prespeed + (prespeed * 0.3));
        //            predict_speed.Text = "推荐：" + prespeed + "秒";
        //        }
        //    };
        //    Invoke(action, size);
        //}



        /// <summary>
        /// 创建订阅
        /// </summary>
        /// <param name="mqttClient"></param>
        private void CreateSubscribe()
        {
            if (!this.MqttUtils.IsConnected())
            {
                return;
            }

            string clientId = GetClientIdByConf(this.EdgeConf);
            string username = this.EdgeConf.Username;
            string password = this.EdgeConf.Password;
            this.MqttUtils.Connect(clientId, username, password);

            string[] topics = new string[3]
            {
                this.MqttTopic.DownNotice,
                this.MqttTopic.DownControl,
                this.MqttTopic.DownRestart
            };
            byte[] qosLevels = new byte[3]
            {
                Constants.Qos0,
                Constants.Qos0,
                Constants.Qos0
            };
            this.MqttUtils.Subscribe(topics, qosLevels, MqttMsgPublishReceived);
        }

        private void MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string topic = e.Topic;
            if (topic == this.MqttTopic.DownControl)
            {
//                AddLogToFrom("收到下发命令。");
                DoModifyPoint(e);
            }

            if (topic == this.MqttTopic.DownNotice)
            {
//                AddLogToFrom("收到平台通知。");
                LoadOnce = false;
            }
        }

        private void DoModifyPoint(MqttMsgPublishEventArgs e)
        {
            string originalRtf = Encoding.UTF8.GetString(e.Message);
            var deviceModifyVal = System.Text.Json.JsonSerializer.Deserialize<DeviceModifyVal>(originalRtf);
            string point = deviceModifyVal.PointType + "." + deviceModifyVal.PointId;

            try
            {
                Rundb runbdb = this.RunDbUtils.GetWrite();
                object n = runbdb.Open();
                if (Convert.ToInt16(n) == 1)
                {
                    runbdb.SetVarValueEx(point, deviceModifyVal.BeforeValue);
//                    AddLogToFrom("修改测点：" + point + "=" + deviceModifyVal.beforeValue);
                }

                if (deviceModifyVal.Duration > 0)
                {
                    Thread.Sleep(deviceModifyVal.Duration * 1000);
                    runbdb.SetVarValueEx(point, deviceModifyVal.AfterValue);
//                    AddLogToFrom("脉冲更新：" + point + "=" + deviceModifyVal.afterValue);
                }
                runbdb.Close();
            }
            catch (Exception ex)
            {
//                AddLogToFrom("fmCDOM异常。" + ex.Message);
            }
        }

        public void ConnectMonitor()
        {
            while (true)
            {
                Thread.Sleep(1000);

                if (this.MqttUtils.IsConnected())
                {
                    continue;
                }

                try
                {
                    CreateSubscribe();
//                   AddLogToFrom("MQTT重连成功。");
                }
                catch (Exception ex)
                {
//                    AddLogToFrom("MQTT重连异常。" + ex.Message);
                }
            }
        }

        private class ReadResult
        {
            public int Count { get; set; }
            public int Size { get; set; }
            public byte[] Data { get; set; }
        }

        public class ReportData
        {
            public string Time { get; set; }

            public string DataType { get; set; }

            public string PointType { get; set; }

            public List<PointData> Data { get; set; }
        }


        public class DeviceModifyVal
        {
            public string DeviceId { get; set; }

            public string PointId { get; set; }

            public string PointType { get; set; }

            public double BeforeValue { get; set; }

            public double AfterValue { get; set; }

            public int Duration { get; set; }
        }

        private void SaveButtonClick(object sender, EventArgs e)
        {
            DoSaveConf();
        }

        private void DoSaveConf()
        {
            this.EdgeConf.SelectTag = this.txtSelectTag.Text;
            this.EdgeConf.Username = this.txtUsername.Text;
            this.EdgeConf.Password = this.txtPassword.Text;
            this.EdgeConf.Address = this.txtAddress.Text;
            this.EdgeConf.Port = this.txtPort.Text;
            this.EdgeConf.Repeate = this.txtRepeate.Text;
            this.EdgeConf.Offset = this.txtOffset.Text;
            this.EdgeConf.UserClientId = this.txtUserClientId.Text;
            this.EdgeConf.DeviceDescribe = this.txtDeviceDescribe.Text;
            this.EdgeConf.SaveConf();
        }

        //public static string ByteConversionGBMBKB(Int64 KSize)
        //{
        //    if (KSize / GB >= 1)
        //    {
        //        // 如果当前Byte的值大于等于1GB
        //        // 将其转换成GB
        //        return (Math.Round(KSize / (float)GB, 2)).ToString() + "GB";
        //    }
        //    else if (KSize / MB >= 1)
        //    {
        //        // 如果当前Byte的值大于等于1MB
        //        // 将其转换成MB
        //        return (Math.Round(KSize / (float)MB, 2)).ToString() + "MB";
        //    }
        //    else if (KSize / KB >= 1)
        //    {
        //        // 如果当前Byte的值大于等于1KB
        //        // 将其转换成KB
        //        return (Math.Round(KSize / (float)KB, 2)).ToString() + "KB";
        //    }
        //    else
        //    {
        //        // 显示Byte值
        //        return KSize.ToString() + "Byte";
        //    }
        //}

        private ReadResult ReadDataByType(string pontType)
        {
            ReadResult result = new()
            {
                Data = null,
                Count = 0,
                Size = 0
            };

            Rundb runbdb = null;
            try
            {
                runbdb = RunDbUtils.GetRead();
                object openResult = runbdb.Open();
                if (Convert.ToInt16(openResult) != Constants.OpenOk)
                {
                    // TODO 打开DOM异常
                    return null;
                }

                var data = runbdb.ReadFilterVarValues(pontType, "*");
                if (data == null)
                {
                    // TODO 日志
                    return result;
                }

                List<PointData> dataList = Tools.GetPointDataList(data, pontType);

                DateTime offsetTime = DateTime.Now.AddSeconds(Convert.ToDouble(this.EdgeConf.Offset));
                var reportData = new ReportData
                {
                    Time = offsetTime.ToString(Constants.FormatLongMs),
                    DataType = GetTypeForInstance(pontType),
                    PointType = pontType,
                    Data = dataList,
                };

                string json = Utils.ToJsonStr(reportData);
                byte[] bytes = Encoding.UTF8.GetBytes(json);

                result.Data = bytes;
                result.Count = dataList.Count;
                result.Size = bytes.Length;
            }
            finally
            {
                if (runbdb != null)
                {
                    runbdb.Close();
                }
            }
            return result;
        }

        private void PublishData()
        {
            string[] pointTypeList = this.txtSelectTag.Text.Split(",");
            while (true)
            {
                //var longOnecePackageLen = 0;
                //var pointCount = 0;
                foreach (string pointType in pointTypeList)
                {
                    ReadResult readResult = ReadDataByType(pointType);
                    if (readResult == null)
                    {
                        // TODO 增加重试处理
                        continue;
                    }

                    if (readResult.Count == 0)
                    {
                        // TODO 打印日志
                        continue;
                    }

                    this.MqttUtils.Public(this.MqttTopic.UpData, readResult.Data);
                    // TODO 打印上报成功日志
                }

//                this.point_count.Text = String.Format("{0}", pointCount);
//                AddPackageOnece(longOnecePackageLen);
                Thread.Sleep(Int32.Parse(this.EdgeConf.Repeate) * 1000);
            }
        }

        //private List<RecortMeta> GetALLPointMetaInfo()
        //{
        //    List<RecortMeta> result = new();
        //    foreach (string selectTag in FormArgs_selectTag.Split(","))
        //    {
        //        Rundb runbdb = RunDbUtils.Instance.getRunDb();
        //        string CODE = selectTag;

        //        try
        //        {
        //            object n = runbdb.Open();
        //            if (Convert.ToInt16(n) == 1)
        //            {
        //                var data = "";
        //                if (this.point_filter.Text == "ALL")
        //                {
        //                    data = runbdb.ReadFilterVarValues(CODE, "*");
        //                }
        //                else
        //                {
        //                    data = runbdb.ReadFilterVarValues(CODE, CODE + this.point_filter.Text);
        //                }

        //                List<PointData> dataList = Tools.GetPointDataList(data, CODE);
        //                var reportMeta = new RecortMeta
        //                {
        //                    time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
        //                    dataType = GetTypeForInstance(selectTag),
        //                    pointType = selectTag,
        //                    data = dataList
        //                };
        //                result.Add(reportMeta);
        //                runbdb.Close();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            AddLogToFrom("Metadata fmCDOM异常。" + ex.Message);
        //        }
        //    }
        //    return result;
        //}

        public static string GetTypeForInstance(string codeType)
        {
            string result = codeType switch
            {
                Constants.AR or Constants.AO or Constants.AI or Constants.VA => "double",
                Constants.DR or Constants.DO or Constants.DI or Constants.VD => "int",
                Constants.VT or _ => "string",
            };
            return result;
        }

        private void EdgeFormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = ShowConfirm("确定退出应用吗？");
            if (result == DialogResult.Yes)
            {
                DoSaveConf();
            }
            else
            {
                e.Cancel = true;
            }
        }


        private void RestartButtonClick(object sender, EventArgs e)
        {
            DialogResult result = ShowConfirm("确定重启应用吗？");
            if (result != DialogResult.Yes)
            {
                return;
            }

            DoSaveConf();

            ProcessStartInfo processStartInfo = new()
            {
                WorkingDirectory = AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                FileName = @"DViewEdge.exe",
                CreateNoWindow = true
            };
            _ = Process.Start(processStartInfo);
            Process.GetCurrentProcess()?.Kill();
        }

        private static DialogResult ShowConfirm(string str) 
        {
            return MessageBox.Show(str, "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        private static void ShowInfoBox(string str)
        {
            _ = MessageBox.Show(str, "提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }


        private void AddressLeave(object sender, EventArgs e)
        {
            bool ok = Utils.IsIpAddress(this.txtAddress.Text);
            if (!ok)
            {
                ShowInfoBox("输入的IP地址不合法");
            }
        }

        private void SelectTagLeave(object sender, EventArgs e)
        {
            string[] splits = this.txtSelectTag.Text.Split(",");
            foreach (string s in splits)
            {
                if (!Constants.SupportType.Contains(s))
                {
                    string str = string.Join(",", Constants.SupportType.ToArray());
                    ShowInfoBox("测点类型输入不合法，正确格式为：" + str);
                }
            }
        }


        private void PortLeave(object sender, EventArgs e)
        {
            bool ok = Utils.IsNumber(this.txtPort.Text);
            if (!ok)
            {
                ShowInfoBox("输入的端口不合法");
            }
        }

        private void RepeateLeave(object sender, EventArgs e)
        {
            bool ok = Utils.IsNaturalNumber(this.txtRepeate.Text);
            if (!ok)
            {
                ShowInfoBox("采集频率只允许输入正整数");
            }
        }

        private void OffsetLeave(object sender, EventArgs e)
        {
            bool ok = Utils.IsInteger(this.txtOffset.Text);
            if (!ok)
            {
                ShowInfoBox("时间偏移只允许输入整数");
            }
        }

        private void UserClientIdLeave(object sender, EventArgs e)
        {
            if (this.txtUserClientId.Text.Length == 0)
            {
                return;
            }

            bool ok = Utils.IsClientId(this.txtUserClientId.Text);
            if (!ok)
            {
                ShowInfoBox("客户端ID不合法。\n请以字母开头，长度在4~50之间，只能包含字符、数字和下划线");
            }
        }

        private void DeviceDescribeLeave(object sender, EventArgs e)
        {
            int len = this.txtDeviceDescribe.Text.Length;
            if (len > 50)
            {
                ShowInfoBox("设备名称描述不允许超过50个字符");
            }
        }
    }
}
