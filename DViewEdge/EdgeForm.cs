using FMDMOLib;
using System;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Drawing;

namespace DViewEdge
{
    public partial class EdgeForm : Form
    {
        private Conf EdgeConf { get; }
        private Topic MqttTopic { get; }
        private MqttUtils MqttUtils { get; }
        private RunDbUtils RunDbUtils { get; }
        private Int64 NetSendCount { get; set; }
        private Int64 NetSendBytes { get; set; }
        private Int64 SendSuccedCount { get; set; }
        private Int64 SendErrorCount { get; set; }
        private int MaxLines { get; set; }
        private bool IsPause { get; set; }
        private bool IsFirstSend { get; set; }

        private class SendInfo
        {
            public int Count { get; set; }
            public Int64 Size { get; set; }
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

        public EdgeForm()
        {
            // 初始化窗体组件
            InitializeComponent();

            // 设置为false能安全的访问窗体控件
            CheckForIllegalCrossThreadCalls = false;

            // 加载配置文件
            this.EdgeConf = new Conf();

            // 初始化显示内容
            this.InitFormData(this.EdgeConf);

            // 获取客户端ID
            string clientId = this.GetClientIdByConf(this.EdgeConf);

            // 加载Topic信息
            this.MqttTopic = new Topic(clientId);

            // 初始化MQTT客户端
            this.MqttUtils = MqttUtils.GetInastance(this.EdgeConf.Address, this.EdgeConf.Port);
            this.MqttUtils.AddPublishedHandler(MqttMsgPublished);

            // 初始化COM客户端
            this.RunDbUtils = RunDbUtils.GetInstance();

            // 启动后台任务
            this.StartTask();
        }

        /// <summary>
        /// 启动后台任务
        /// </summary>
        private void StartTask()
        {
            // 监控任务
            Task connectMonitor = new(() => { ConnectMonitor(); });
            connectMonitor.Start();
            AppendLog("启动监控任务");

            // 订阅任务
            Task topicSubscribe = new(() => { CreateSubscribe(); });
            topicSubscribe.Start();
            AppendLog("启动订阅任务");

            // 采集任务
            Task publishData = new(() => { PublishData(); });
            publishData.Start();
            AppendLog("启动采集任务");
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
        private void InitFormData(Conf conf)
        {
            this.lblCount.Text = "0";
            this.lblSize.Text = "0";
            this.lblSucceed.Text = "0";
            this.lblError.Text = "0";
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

            // 初始化连接状态
            StatusOk();

            // 初始化全局变量
            this.MaxLines = 100;
            this.IsPause = false;
            this.IsFirstSend = true;

            // 位于屏幕最前面
            this.TopMost = true;

            // 流量计数初始化
            this.NetSendCount = 0;
            this.NetSendBytes = 0;
            this.SendSuccedCount = 0;
            this.SendErrorCount = 0;
        }

        /// <summary>
        /// 创建订阅
        /// </summary>
        private void CreateSubscribe()
        {
            if (this.MqttUtils.IsConnected())
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
            string msg = Encoding.UTF8.GetString(e.Message);

            if (topic == this.MqttTopic.DownControl)
            {
                AppendLog(String.Format("命令下发通知：{0}", msg));
                DoModifyPoint(msg);
            }
            if (topic == this.MqttTopic.DownNotice)
            {
                AppendLog(String.Format("上报数据通知：{0}", msg));
                this.DoSendMetaOnce();
            }
        }

        public void MqttMsgPublished(object sender, MqttMsgPublishedEventArgs e)
        {
            if (e.IsPublished)
            {
                this.SendSuccedCount += 1;
            }
            else
            {
                this.SendErrorCount += 1;
            }
        }

        private void DoModifyPoint(string msg)
        {
            DeviceModifyVal val = Utils.StrToJson(msg);
            string point = val.PointType + "." + val.PointId;

            try
            {
                Rundb runbdb = this.RunDbUtils.GetWrite();
                object n = runbdb.Open();
                if (Convert.ToInt16(n) == 1)
                {
                    runbdb.SetVarValueEx(point, val.BeforeValue);
                    AppendLog(String.Format("修改测点{0}值为{1}", point, val.BeforeValue));
                }

                if (val.Duration > 0)
                {
                    Thread.Sleep(val.Duration * 1000);
                    runbdb.SetVarValueEx(point, val.AfterValue);
                    AppendLog(String.Format("修改测点{0}值为{1}", point, val.AfterValue));
                }
                runbdb.Close();
            }
            catch (Exception e)
            {
                AppendLog(String.Format("命令下发异常：{0}", e.Message));
            }
        }

        public void ConnectMonitor()
        {
            while (true)
            {
                Thread.Sleep(1000);
                if (this.MqttUtils.IsConnected())
                {
                    StatusOk();
                    continue;
                }

                try
                {
                    this.CreateSubscribe();
                    StatusOk();
                    AppendLog("重新连接平台");
                }
                catch (Exception e)
                {
                    StatusNg();
                    this.SendErrorCount += 1;
                    AppendLog(String.Format("平台连接异常：{0}", e.Message));
                }
                finally
                {
                    AppendLog("事件提醒：尝试重新连接");
                }
            }
        }

        private void StatusOk()
        {
            this.lblStatus.Text = "正常";
            this.lblStatus.ForeColor = Color.Blue;
        }

        private void StatusNg()
        {
            this.lblStatus.Text = "离线";
            this.lblStatus.ForeColor = Color.Red;
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


        private static SendInfo GetSendInfoForData(ReportData data)
        {
            string json = Utils.JsonToStr(data);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            return new SendInfo
            {
                Data = bytes,
                Size = bytes.Length,
                Count = data.Data.Count
            };
        }

        private static SendInfo GetSendInfoForMeta(List<ReportData> list)
        {
            int count = 0;
            foreach (ReportData data in list)
            {
                count += data.Data.Count;
            }
            string json = Utils.JsonToStr(list);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            return new SendInfo
            {
                Data = bytes,
                Size = bytes.Length,
                Count = count
            };
        }

        private ReportData GetReportData(string pontType, out bool isError, out bool noData)
        {
            Rundb runbdb = null;
            try
            {
                // 偏移后的时间
                DateTime offsetTime = DateTime.Now.AddSeconds(Convert.ToDouble(this.EdgeConf.Offset));

                // 打开COM接口
                runbdb = RunDbUtils.GetRead();
                object openResult = runbdb.Open();
                if (Convert.ToInt16(openResult) != Constants.OpenOk)
                {
                    AppendLog(String.Format("COM接口异常，应答结果：{0}", openResult));
                    isError = true;
                    noData = true;
                    return null;
                }

                // 读取测点数据
                var data = runbdb.ReadFilterVarValues(pontType, "*");
                if (data == null)
                {
                    isError = false;
                    noData = true;
                    return null;
                }

                // 转换数据结构
                List<PointData> dataList = Tools.GetPointDataList(data, pontType);

                // 生成应答数据
                var reportData = new ReportData
                {
                    Time = offsetTime.ToString(Constants.FormatLongMs),
                    DataType = GetDataType(pontType),
                    PointType = pontType,
                    Data = dataList,
                };
                isError = false;
                noData = false;
                return reportData;
            }
            finally
            {
                if (runbdb != null)
                {
                    runbdb.Close();
                }
            }
        }

        private void PublishData()
        {
            while (true)
            {
                // 上报测点元数据
                if (this.IsFirstSend)
                {
                    this.DoSendMetaOnce();
                    this.IsFirstSend = false;
                }

                // 上报各类型数据
                this.DoSendDataOnce();

                // 等待下一次上报
                Thread.Sleep(Int32.Parse(this.EdgeConf.Repeate) * 1000);
            }
        }

        private void DoSendMetaOnce()
        {
            try
            {
                List<ReportData> dataList = new();
                string[] pointTypeList = this.txtSelectTag.Text.Split(",");
                foreach (string pointType in pointTypeList)
                {
                    // 读取数据
                    ReportData reportData = this.GetReportData(pointType, out bool isError, out bool noData);

                    // 接口异常
                    if (isError)
                    {
                        continue;
                    }

                    // 没有数据
                    if (noData)
                    {
                        continue;
                    }
                    dataList.Add(reportData);
                }

                SendInfo sendInfo = GetSendInfoForMeta(dataList);

                // 发送数据
                this.MqttUtils.Public(this.MqttTopic.UpMeta, sendInfo.Data);
                AppendLog(String.Format("上报测点元数据, 测点数{0}", sendInfo.Count));

                // 流量计数
                this.NetSendCount += 1;
                this.NetSendBytes += sendInfo.Size;
            }
            catch (Exception e)
            {
                this.SendErrorCount += 1;
                AppendLog(String.Format("COM接口异常:{0}", e.Message));
            }
            finally
            {
                RefreshFlowData();
            }
        }

        private void DoSendDataOnce()
        {
            try
            {
                string[] pointTypeList = this.txtSelectTag.Text.Split(",");
                foreach (string pointType in pointTypeList)
                {
                    // 读取数据
                    ReportData reportData = this.GetReportData(pointType, out bool isError, out bool noData);

                    // 接口异常
                    if (isError)
                    {
                        this.SendErrorCount += 1;
                        continue;
                    }

                    // 没有数据
                    if (noData)
                    {
                        continue;
                    }

                    // 流量数据
                    SendInfo sendInfo = GetSendInfoForData(reportData);

                    // 发送数据
                    this.MqttUtils.Public(this.MqttTopic.UpData, sendInfo.Data);
                    AppendLog(String.Format("上报{0}类型测点{1}个", pointType, sendInfo.Count));

                    // 流量计数
                    this.NetSendCount += 1;
                    this.NetSendBytes += sendInfo.Size;
                }
            }
            catch (Exception e)
            {
                this.SendErrorCount += 1;
                AppendLog(String.Format("COM接口异常:{0}", e.Message));
            }
            finally
            {
                RefreshFlowData();
            }
        }

        private void RefreshFlowData()
        {
            this.lblCount.Text = this.NetSendCount.ToString();
            this.lblSize.Text = Utils.ConvertSize(this.NetSendBytes);
            this.lblSucceed.Text = this.SendSuccedCount.ToString();
            this.lblError.Text = this.SendErrorCount.ToString();
        }

        private static string GetDataType(string pointType)
        {
            string result = pointType switch
            {
                Constants.AR or Constants.AO or Constants.AI or Constants.VA => "double",
                Constants.DR or Constants.DO or Constants.DI or Constants.VD => "int",
                Constants.VT or _ => "string",
            };
            return result;
        }

        private void EdgeFormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = Utils.ShowConfirm("确定退出应用吗？");
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
            DialogResult result = Utils.ShowConfirm("确定重启应用吗？");
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

        private void AddressLeave(object sender, EventArgs e)
        {
            bool ok = Utils.IsIpAddress(this.txtAddress.Text);
            if (!ok)
            {
                Utils.ShowInfoBox("输入的IP地址不合法");
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
                    Utils.ShowInfoBox("测点类型输入不合法，正确格式为：" + str);
                }
            }
        }

        private void PortLeave(object sender, EventArgs e)
        {
            bool ok = Utils.IsNumber(this.txtPort.Text);
            if (!ok)
            {
                Utils.ShowInfoBox("输入的端口不合法");
            }
        }

        private void RepeateLeave(object sender, EventArgs e)
        {
            bool ok = Utils.IsNaturalNumber(this.txtRepeate.Text);
            if (!ok)
            {
                Utils.ShowInfoBox("采集频率只允许输入正整数");
            }
        }

        private void OffsetLeave(object sender, EventArgs e)
        {
            bool ok = Utils.IsInteger(this.txtOffset.Text);
            if (!ok)
            {
                Utils.ShowInfoBox("时间偏移只允许输入整数");
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
                Utils.ShowInfoBox("客户端ID不合法。\n请以字母开头，长度在4~50之间，只能包含字符、数字和下划线");
            }
        }

        private void DeviceDescribeLeave(object sender, EventArgs e)
        {
            int len = this.txtDeviceDescribe.Text.Length;
            if (len > 50)
            {
                Utils.ShowInfoBox("设备名称描述不允许超过50个字符");
            }
        }

        private void LogBtnClieck(object sender, EventArgs e)
        {
            if (this.splitContainer.Panel2Collapsed == false)
            {
                // 折叠
                this.splitContainer.Panel2Collapsed = true;
                this.btnLog.Text = "打开日志";
            }
            else
            {
                // 打开
                this.splitContainer.Panel2Collapsed = false;
                this.btnLog.Text = "关闭日志";
            }
        }

        private void PauseBtnClienk(object sender, EventArgs e)
        {
            if (this.IsPause)
            {
                this.IsPause = false;
                this.btnPause.Text = "暂停";
            }
            else
            {
                this.IsPause = true;
                this.btnPause.Text = "打开";
            }
        }

        private void CleanBtnClick(object sender, EventArgs e)
        {
            this.rtbLogContent.Text = "";
        }

        private void AppendLog(string str)
        {
            if (this.IsPause)
            {
                return;
            }
            string log = string.Format("{0}:{1}\n", Utils.TimeNow(), str);
            this.rtbLogContent.AppendText(log);
        }

        private void LogContentTextChanged(object sender, EventArgs e)
        {
            if (this.rtbLogContent.Lines.Length > this.MaxLines)
            {
                this.rtbLogContent.Text = this.rtbLogContent.Text[(this.rtbLogContent.Lines[0].Length + 1)..];
                this.rtbLogContent.Select(this.rtbLogContent.Text.Length, 0);
            }
        }

        private void MaxLinesLeave(object sender, EventArgs e)
        {
            if (this.txtMaxLines.Text == "")
            {
                Utils.ShowInfoBox("显示行数不允许为空");
                return;
            }

            bool ok = Utils.IsNaturalNumber(this.txtMaxLines.Text);
            if (!ok)
            {
                Utils.ShowInfoBox("显示行数只允许输入正整数");
                return;
            }

            int maxLines = Convert.ToInt16(this.txtMaxLines.Text);
            if (maxLines <= 0)
            {
                Utils.ShowInfoBox("显示行数必须大于0");
                return;
            }
            if (maxLines > 1000)
            {
                Utils.ShowInfoBox("显示行数不能超过1000");
                return;
            }

            this.MaxLines = maxLines;
        }

        private void UpMetaBtnClick(object sender, EventArgs e)
        {
            // 防止重复操作
            this.btnUpMeta.Enabled = false;

            // 上报测点数据
            this.DoSendMetaOnce();

            Thread.Sleep(1000);
            this.btnUpMeta.Enabled = true;
        }
    }
}
