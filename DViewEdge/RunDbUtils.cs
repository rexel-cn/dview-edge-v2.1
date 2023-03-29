using FMDMOLib;

namespace DViewEdge
{
    public class RunDbUtils
    {
        private static RunDbUtils Instance = null;
        private Rundb Read { get; set; }
        private Rundb Write { get; set; }

        private RunDbUtils() { }

        public static RunDbUtils GetInstance()
        {
            if (Instance != null)
            {
                return Instance;
            }

            RunDbUtils _instance = new()
            {
                Read = new Rundb(),
                Write = new Rundb(),
            };
            Instance = _instance;
            return _instance;
        }

        public Rundb GetRead()
        {
            return Read;
        }

        public Rundb GetWrite()
        {
            return Write;
        }

    }
}
