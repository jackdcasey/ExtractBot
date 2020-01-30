using System;
using System.IO;
namespace ExtractBotService
{
    class Paths
    {
        public static string RootDir
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "ExtractBot");
            }
        }

        public static string ConfigFile
        {
            get
            {
                return Path.Combine(RootDir, "ExtractBotConfig.txt");
            }
        }

        public static string LogFile
        {
            get
            {
                return Path.Combine(RootDir, "ExtractBotLog.txt");
            }
        }

        public static string LogFileOld
        {
            get
            {
                return Path.Combine(RootDir, "ExtractBotLog.txt.old");
            }
        }
    }
}
