using System;
using System.IO;
using System.Linq;


namespace ExtractBotService
{
    public class Watcher
    {
        private readonly ExtractBotConfig _config;

        public Watcher(ExtractBotConfig config)
        {
            _config = config;

            Logger.Write("Starting watch:");
            Logger.Write($"Directory: {_config.DirectoryToWatch}");
            Logger.Write($"Extensions: {String.Join(" ,", _config.FileExtensions)}");


            FileSystemWatcher fsw = new FileSystemWatcher()
            {
                Path = _config.DirectoryToWatch,
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.DirectoryName | NotifyFilters.Size | NotifyFilters.FileName,
                Filter = "*",
                IncludeSubdirectories = true,
            };

            fsw.Created += new FileSystemEventHandler(ChangeHandler);
            fsw.Changed += new FileSystemEventHandler(ChangeHandler);
            fsw.Renamed += new RenamedEventHandler(ChangeHandler);

            fsw.EnableRaisingEvents = true;
        }

        private void ChangeHandler(Object source, FileSystemEventArgs e)
        {
            if (_config.FileExtensions.Contains(Path.GetExtension(e.FullPath), StringComparer.OrdinalIgnoreCase))
            {
                Extract.ExtractFolder(e.FullPath);
            }
        }
    }
}
