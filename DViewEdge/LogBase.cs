using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DViewEdge
{
    public class LogBase
    {
        public string _Msg;
        public LogLevel _level;
        public LogBase(string Msg, LogLevel level)
        {
            _Msg = Msg;
            _level = level;
        }
    }
    public enum LogLevel
    {
        Information = 0,
        warrning,
        error
    }
}
