using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public class LogView
    {
        private static LogView _instance = null;
        private static readonly object InitLock = new object();
        private static List<TextBoxBase> _logList = null;

        private LogView()
        {
            _logList = new List<TextBoxBase>();
        }

        private static LogView Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (InitLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new LogView();
                        }
                    }
                }
                return _instance;
            }
        }

        private bool AddLogView(TextBoxBase logBox)
        {
            if (_logList.Contains(logBox))
            {
                return false;
            }
            _logList.Add(logBox);
            return true;
        }

        private bool RemovelogView(TextBoxBase logBox)
        {
            if (_logList.Count == 0 || !_logList.Contains(logBox))
            {
                return false;
            }
            _logList.Remove(logBox);
            return true;
        }

        private bool Toast(LogBase msg)
        {
            if (_logList != null)
            {
                _logList.ForEach(logControl => logControl.Invoke(new Action(() =>
                {
                    if (logControl.Lines.Length > 100)
                    {
                        logControl.Text = @"";
                    }
                    logControl.Text += msg._Msg;
                    logControl.Select(logControl.Text.Length, 0);
                    logControl.ScrollToCaret();
                })));
            }
            return true;
        }

        public static bool ToastLog(LogBase log)
        {
            return Instance.Toast(log);
        }

        public static bool AddLogBox(TextBoxBase logBox)
        {
            return Instance.AddLogView(logBox);
        }

        public static bool RemoveLogBox(TextBoxBase logBox)
        {
            return Instance.RemovelogView(logBox);
        }
    }
}
