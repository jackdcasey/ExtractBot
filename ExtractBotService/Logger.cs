using System;
using System.IO;

namespace ExtractBotService
{
    public static class Logger
    {
        public static void Write(string data)
        {         
            string line = $"{DateTime.UtcNow.ToString("O")}: {data}";

            using(StreamWriter sw = new StreamWriter(Paths.LogFile, true))
            {
                sw.WriteLine(line);
            }
        }

        public static void Rotate()
        {
            File.Copy(Paths.LogFile, Paths.LogFileOld, true);
            File.Delete(Paths.LogFile);
        }
    }
}
