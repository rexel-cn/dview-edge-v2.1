using System.Collections.Generic;
using System.Reflection;
using Microsoft.Office.Interop.Excel;

namespace DViewEdge
{
    public class ExcelUtils
    {
        private Application ExcelApp;
        private Workbook WbClass;

        /// <summary>
        /// ExcelUtils
        /// </summary>
        /// <param name="filename">filename</param>
        public ExcelUtils(string filename)
        {
            ExcelApp = new Application();
            object objOpt = Missing.Value;
            WbClass = ExcelApp.Workbooks.Open(
                filename, objOpt, false, objOpt, objOpt, objOpt, true,
                objOpt, objOpt, true, objOpt, objOpt, objOpt, objOpt, objOpt);
        }

        /// <summary>
        /// 关闭Excel
        /// </summary>
        public void Close()
        {
            if (WbClass != null)
            {
                WbClass.Close(false, Missing.Value, false);
            }
            if (ExcelApp != null)
            {
                ExcelApp.Quit();
                ExcelApp = null;
            }
        }

        public Dictionary<string, string> GetPointDict(string pointType)
        {
            Worksheet ws = GetWorksheetByName(pointType);
            if (ws == null)
            {
                return null;
            }

            int columns = ws.UsedRange.Columns.Count;
            int rows = ws.UsedRange.Rows.Count;
            int idColumnIndex = 0;
            int nameColumnIndex = 0;
            for (int i = 0; i < columns; i++)
            {
                Range range = (Range)ws.Cells[1, i + 1];
                if (range.Text.Contains("变量名称"))
                {
                    idColumnIndex = i + 1;
                }
                if (range.Text.Contains("变量描述"))
                {
                    nameColumnIndex = i + 1;
                }
                if (idColumnIndex > 0 && nameColumnIndex > 0)
                {
                    break;
                }
            }

            Dictionary<string, string> dict = new Dictionary<string, string>();
            for (int i = 1; i < rows; i++)
            {
                int row = i + 1;
                Range idRange = (Range)ws.Cells[row, idColumnIndex];
                if (idRange == null || idRange.Text == null)
                {
                    continue;
                }
                if (!Tools.CheckPointValid(idRange.Text))
                {
                    continue;
                }

                Range nameRange = (Range)ws.Cells[row, nameColumnIndex];
                if (nameRange == null || nameRange.Text == null)
                {
                    continue;
                }

                dict.Add(idRange.Text, nameRange.Text);
            }
            return dict;
        }

        private Worksheet GetWorksheetByName(string name)
        {
            if (WbClass == null)
            {
                return null;
            }
            Sheets sheets = WbClass.Worksheets;
            foreach (Worksheet s in sheets)
            {
                if (s.Name == name)
                {
                    return s;
                }
            }
            return null;
        }
    }
}
