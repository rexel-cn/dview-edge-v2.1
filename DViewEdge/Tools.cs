using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Globalization;
using static DViewEdge.RunDbUtils;

namespace DViewEdge
{
    public class Tools
    {
        /// <summary>
        /// 转换运行数据结构
        /// </summary>
        /// <param name="listStr">listStr</param>
        /// <param name="pointType">pointType</param>
        /// <param name="pointDict">pointDict</param>
        /// <returns>结果</returns>
        public static List<PointData> GetPointDataList(string listStr, string pointType, Dictionary<string, string> pointDict)
        {
            List<PointData> pointDataList = new List<PointData>();
            if (listStr == "" || listStr == null)
            {
                return pointDataList;
            }

            Regex r = new Regex(@"\(.*?\)");
            int get_cnt = int.Parse(r.Match(listStr).Value.Replace("(", "").Replace(")", ""));
            if (get_cnt == 0)
            {
                return pointDataList;
            }

            int start;
            int end;
            int tmp = 0;
            for (; ;)
            {
                start = listStr.IndexOf("|", tmp);
                string lineData;
                if (pointType == Constants.VT)
                {
                    int cen = listStr.IndexOf(",'", start);
                    end = listStr.IndexOf("'|", cen);
                    lineData = listStr.Substring(start + 1, end - start);
                }
                else
                {
                    int cen = listStr.IndexOf(",", start);
                    end = listStr.IndexOf("|", cen);
                    lineData = listStr.Substring(start + 1, end - start - 1);
                }

                string pointId = lineData.Split(',')[0];
                string pointValue = lineData.Split(',')[1].Replace("'", "");

                if (CheckPointValid(pointId))
                {
                    PointData pointData = MakePointData(pointType, pointId, pointValue, pointDict);
                    pointDataList.Add(pointData);
                }

                if (pointType == Constants.VT)
                {
                    tmp = end;
                    if (tmp + 2 >= listStr.Length)
                    {
                        break;
                    }
                }
                else
                {
                    tmp = end;
                    if (tmp + 1 >= listStr.Length)
                    {
                        break;
                    }
                }
            }
            return pointDataList;
        }

        /// <summary>
        /// 检查测点是否有效
        /// </summary>
        /// <param name="str">str</param>
        /// <returns>结果</returns>
        public static bool CheckPointValid(string str)
        {
            if (Regex.IsMatch(str, @"[\u4e00-\u9fa5]"))
            {
                return false;
            }
            if (str.StartsWith("%"))
            {
                return false;
            }
            if (str.StartsWith("@"))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 生成测点数据对象
        /// </summary>
        /// <param name="pointType">测点类型</param>
        /// <param name="pointId">测点Id</param>
        /// <param name="pointValue">测点值</param>
        /// <param name="pointDict">测点名称字典</param>
        /// <returns>PointData</returns>
        private static PointData MakePointData(
            string pointType, string pointId, string pointValue, Dictionary<string, string> pointDict)
        {
            string pointName = "";
            if (pointDict != null && pointDict.ContainsKey(pointId))
            {
                pointName = pointDict[pointId];
            }

            PointData pointData = new PointData()
            {
                PointId = pointId,
                PointName = pointName,
                Qty = Constants.QtyOk
            };

            if (pointValue == "{0}")
            {
                pointData.Qty = Constants.QtyNg;
                pointData.PointValue = 0;
                return pointData;
            }

            string dataType = Utils.GetDataType(pointType);
            if (Constants.TypeDouble.Equals(dataType))
            {
                pointData.PointValue = Convert.ToDouble(pointValue, CultureInfo.InvariantCulture);
            }
            else if (Constants.TypeInt.Equals(dataType))
            {
                pointData.PointValue = Convert.ToInt32(pointValue, CultureInfo.InvariantCulture);
            }
            else
            {
                pointData.PointValue = pointValue;
            }
            return pointData;
        }
    }
}

