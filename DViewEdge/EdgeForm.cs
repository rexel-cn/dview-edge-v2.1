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
        private long NetSendCount { get; set; }
        private long NetSendBytes { get; set; }
        private long SendSuccedCount { get; set; }
        private long SendErrorCount { get; set; }
        private int MaxLines { get; set; }
        private bool IsPause { get; set; }
        private bool IsFirstSend { get; set; }

        /// <summary>
        /// 流量信息
        /// </summary>
        private class SendInfo
        {
            public int Count { get; set; }
            public long Size { get; set; }
            public byte[] Data { get; set; }
        }

        /// <summary>
        /// 运行数据
        /// </summary>
        public class ReportData
        {
            public string Time { get; set; }
            public string DataType { get; set; }
            public string PointType { get; set; }
            public List<PointData> Data { get; set; }
        }

        /// <summary>
        /// 测点信息
        /// </summary>
        public class PointData
        {
            public string PointId { get; set; }
            public object PointValue { get; set; }
            public string Qty { get; set; }
        }

        /// <summary>
        /// 设备元数据
        /// </summary>
        public class DeviceMeta
        {
            public string Username { get; set; }
            public string DeviceDescribe { get; set; }
            public List<ReportData> PointList { get; set; }
        }

        /// <summary>
        /// 下发数据
        /// </summary>
        public class DeviceModifyVal
        {
            public string DeviceId { get; set; }
            public string PointId { get; set; }
            public string PointType { get; set; }
            public double BeforeValue { get; set; }
            public double AfterValue { get; set; }
            public int Duration { get; set; }
        }

        /// <summary>
        /// 主窗体
        /// </summary>
        public EdgeForm()
        {
            // 初始化窗体组件
            InitializeComponent();

            // 设置为false能安全的访问窗体控件
            CheckForIllegalCrossThreadCalls = false;

            // 加载配置文件
            EdgeConf = new Conf();

            // 初始化显示内容
            InitFormData(EdgeConf);

            // 获取客户端ID
            string clientId = GetClientIdByConf(EdgeConf);

            // 加载Topic信息
            MqttTopic = new Topic(clientId);

            // 初始化MQTT客户端
            MqttUtils = MqttUtils.GetInastance(EdgeConf.Address, EdgeConf.Port);
            MqttUtils.AddPublishedHandler(MqttMsgPublished);

            // 初始化COM客户端
            RunDbUtils = RunDbUtils.GetInstance();

            // 启动后台任务
            StartTask();
        }

        /// <summary>
        /// 初始化显示内容及全局变量
        /// </summary>
        /// <param name="conf">配置文件</param>
        private void InitFormData(Conf conf)
        {
            lblCount.Text = "0";
            lblSize.Text = "0";
            lblSucceed.Text = "0";
            lblError.Text = "0";
            lblStartTime.Text = Utils.TimeNow();
            lblMachineCode.Text = FingerPrint.Value();

            txtUsername.Text = conf.Username;
            txtPassword.Text = conf.Password;
            txtAddress.Text = conf.Address;
            txtPort.Text = conf.Port;
            txtSelectTag.Text = conf.SelectTag;
            txtRepeate.Text = conf.Repeate;
            txtOffset.Text = conf.Offset;
            txtUserClientId.Text = conf.UserClientId;
            txtDeviceDescribe.Text = conf.DeviceDescribe;

            // 初始化连接状态
            StatusNg();

            // 初始化全局变量
            MaxLines = 100;
            IsPause = false;
            IsFirstSend = true;

            // 位于屏幕最前面
            TopMost = true;

            // 流量计数初始化
            NetSendCount = 0;
            NetSendBytes = 0;
            SendSuccedCount = 0;
            SendErrorCount = 0;
        }

        /// <summary>
        /// 启动后台任务
        /// </summary>
        private void StartTask()
        {
            // 监控任务
            Task task1 = new(() => { CheckConnect(); });
            task1.Start();
            AppendLog("启动监控任务");

            // 订阅任务
            Task task2 = new(() => { TopicSubscribe(); });
            task2.Start();
            AppendLog("启动订阅任务");

            // 采集任务
            Task task3 = new(() => { PublishRunData(); });
            task3.Start();
            AppendLog("启动采集任务");
        }

        /// <summary>
        /// 根据配置文件获取客户端Id
        /// </summary>
        /// <param name="conf">配置文件</param>
        /// <returns>客户端ID</returns>
        private string GetClientIdByConf(Conf conf)
        {
            if (string.IsNullOrEmpty(conf.UserClientId))
            {
                return lblMachineCode.Text;
            }
            else
            {
                return conf.UserClientId;
            }
        }

        /// <summary>
        /// 检查连接状态
        /// </summary>
        private void CheckConnect()
        {
            while (true)
            {
                Thread.Sleep(1000);
                if (MqttUtils.IsConnected())
                {
                    StatusOk();
                    continue;
                }

                TopicSubscribe();
            }
        }

        /// <summary>
        /// 创建消息订阅
        /// </summary>
        private void TopicSubscribe()
        {
            if (MqttUtils.IsConnected())
            {
                Console.Write("TopicSubscribe. MQTT IsConnected.");
                return;
            }

            try
            {
                string clientId = GetClientIdByConf(EdgeConf);
                string username = EdgeConf.Username;
                string password = EdgeConf.Password;
                MqttUtils.Connect(clientId, username, password);

                string[] topics = new string[3]
                {
                    MqttTopic.DownNotice,
                    MqttTopic.DownControl,
                    MqttTopic.DownRestart
                };
                byte[] qosLevels = new byte[3]
                {
                    Constants.Qos0,
                    Constants.Qos0,
                    Constants.Qos0
                };
                MqttUtils.Subscribe(topics, qosLevels, MqttMsgPublishReceived);

                StatusOk();
                AppendLog("重新连接平台");
            }
            catch (Exception e)
            {
                StatusNg();
                SendErrorCount += 1;
                AppendLog(string.Format("MQTT Broker连接异常。{0}", e.Message));
            }
        }

        /// <summary>
        /// 发送运行数据
        /// </summary>
        private void PublishRunData()
        {
            while (true)
            {
                // 上报测点元数据
                if (IsFirstSend)
                {
                    DoSendMetaOnce();
                    IsFirstSend = false;
                }

                // 上报各类型数据
                DoSendDataOnce();

                // 等待下一次上报
                Thread.Sleep(int.Parse(EdgeConf.Repeate) * 1000);
            }
        }

        /// <summary>
        /// 执行元数据上报
        /// </summary>
        private void DoSendMetaOnce()
        {
            try
            {
                List<ReportData> dataList = new();
                string[] pointTypeList = txtSelectTag.Text.Split(",");
                foreach (string pointType in pointTypeList)
                {
                    ReportData reportData = GetReportData(pointType, out bool isError, out bool noData);
                    if (isError)
                    {
                        SendErrorCount += 1;
                        continue;
                    }
                    if (noData)
                    {
                        continue;
                    }
                    if (reportData != null)
                    {
                        dataList.Add(reportData);
                    }
                }

                DeviceMeta meta = new()
                {
                    Username = EdgeConf.Username,
                    DeviceDescribe = txtDeviceDescribe.Text,
                    PointList = dataList
                };
                SendInfo sendInfo = GetSendInfoForMeta(meta);

                if (sendInfo.Count > 0)
                {
                    // 发送数据
                    MqttUtils.Public(MqttTopic.UpMeta, sendInfo.Data);
                    AppendLog(string.Format("上报测点元数据, 测点数{0}", sendInfo.Count));

                    // 流量计数
                    NetSendCount += 1;
                    NetSendBytes += sendInfo.Size;
                }
            }
            catch (Exception e)
            {
                SendErrorCount += 1;
                AppendLog(string.Format("COM接口异常:{0}", e.Message));
            }
            finally
            {
                RefreshFlowData();
            }
        }

        /// <summary>
        /// 执行运行数据上报
        /// </summary>
        private void DoSendDataOnce()
        {
            try
            {
                string[] pointTypeList = txtSelectTag.Text.Split(",");
                foreach (string pointType in pointTypeList)
                {
                    ReportData reportData = GetReportData(pointType, out bool isError, out bool noData);
                    if (isError)
                    {
                        Console.WriteLine("DoSendDataOnce. isError");
                        SendErrorCount += 1;
                        continue;
                    }
                    if (noData)
                    {
                        Console.WriteLine("DoSendDataOnce. noData");
                        continue;
                    }
                    if (reportData == null)
                    {
                        Console.WriteLine("DoSendDataOnce. reportData == null");
                        continue;
                    }
                    SendInfo sendInfo = GetSendInfoForData(reportData);
                    Console.WriteLine(string.Format("DoSendDataOnce. sendInfo: {0}", reportData.ToString()));

                    if (sendInfo.Count > 0)
                    {
                        // 发送数据
                        MqttUtils.Public(MqttTopic.UpData, sendInfo.Data);
                        AppendLog(string.Format("上报{0}类型测点{1}个", pointType, sendInfo.Count));

                        // 流量计数
                        NetSendCount += 1;
                        NetSendBytes += sendInfo.Size;
                    }
                }
            }
            catch (Exception e)
            {
                SendErrorCount += 1;
                AppendLog(string.Format("COM接口异常:{0}", e.Message));
            }
            finally
            {
                RefreshFlowData();
            }
        }

        /// <summary>
        /// 执行命令下发
        /// </summary>
        /// <param name="msg">msg</param>
        private void DoModifyPoint(string msg)
        {
            try
            {
                DeviceModifyVal val = Utils.StrToJson(msg);
                string point = val.PointType + "." + val.PointId;

                Rundb runbdb = RunDbUtils.GetWrite();
                object n = runbdb.Open();
                if (Convert.ToInt16(n) == 1)
                {
                    runbdb.SetVarValueEx(point, val.BeforeValue);
                    AppendLog(string.Format("修改测点{0}值为{1}", point, val.BeforeValue));
                }

                if (val.Duration > 0)
                {
                    Thread.Sleep(val.Duration * 1000);
                    runbdb.SetVarValueEx(point, val.AfterValue);
                    AppendLog(string.Format("修改测点{0}值为{1}", point, val.AfterValue));
                }
                runbdb.Close();
            }
            catch (Exception e)
            {
                AppendLog(string.Format("命令下发异常：{0}", e.Message));
            }
        }

        /// <summary>
        /// 执行保存配置文件
        /// </summary>
        private void DoSaveConf()
        {
            EdgeConf.SelectTag = txtSelectTag.Text;
            EdgeConf.Username = txtUsername.Text;
            EdgeConf.Password = txtPassword.Text;
            EdgeConf.Address = txtAddress.Text;
            EdgeConf.Port = txtPort.Text;
            EdgeConf.Repeate = txtRepeate.Text;
            EdgeConf.Offset = txtOffset.Text;
            EdgeConf.UserClientId = txtUserClientId.Text;
            EdgeConf.DeviceDescribe = txtDeviceDescribe.Text;
            EdgeConf.SaveConf();
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
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

        /// <summary>
        /// 保存配置按钮按下
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void SaveButtonClick(object sender, EventArgs e)
        {
            DoSaveConf();
        }

        /// <summary>
        /// 重启按钮按下
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void RestartButtonClick(object sender, EventArgs e)
        {
            // 提示信息
            DialogResult result = Utils.ShowConfirm("确定重启应用吗？");
            if (result != DialogResult.Yes)
            {
                return;
            }

            // 保存配置
            DoSaveConf();

            // 重启应用
            ProcessStartInfo processStartInfo = new()
            {
                WorkingDirectory = AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                FileName = @"DViewEdge.exe",
                CreateNoWindow = true
            };
            _ = Process.Start(processStartInfo);
            Process.GetCurrentProcess()?.Kill();
        }

        /// <summary>
        /// 上报测点按钮按下
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void UpMetaBtnClick(object sender, EventArgs e)
        {
            // 防止重复操作
            btnUpMeta.Enabled = false;

            // 上报测点数据
            DoSendMetaOnce();

            Thread.Sleep(1000);
            btnUpMeta.Enabled = true;
        }

        /// <summary>
        /// 打开日志按钮按下
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void LogBtnClick(object sender, EventArgs e)
        {
            if (splitContainer.Panel2Collapsed == false)
            {
                // 折叠
                splitContainer.Panel2Collapsed = true;
                btnLog.Text = "打开日志";
            }
            else
            {
                // 打开
                splitContainer.Panel2Collapsed = false;
                btnLog.Text = "关闭日志";
            }
        }

        /// <summary>
        /// 暂停按钮按下
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void PauseBtnClick(object sender, EventArgs e)
        {
            if (IsPause)
            {
                IsPause = false;
                btnPause.Text = "暂停";
            }
            else
            {
                IsPause = true;
                btnPause.Text = "打开";
            }
        }

        /// <summary>
        /// 清除按钮按下
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void CleanBtnClick(object sender, EventArgs e)
        {
            rtbLogContent.Text = "";
        }

        /// <summary>
        /// 获取运行数据流量信息
        /// </summary>
        /// <param name="data">data</param>
        /// <returns>SendInfo</returns>
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

        /// <summary>
        /// 获取测点数据流量信息
        /// </summary>
        /// <param name="meta">meta</param>
        /// <returns>SendInfo</returns>
        private static SendInfo GetSendInfoForMeta(DeviceMeta meta)
        {
            int count = 0;
            foreach (ReportData data in meta.PointList)
            {
                count += data.Data.Count;
            }
            string json = Utils.JsonToStr(meta);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            return new SendInfo
            {
                Data = bytes,
                Size = bytes.Length,
                Count = count
            };
        }

        /// <summary>
        /// 读取指定类型测点运行数据
        /// </summary>
        /// <param name="pontType">测点类型</param>
        /// <param name="isError">是否异常</param>
        /// <param name="noData">是否有数据</param>
        /// <returns>ReportData</returns>
        private ReportData GetReportData(string pontType, out bool isError, out bool noData)
        {
            Rundb runbdb = null;
            try
            {
                // 偏移后的时间
                DateTime offsetTime = DateTime.Now.AddSeconds(Convert.ToDouble(EdgeConf.Offset));

                // 打开COM接口
                runbdb = RunDbUtils.GetRead();
                object openResult = runbdb.Open();
                if (Convert.ToInt16(openResult) != Constants.OpenOk)
                {
                    AppendLog(string.Format("COM接口异常，应答结果：{0}", openResult.ToString()));
                    isError = true;
                    noData = true;
                    return null;
                }

                // 读取测点数据
                var data = runbdb.ReadFilterVarValues(pontType, "*");
                runbdb.Close();
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
                    DataType = Utils.GetDataType(pontType),
                    PointType = pontType,
                    Data = dataList,
                };
                isError = false;
                noData = false;
                return reportData;
            }
            catch (Exception e)
            {
                isError = true;
                noData = true;
                AppendLog(string.Format("COM接口打开异常：{0}", e.Message));
                return null;
            }
        }

        /// <summary>
        /// 刷新流量数据
        /// </summary>
        private void RefreshFlowData()
        {
            lblCount.Text = NetSendCount.ToString("###,###");
            lblSize.Text = Utils.ConvertSize(NetSendBytes);
            lblSucceed.Text = SendSuccedCount.ToString("###,###");
            lblError.Text = SendErrorCount.ToString("###,###");
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="str">str</param>
        private void AppendLog(string str)
        {
            if (IsPause)
            {
                return;
            }
            string log = string.Format("{0}:{1}\n", Utils.TimeNow(), str);
            rtbLogContent.AppendText(log);
        }

        /// <summary>
        /// 内容合法检查：地址
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void AddressLeave(object sender, EventArgs e)
        {
            bool ok = Utils.IsIpAddress(txtAddress.Text);
            if (!ok)
            {
                Utils.ShowInfoBox("输入的IP地址不合法");
            }
        }

        /// <summary>
        /// 内容合法检查：端口
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void PortLeave(object sender, EventArgs e)
        {
            bool ok = Utils.IsNumber(txtPort.Text);
            if (!ok)
            {
                Utils.ShowInfoBox("输入的端口不合法");
            }
        }

        /// <summary>
        /// 内容合法检查：测点类型
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void SelectTagLeave(object sender, EventArgs e)
        {
            string[] splits = txtSelectTag.Text.Split(",");
            foreach (string s in splits)
            {
                if (!Constants.SupportType.Contains(s))
                {
                    string str = string.Join(",", Constants.SupportType.ToArray());
                    Utils.ShowInfoBox("测点类型输入不合法，正确格式为：" + str);
                }
            }
        }

        /// <summary>
        /// 内容合法检查：采集频率
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void RepeateLeave(object sender, EventArgs e)
        {
            bool ok = Utils.IsNaturalNumber(txtRepeate.Text);
            if (!ok)
            {
                Utils.ShowInfoBox("采集频率只允许输入正整数");
            }
        }

        /// <summary>
        /// 内容合法检查：时间偏移
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void OffsetLeave(object sender, EventArgs e)
        {
            bool ok = Utils.IsInteger(txtOffset.Text);
            if (!ok)
            {
                Utils.ShowInfoBox("时间偏移只允许输入整数");
            }
        }

        /// <summary>
        /// 内容合法检查：自定义客户端
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void UserClientIdLeave(object sender, EventArgs e)
        {
            if (txtUserClientId.Text.Length == 0)
            {
                return;
            }

            bool ok = Utils.IsClientId(txtUserClientId.Text);
            if (!ok)
            {
                Utils.ShowInfoBox("客户端ID不合法。\n请以字母开头，长度在4~50之间，只能包含字符、数字和下划线");
            }
        }

        /// <summary>
        /// 内容合法检查：设备名称描述
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void DeviceDescribeLeave(object sender, EventArgs e)
        {
            int len = txtDeviceDescribe.Text.Length;
            if (len > 50)
            {
                Utils.ShowInfoBox("设备名称描述不允许超过50个字符");
            }
        }

        /// <summary>
        /// 内容合法检查：最大行数
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void MaxLinesLeave(object sender, EventArgs e)
        {
            if (txtMaxLines.Text == "")
            {
                Utils.ShowInfoBox("显示行数不允许为空");
                return;
            }

            bool ok = Utils.IsNaturalNumber(txtMaxLines.Text);
            if (!ok)
            {
                Utils.ShowInfoBox("显示行数只允许输入正整数");
                return;
            }

            int maxLines = Convert.ToInt16(txtMaxLines.Text);
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

            MaxLines = maxLines;
        }

        /// <summary>
        /// 日志内容更新
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void LogContentTextChanged(object sender, EventArgs e)
        {
            if (rtbLogContent.Lines.Length > MaxLines)
            {
                rtbLogContent.Text = rtbLogContent.Text[(rtbLogContent.Lines[0].Length + 1)..];
                rtbLogContent.Select(rtbLogContent.Text.Length, 0);
            }
        }

        /// <summary>
        /// 设置连接正常显示
        /// </summary>
        private void StatusOk()
        {
            lblStatus.Text = "正常";
            lblStatus.ForeColor = Color.Blue;
        }

        /// <summary>
        /// 设置连接异常显示
        /// </summary>
        private void StatusNg()
        {
            lblStatus.Text = "离线";
            lblStatus.ForeColor = Color.Red;
        }

        /// <summary>
        /// Mqtt客户端接收消息
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string topic = e.Topic;
            string msg = Encoding.UTF8.GetString(e.Message);

            if (topic == MqttTopic.DownControl)
            {
                AppendLog(string.Format("命令下发通知：{0}", msg));
                DoModifyPoint(msg);
            }
            if (topic == MqttTopic.DownNotice)
            {
                AppendLog(string.Format("上报数据通知：{0}", msg));
                DoSendMetaOnce();
            }
        }

        /// <summary>
        /// Mqtt客户端发送消息应答
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void MqttMsgPublished(object sender, MqttMsgPublishedEventArgs e)
        {
            if (e.IsPublished)
            {
                SendSuccedCount += 1;
            }
            else
            {
                SendErrorCount += 1;
            }
        }
    }
}
