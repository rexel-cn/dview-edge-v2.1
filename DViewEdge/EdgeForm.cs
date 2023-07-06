using System;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Drawing;
using static DViewEdge.RunDbUtils;
using FMDMOLib;
using DViewEdge.Properties;

namespace DViewEdge
{
    public partial class EdgeForm : Form
    {
        private EdgePoint EdgePoint { get; }
        private Conf EdgeConf { get; }
        private Log Log { get; }
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
            try
            {
                // 初始化窗体组件
                InitializeComponent();

                // 设置为false能安全的访问窗体控件
                CheckForIllegalCrossThreadCalls = false;

                // 加载日志配置
                Log = new Log();

                // 加载配置文件
                EdgeConf = new Conf();

                // 打印配置内容
                PrintConf();

                // 初始化显示内容
                InitFormData(EdgeConf);
                Log.WriteAppend("完成初始化显示内容");

                // 获取客户端ID
                string clientId = GetClientIdByConf(EdgeConf);
                Log.WriteAppend("完成客户端ID生成");

                // 加载Topic信息
                MqttTopic = new Topic(clientId);
                Log.WriteAppend("完成加载Topic信息");

                // 初始化MQTT客户端
                MqttUtils = MqttUtils.GetInastance(EdgeConf.Address, EdgeConf.Port);
                MqttUtils.AddPublishedHandler(MqttMsgPublished);
                Log.WriteAppend("完成MQTT客户端初始化");

                // 初始化COM客户端
                RunDbUtils = RunDbUtils.GetInstance();
                Log.WriteAppend("完成COM客户端初始化");

                // 分频采集子窗体
                EdgePoint = new EdgePoint(EdgeConf, RunDbUtils);

                // 启动后台任务
                StartTask();
            }
            catch (Exception e)
            {
                Log.WriteAppend(e.Message);
            }
        }

        /// <summary>
        /// 将配置文件读取结果写入日志
        /// </summary>
        private void PrintConf()
        {
            Log.WriteAppend("[config]repeate:" + EdgeConf.Repeate);
            Log.WriteAppend("[config]username:" + EdgeConf.Username);
            Log.WriteAppend("[config]password:" + EdgeConf.Password);
            Log.WriteAppend("[config]address:" + EdgeConf.Address);
            Log.WriteAppend("[config]port:" + EdgeConf.Port);
            Log.WriteAppend("[config]selectTag:" + EdgeConf.SelectTag);
            Log.WriteAppend("[config]offset:" + EdgeConf.Offset);
            Log.WriteAppend("[config]userClientId:" + EdgeConf.UserClientId);
            Log.WriteAppend("[config]deviceDescribe:" + EdgeConf.DeviceDescribe);
            Log.WriteAppend("[auth]username:" + EdgeConf.AuthUser);
            Log.WriteAppend("[auth]password:" + EdgeConf.AuthPass);
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
            txtRepeate.Text = conf.Repeate;
            txtOffset.Text = conf.Offset;
            txtUserClientId.Text = conf.UserClientId;
            txtDeviceDescribe.Text = conf.DeviceDescribe;

            // 初始化选择框
            InitPointTypeCheckBox(conf.SelectTag);

            // 初始化连接状态
            PlatformNg();

            // 初始化全局变量
            MaxLines = 100;
            IsPause = false;
            IsFirstSend = true;

            // 流量计数初始化
            NetSendCount = 0;
            NetSendBytes = 0;
            SendSuccedCount = 0;
            SendErrorCount = 0;
        }

        private void InitPointTypeCheckBox(string selectTag)
        {
            if (string.IsNullOrEmpty(selectTag))
            {
                return;
            }

            string[] pointTypeList = selectTag.Split(',');
            foreach (string pointType in pointTypeList)
            {
                switch (pointType)
                {
                    case Constants.AR:
                        checkBoxAr.Checked = true;
                        break;
                    case Constants.AI:
                        checkBoxAi.Checked = true;
                        break;
                    case Constants.AO:
                        checkBoxAo.Checked = true;
                        break;
                    case Constants.DR:
                        checkBoxDr.Checked = true;
                        break;
                    case Constants.DI:
                        checkBoxDi.Checked = true;
                        break;
                    case Constants.DO:
                        checkBoxDo.Checked = true;
                        break;
                    case Constants.VA:
                        checkBoxVa.Checked = true;
                        break;
                    case Constants.VD:
                        checkBoxVd.Checked = true;
                        break;
                }
            }
        }

        /// <summary>
        /// 启动后台任务
        /// </summary>
        private void StartTask()
        {
            // 监控任务
            Task task1 = new Task(() => { CheckConnect(); });
            task1.Start();
            AppendLog("启动监控任务");
            Log.WriteAppend("启动监控任务");

            // 订阅任务
            Task task2 = new Task(() => { TopicSubscribe(); });
            task2.Start();
            AppendLog("启动订阅任务");
            Log.WriteAppend("启动订阅任务");

            // 采集任务
            Task task3 = new Task(() => { PublishRunData(); });
            task3.Start();
            AppendLog("启动采集任务");
            Log.WriteAppend("启动采集任务");

            // 分频采集
            StartSpecialTask();
        }

        /// <summary>
        /// 执行分频采集
        /// </summary>
        private void StartSpecialTask()
        {
            // 获取配置文件
            PointConf pointConf = new PointConf();
            List<PointConf.PointJson> pointJsonList = pointConf.Load();

            // 没有分频测点
            if (pointJsonList.Count <= 0)
            {
                return;
            }

            // 转换数据结构
            // Dictionary<频率, Dictionary<测点类型, List<测点ID>>>
            Dictionary<int, Dictionary<string, List<string>>> dict = new Dictionary<int, Dictionary<string, List<string>>>();
            foreach (PointConf.PointJson pointJson in pointJsonList)
            {
                string pointType = pointJson.PointType;
                List<PointConf.PointRepeate> pointList = pointJson.PointList;
                if (pointList == null || pointList.Count <= 0)
                {
                    continue;
                }
                foreach (PointConf.PointRepeate pointRepeate in pointJson.PointList)
                {
                    string pointId = pointRepeate.PointId;
                    int repeate = pointRepeate.Repeate;
                    if (!dict.ContainsKey(repeate))
                    {
                        dict.Add(repeate, new Dictionary<string, List<string>>());
                    }
                    _ = dict.TryGetValue(repeate, out Dictionary<string, List<string>> pointTypeDict);
                    if (!pointTypeDict.ContainsKey(pointType))
                    {
                        pointTypeDict.Add(pointType, new List<string>());
                    }
                    pointTypeDict.TryGetValue(pointType, out List<String> pointIdList);
                    pointIdList.Add(pointId);
                }
            }

            // 没有分频测点
            if (dict.Count <= 0)
            {
                return;
            }

            // 按频率启动任务
            foreach (KeyValuePair<int, Dictionary<string, List<string>>> kv in dict)
            {
                // 不可用频率
                int repeate = kv.Key;
                if (repeate <= 0)
                {
                    continue;
                }

                // 没有分频测点
                Dictionary<string, List<string>> pointTypeDict = kv.Value;
                if (pointTypeDict.Count <= 0)
                {
                    continue;
                }

                // 启动采集任务
                Task task = new Task(() => { PublishSpecialPointData(repeate, pointTypeDict); });
                task.Start();
                AppendLog(string.Format("启动分频采集任务。频率:{0}", repeate));
                Log.WriteAppend(string.Format("启动分频采集任务。频率:{0}", repeate));
            }
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
                    PlatformOk();
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
            try
            {
                if (MqttUtils.IsConnected())
                {
                    Console.Write("TopicSubscribe. MQTT IsConnected.");
                    return;
                }

                string clientId = GetClientIdByConf(EdgeConf);
                MqttUtils.Connect(clientId, EdgeConf.AuthUser, EdgeConf.AuthPass);

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

                PlatformOk();
                AppendLog("重新连接平台");
            }
            catch (Exception e)
            {
                PlatformNg();
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
                if (txtRepeate.IsDisposed)
                {
                    return;
                }

                // 上报测点元数据
                if (IsFirstSend)
                {
                    DoSendMetaOnce();
                    IsFirstSend = false;
                }

                // 上报各类型数据
                Task.Run(() =>
                {
                    DoSendDataOnce();
                });

                // 等待下一次上报
                int s = (int)(double.Parse(txtRepeate.Text) * 1000);
                Thread.Sleep(s);
            }
        }

        /// <summary>
        /// 分频发送运行数据
        /// </summary>
        /// <param name="repeate">频率</param>
        /// <param name="pointTypeDict">测点类型字典</param>
        private void PublishSpecialPointData(int repeate, Dictionary<string, List<string>> pointTypeDict)
        {
            while (true)
            {
                // 上报各类型数据
                Task.Run(() =>
                {
                    DoSendSpecialDataOnce(repeate, pointTypeDict);
                });

                // 等待下一次上报
                Thread.Sleep(repeate);
            }
        }

        /// <summary>
        /// 执行元数据上报
        /// </summary>
        private void DoSendMetaOnce()
        {
            try
            {
                // 偏移后的时间
                DateTime offsetTime = DateTime.Now.AddSeconds(Convert.ToDouble(EdgeConf.Offset));

                // 按照数据类型采集数据
                List<ReportData> dataList = new List<ReportData>();
                List<string> pointTypeList = GetPointTypeList();
                foreach (string pointType in pointTypeList)
                {
                    ReportData reportData = GetReportData(pointType, out bool isError, out bool noData, null, null, offsetTime);
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

                DeviceMeta meta = new DeviceMeta()
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
                // 偏移后的时间
                DateTime offsetTime = DateTime.Now.AddSeconds(Convert.ToDouble(EdgeConf.Offset));

                // 按照数据类型采集数据
                List<string> pointTypeList = GetPointTypeList();
                foreach (string pointType in pointTypeList)
                {
                    ReportData reportData = GetReportData(pointType, out bool isError, out bool noData, null, null, offsetTime);
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
                        AppendLog(string.Format("正常上报{0}类型测点{1}个", pointType, sendInfo.Count));

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
        /// 执行运行数据上报
        /// </summary>
        /// <param name="repeate">采集频率</param>
        /// <param name="pointTypeList">测点类型列表</param>
        private void DoSendSpecialDataOnce(int repeate, Dictionary<string, List<string>> pointTypeList)
        {
            try
            {
                // 偏移后的时间
                DateTime offsetTime = DateTime.Now.AddSeconds(Convert.ToDouble(EdgeConf.Offset));

                // 按照数据类型采集数据
                foreach (KeyValuePair<string, List<string>> kv in pointTypeList)
                {
                    string pointType = kv.Key;
                    List<string> pointIdList = kv.Value;

                    // 过滤分频测点
                    ReportData reportData = GetReportData(pointType, out bool isError, out bool noData, pointIdList, null, offsetTime);
                    if (isError)
                    {
                        Console.WriteLine("DoSendSpecialDataOnce. isError");
                        SendErrorCount += 1;
                        continue;
                    }
                    if (noData)
                    {
                        Console.WriteLine("DoSendSpecialDataOnce. noData");
                        continue;
                    }
                    if (reportData == null)
                    {
                        Console.WriteLine("DoSendSpecialDataOnce. reportData == null");
                        continue;
                    }
                    SendInfo sendInfo = GetSendInfoForData(reportData);
                    Console.WriteLine(string.Format("DoSendSpecialDataOnce. sendInfo: {0}", reportData.ToString()));

                    if (sendInfo.Count > 0)
                    {
                        // 发送数据
                        MqttUtils.Public(MqttTopic.UpData, sendInfo.Data);
                        AppendLog(string.Format("分频上报{0}类型测点{1}个。频率{2}", pointType, sendInfo.Count, repeate));

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
            List<string> pointTypeList = GetPointTypeList();
            EdgeConf.SelectTag = string.Join(",", pointTypeList);
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
            ProcessStartInfo processStartInfo = new ProcessStartInfo()
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
            Task.Run(() =>
            {
                DoSendMetaOnce();
            });

            Thread.Sleep(1000);
            btnUpMeta.Enabled = true;
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
                btnPause.BackgroundImage = Resources.pause;
            }
            else
            {
                IsPause = true;
                btnPause.Text = "打开";
                btnPause.BackgroundImage = Resources.start;
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
        /// 获取测点类型集合
        /// </summary>
        /// <returns></returns>
        private List<string> GetPointTypeList()
        {
            List<string> result = new List<string>();
            if (checkBoxAr.Checked)
            {
                result.Add(Constants.AR);
            }
            if (checkBoxAi.Checked)
            {
                result.Add(Constants.AI);
            }
            if (checkBoxAo.Checked)
            {
                result.Add(Constants.AO);
            }
            if (checkBoxDr.Checked)
            {
                result.Add(Constants.DR);
            }
            if (checkBoxDi.Checked)
            {
                result.Add(Constants.DI);
            }
            if (checkBoxDo.Checked)
            {
                result.Add(Constants.DO);
            }
            if (checkBoxVa.Checked)
            {
                result.Add(Constants.VA);
            }
            if (checkBoxVd.Checked)
            {
                result.Add(Constants.VD);
            }
            return result;
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
        /// <param name="pointFilter">测点过滤</param>
        /// <param name="pointDict">测点名称字典</param>
        /// <param name="offsetTime">偏移后的时间</param>
        /// <returns>ReportData</returns>
        private ReportData GetReportData(string pontType, out bool isError, out bool noData,
            List<string> pointFilter, Dictionary<string, string> pointDict, DateTime offsetTime)
        {
            try
            {
                // 打开COM接口
                Rundb runbdb = RunDbUtils.GetRead();
                object openResult = runbdb.Open();
                if (Convert.ToInt16(openResult) != Constants.OpenOk)
                {
                    ComNg();
                    AppendLog(string.Format("COM接口异常，应答结果：{0}", openResult.ToString()));
                    isError = true;
                    noData = true;
                    return null;
                }

                // 更新COM连接状态
                ComOk();

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
                List<PointData> dataList = Tools.GetPointDataList(data, pontType, pointDict);

                // 过滤数据测点
                List<PointData> dataFilter = FilterPointData(dataList, pointFilter);

                // 生成应答数据
                var reportData = new ReportData
                {
                    Time = offsetTime.ToString(Constants.FormatLongMs),
                    DataType = Utils.GetDataType(pontType),
                    PointType = pontType,
                    Data = dataFilter,
                };
                isError = false;
                noData = false;
                return reportData;
            }
            catch (Exception e)
            {
                ComNg();
                isError = true;
                noData = true;
                AppendLog(string.Format("COM接口打开异常：{0}", e.Message));
                return null;
            }
        }

        /// <summary>
        /// 过滤测点数据
        /// </summary>
        /// <param name="dataList">dataList</param>
        /// <param name="pointFilter">pointFilter</param>
        /// <returns>结果</returns>
        private static List<PointData> FilterPointData(List<PointData> dataList, List<string> pointFilter)
        {
            if (pointFilter == null || pointFilter.Count <= 0)
            {
                return dataList;
            }
            if (dataList == null || dataList.Count <= 0)
            {
                return dataList;
            }

            List<PointData> result = new List<PointData>();
            foreach (PointData pointData in dataList)
            {
                if (pointFilter.Contains(pointData.PointId))
                {
                    result.Add(pointData);
                }
            }
            return result;
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
            try
            {
                if (IsPause)
                {
                    return;
                }
                string log = string.Format("{0}:{1}\n", Utils.TimeNow(), str);
                if (!rtbLogContent.IsDisposed)
                {
                    rtbLogContent.AppendText(log);
                }
            }
            catch (Exception e)
            {
                Log.WriteAppend(e.ToString());
            }
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
        /// 内容合法检查：采集频率
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void RepeateLeave(object sender, EventArgs e)
        {
            bool ok = Utils.IsPositiveNumber(txtRepeate.Text);
            if (!ok)
            {
                Utils.ShowInfoBox("采集频率只允许输入正数");
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
                Utils.ShowInfoBox("客户端ID长度必须在4~50之间");
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
                rtbLogContent.Text = rtbLogContent.Text.Substring(rtbLogContent.Lines[0].Length + 1);
                rtbLogContent.Select(rtbLogContent.Text.Length, 0);
            }
        }

        /// <summary>
        /// 设置连接正常显示
        /// </summary>
        private void PlatformOk()
        {
            lblStatus.Text = "正常";
            lblStatus.ForeColor = Color.Blue;
        }

        /// <summary>
        /// 设置连接异常显示
        /// </summary>
        private void PlatformNg()
        {
            lblStatus.Text = "离线";
            lblStatus.ForeColor = Color.Red;
        }

        /// <summary>
        /// 设置连接正常显示
        /// </summary>
        private void ComOk()
        {
            lblCom.Text = "正常";
            lblCom.ForeColor = Color.Blue;
        }

        /// <summary>
        /// 设置连接异常显示
        /// </summary>
        private void ComNg()
        {
            lblCom.Text = "断开";
            lblCom.ForeColor = Color.Red;
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

        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void EdgeFormClosed(object sender, FormClosedEventArgs e)
        {
            // 强制所有消息中止，退出所有窗体，但是若有委托线程，无法干净退出
            Application.Exit();
            // 最彻底的退出方式、不管什么线程都被强制退出
            Environment.Exit(0);
        }

        /// <summary>
        /// 采集按钮
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void PointBtnClick(object sender, EventArgs e)
        {
            EdgePoint.Show();
        }

        /// <summary>
        /// 选择文件
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void SelectFileButtonClick(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog
            {
                // 设置文件选择窗口标题
                Title = "请选择测点文件",
                // 设置文件选择窗口的初始目录
                // InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                InitialDirectory = "C:\\DView500\\Temp",
                // 设置过滤器
                Filter = "(*.xlsx)|*.xlsx|(*.xls)|*.xls",
                // 设置单选
                Multiselect = false
            };
            // 打开选择框
            if (of.ShowDialog() == DialogResult.OK)
            {
                txtFileDir.Text = of.FileName;
            }
        }

        /// <summary>
        /// 上报名称
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpNameButtonClick(object sender, EventArgs e)
        {
            DialogResult result = Utils.ShowConfirm("上报的测点名称会覆盖平台已有名称，请确认选择的测点文件正确且合法，确认要上报吗？");
            if (result == DialogResult.No)
            {
                return;
            }

            ExcelUtils excelUtils = null;
            try
            {
                // 测点类型列表
                List<string> pointTypeList = GetPointTypeList();

                // 打开测点文件
                excelUtils = new ExcelUtils(txtFileDir.Text);

                // 读取测点文件
                Dictionary<string, Dictionary<string, string>> pointTypeDict = new Dictionary<string, Dictionary<string, string>>();
                foreach (string pointType in pointTypeList)
                {
                    Dictionary<string, string> dict = excelUtils.GetPointDict(pointType);
                    if (dict != null)
                    {
                        pointTypeDict.Add(pointType, dict);
                    }
                }

                // 遍历测点类型
                List<ReportData> dataList = new List<ReportData>();

                // 偏移后的时间
                DateTime offsetTime = DateTime.Now.AddSeconds(Convert.ToDouble(EdgeConf.Offset));

                // 按照数据类型采集数据
                foreach (string pointType in pointTypeList)
                {
                    // 获取测点名称字典
                    pointTypeDict.TryGetValue(pointType, out Dictionary<string, string> dict);

                    // 从COM中读取测点数据
                    ReportData reportData = GetReportData(pointType, out bool isError, out bool noData, null, dict, offsetTime);
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

                // 生成上报数据
                DeviceMeta meta = new DeviceMeta()
                {
                    Username = EdgeConf.Username,
                    DeviceDescribe = txtDeviceDescribe.Text,
                    PointList = dataList
                };
                SendInfo sendInfo = GetSendInfoForMeta(meta);

                // 执行数据上报
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
            catch (Exception ex)
            {
                SendErrorCount += 1;
                AppendLog(string.Format("COM接口异常:{0}", ex.Message));
            }
            finally
            {
                if (excelUtils != null)
                {
                    excelUtils.Close();
                }
                RefreshFlowData();
            }
        }
    }
}
