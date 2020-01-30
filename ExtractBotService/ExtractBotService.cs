using System;
using System.IO;
using System.ServiceProcess;
using System.Text.Json;

namespace ExtractBotService
{
    public partial class ExtractBotService : ServiceBase
    {
        public static Watcher _watcher;
        
        public ExtractBotService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Logger.Write("Starting service");

            _watcher = new Watcher(ExtractBotConfig.CurrentConfig);
        }

        protected override void OnStop()
        {
            Logger.Write("Stopping service");
            Logger.Rotate();
        }
    }
}
