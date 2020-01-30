using System;
using System.IO;
using System.IO.Compression;

namespace ExtractBotService
{
    static class Extract
    {
        public static void ExtractFolder(string zipPath)
        {
            string extractDir = Path.Combine(Path.GetDirectoryName(zipPath), Path.GetFileNameWithoutExtension(zipPath));

            Logger.Write($"Trying extraction of {zipPath}");
            
            try
            {
                ZipFile.ExtractToDirectory(zipPath, extractDir);
                Logger.Write("Extraction successful");
            }
            catch (Exception ex)
            {
                Logger.Write($"Extraction failed");
                Logger.Write(ex.Message);
            }        
        }
    }
}
