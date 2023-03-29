using System;

namespace DViewEdge
{
    public static class Log
    {
        public static void Pop(string Msg, LogLevel level)
        {
            string log = string.Format(@"{1:yyyy:MM:dd HH:mm:ss:} {0}: {2}", "[info]", DateTime.Now, Msg);
            LogBase msgEntry = new(log + Environment.NewLine, (LogLevel)(int)level);
            LogView.ToastLog(msgEntry);
        }
    }
}
