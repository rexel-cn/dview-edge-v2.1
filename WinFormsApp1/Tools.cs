using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Globalization;

namespace WinFormsApp1
{
    public class Tools
    {
        public static List<PointData> GetPointDataList(string listStr, string type)
        {
            List<PointData> pointDataList = new();
            if (listStr == "" || listStr == null)
            {
                return pointDataList;
            }

            Regex r = new(@"\(.*?\)");
            int get_cnt = Int32.Parse(r.Match(listStr).Value.Replace("(", "").Replace(")", ""));
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
                if (type == "VT")
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

                string pointId = lineData.Split(",")[0];
                string pointValue = lineData.Split(",")[1].Replace("'", "");

                if (CheckPointValid(pointId))
                {
                    PointData pointData = MakePointData(type, pointId, pointValue);
                    pointDataList.Add(pointData);
                }

                if (type == "VT")
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

        private static bool CheckPointValid(string str)
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

        private static PointData MakePointData(string type, string pointId, string pointValue)
        {
            PointData pointData = new()
            {
                pointId = pointId,
                qty = "0"
            };

            if (pointValue == "{0}")
            {
                pointData.qty = "1";
                pointData.pointValue = 0;
                return pointData;
            }

            if (type == "VA" || type == "AR" || type == "AO" || type == "AI")
            {
                pointData.pointValue = Convert.ToDouble(pointValue, CultureInfo.InvariantCulture);
            }
            else if (type == "VD" || type == "DR" || type == "DO" || type == "DI")
            {
                pointData.pointValue = Convert.ToInt32(pointValue, CultureInfo.InvariantCulture);
            }
            else
            {
                pointData.pointValue = pointValue;
            }
            return pointData;
        }
    }

    public class PointData
    {
        public string pointId { get; set; }
        public object pointValue { get; set; }
        public string qty { get; set; }
    }
}

