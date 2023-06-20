using FMDMOLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static DViewEdge.RunDbUtils;

namespace DViewEdge
{
    public partial class EdgePoint : Form
    {
        /// <summary>
        /// 全局变量
        /// </summary>
        private static readonly Dictionary<string, DataGridView> DataGridDict = new();
        private PointConf PointConf { get; }
        private Conf EdgeConf { get; }
        private RunDbUtils RunDbUtils { get; }

        /// <summary>
        /// 分频采集窗体初始化
        /// </summary>
        /// <param name="edgeConf">配置文件</param>
        /// <param name="runDbUtils">COM组件</param>
        public EdgePoint(Conf edgeConf, RunDbUtils runDbUtils)
        {
            InitializeComponent();

            // 创建表格控件
            AddDataGrid(Constants.AR, dataGridAR);
            AddDataGrid(Constants.AI, dataGridAI);
            AddDataGrid(Constants.AO, dataGridAO);
            AddDataGrid(Constants.DR, dataGridDR);
            AddDataGrid(Constants.DO, dataGridDO);
            AddDataGrid(Constants.DI, dataGridDI);
            AddDataGrid(Constants.VA, dataGridVA);
            AddDataGrid(Constants.VD, dataGridVD);

            // 初始化全局变量
            EdgeConf = edgeConf;
            RunDbUtils = runDbUtils;
            PointConf = new PointConf();
        }

        /// <summary>
        /// 创建表格控件
        /// </summary>
        /// <param name="pointType">测点类型</param>
        /// <param name="dataGrid">控件对象</param>
        private static void AddDataGrid(string pointType, DataGridView dataGrid)
        {
            if (!DataGridDict.ContainsKey(pointType))
            {
                DataGridDict.Add(pointType, dataGrid);
            }
        }

        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e"></param>e
        private void SaveButtonClick(object sender, EventArgs e)
        {
            List<PointConf.PointJson> pointJson = GetPointJson();
            PointConf.Save(pointJson);
            BindDataGrid();
        }

        /// <summary>
        /// 绑定表格数据
        /// </summary>
        private void BindDataGrid()
        {
            // 读取分频配置
            List<PointConf.PointJson> pointConf = PointConf.Load();

            // 遍历测点类型
            foreach (KeyValuePair<string, DataGridView> kv in DataGridDict)
            {
                string pointType = kv.Key;
                DataGridView dataGrid = kv.Value;

                // 读取测点数据
                ReportData reportData = GetReportData(pointType);
                if (reportData == null || reportData.Data == null)
                {
                    continue;
                }
                if (reportData.Data.Count <= 0)
                {
                    continue;
                }

                // 读取配置信息
                List<PointConf.PointRepeate> pointRepeateList = GetPointRepeateList(pointConf, pointType);

                // 生成表格数据
                dataGrid.Rows.Clear();
                foreach (PointData pointData in reportData.Data)
                {
                    PointConf.PointRepeate pointRepeate = GetOnePointRepeate(pointRepeateList, pointData.PointId);
                    double repeate = Convert.ToDouble(EdgeConf.Repeate) * 1000;

                    int index = dataGrid.Rows.Add();
                    dataGrid.Rows[index].Cells[0].Value = pointData.PointId;
                    bool isSpecial = false;
                    if (pointRepeate == null || pointRepeate.Repeate == repeate)
                    {
                        dataGrid.Rows[index].Cells[1].Value = repeate;
                    }
                    else
                    {
                        dataGrid.Rows[index].Cells[1].Value = pointRepeate.Repeate;
                        isSpecial = true;
                    }

                    if (isSpecial)
                    {
                        dataGrid.Rows[index].DefaultCellStyle.ForeColor = Color.Red;
                    }
                }
            }
        }

        /// <summary>
        /// 根据测点类型获取配置信息
        /// </summary>
        /// <param name="pointConf">pointConf</param>
        /// <param name="pointType">pointType</param>
        /// <returns></returns>
        private static List<PointConf.PointRepeate> GetPointRepeateList(List<PointConf.PointJson> pointConf, string pointType)
        {
            if (pointConf == null)
            {
                return null;
            }
            foreach (PointConf.PointJson point in pointConf)
            {
                if (pointType.Equals(point.PointType))
                {
                    return point.PointList;
                }
            }
            return null;
        }

        /// <summary>
        /// 根据测点ID获取配置信息
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="pointId">pointId</param>
        /// <returns></returns>
        private static PointConf.PointRepeate GetOnePointRepeate(List<PointConf.PointRepeate> list, string pointId)
        {
            if (list == null)
            {
                return null;
            }
            foreach (PointConf.PointRepeate pointRepeate in list)
            {
                if (pointId.Equals(pointRepeate.PointId))
                {
                    return pointRepeate;
                }
            }
            return null;
        }

        /// <summary>
        /// 读取指定类型测点运行数据
        /// </summary>
        /// <param name="pontType">测点类型</param>
        /// <param name="isError">是否异常</param>
        /// <param name="noData">是否有数据</param>
        /// <returns>ReportData</returns>
        private ReportData GetReportData(string pontType)
        {
            try
            {
                // 打开COM接口
                Rundb runbdb = RunDbUtils.GetRead();
                object openResult = runbdb.Open();
                if (Convert.ToInt16(openResult) != Constants.OpenOk)
                {
                    return null;
                }

                // 读取测点数据
                var data = runbdb.ReadFilterVarValues(pontType, "*");
                runbdb.Close();
                if (data == null)
                {
                    return null;
                }

                // 转换数据结构
                List<PointData> dataList = Tools.GetPointDataList(data, pontType);

                // 生成应答数据
                var reportData = new ReportData
                {
                    Time = DateTime.Now.ToString(Constants.FormatLongMs),
                    DataType = Utils.GetDataType(pontType),
                    PointType = pontType,
                    Data = dataList,
                };
                return reportData;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        /// <summary>
        /// 生成配置JSON
        /// </summary>
        /// <returns></returns>
        private List<PointConf.PointJson> GetPointJson()
        {
            List<PointConf.PointJson> pointConf = new();
            double repeateGlobal = Convert.ToDouble(EdgeConf.Repeate) * 1000;

            string[] pointTypeList = EdgeConf.SelectTag.Split(",");
            foreach (string pointType in pointTypeList)
            {
                DataGridDict.TryGetValue(pointType, out DataGridView dataGrid);
                if (dataGrid == null)
                {
                    continue;
                }

                List<PointConf.PointRepeate> list = new();
                int count = dataGrid.Rows.Count;
                for (int i = 0; i < count; i++)
                {
                    string pointId = Convert.ToString(dataGrid.Rows[i].Cells[0].Value);
                    double repeate = Convert.ToDouble(dataGrid.Rows[i].Cells[1].Value);
                    if (repeateGlobal == repeate)
                    {
                        continue;
                    }
                    if (repeate <= 0)
                    {
                        continue;
                    }
                    PointConf.PointRepeate pointRepeate = new()
                    {
                        PointId = pointId,
                        Repeate = (int)repeate
                    };
                    list.Add(pointRepeate);
                }
                if (list.Count > 0)
                {
                    PointConf.PointJson pointAR = new()
                    {
                        PointType = pointType,
                        PointList = list
                    };
                    pointConf.Add(pointAR);
                }
            }
            return pointConf;
        }

        /// <summary>
        /// 窗体关闭
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void EdgePoint_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        /// <summary>
        /// 窗体激活
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EdgePoint_Activated(object sender, EventArgs e)
        {
            BindDataGrid();
        }
    }
}
