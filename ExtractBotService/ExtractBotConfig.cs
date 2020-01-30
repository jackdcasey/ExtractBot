using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExtractBotService
{
    public class ExtractBotConfig
    {
        public string DirectoryToWatch { get; set; }
        public string[] FileExtensions { get; set; }

        public static ExtractBotConfig CurrentConfig
        {
            get
            {
                string rawConfigData = File.ReadAllText(Paths.ConfigFile).Replace("\\", "\\\\");
                return JsonSerializer.Deserialize<ExtractBotConfig>(rawConfigData);
            }
        }

        public static void InitializeConfig()
        {            
            ExtractBotConfig initialConfig = new ExtractBotConfig
            {
                DirectoryToWatch = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                FileExtensions = new[] { ".zip" }
            };

            Directory.CreateDirectory(Paths.RootDir);

            string rawConfigData = JsonSerializer.Serialize(initialConfig, new JsonSerializerOptions { WriteIndented = true }).Replace("\\\\", "\\");
            File.WriteAllText(Paths.ConfigFile, rawConfigData);

            Logger.Write("Initialized configuration");
        }
    }
}
