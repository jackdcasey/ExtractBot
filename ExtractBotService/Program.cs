﻿using System;
using System.Collections;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.IO;
using System.Text.Json;

namespace ExtractBotService
{
    static class Program
    {     
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                ServiceBase[] ServicesToRun;

                ServicesToRun = new ServiceBase[]
                {
                    new ExtractBotService()
                };

                ServiceBase.Run(ServicesToRun);
            }

            else if (args.Length == 1)
            {
                switch (args[0])
                {
                    case "-install":
                        ExtractBotConfig.InitializeConfig();
                        InstallService();
                        StartService();
                        break;
                    case "-uninstall":
                        StopService();
                        UninstallService();
                        DeleteConfig();
                        break;
                    case "-restart":
                        StopService();
                        StartService();
                        break;
                    case "-reinstall":
                        ExtractBotConfig.InitializeConfig();
                        StopService();
                        UninstallService();
                        InstallService();
                        StartService();
                        break;
                    case "-start":
                        StartService();
                        break;
                    case "-stop":
                        StopService();
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        private static void DeleteConfig()
        {
            Directory.Delete(Paths.RootDir, true);
        }
        
        private static bool IsInstalled()
        {
            return ServiceController.GetServices().Any(s => s.ServiceName == "ExtractBotService");
        }

        private static bool IsRunning()
        {
            if (!IsInstalled()) return false;

            return ServiceController.GetServices().Any(s => s.Status == ServiceControllerStatus.Running);
        }

        private static AssemblyInstaller GetInstaller()
        {
            AssemblyInstaller installer = new AssemblyInstaller(typeof(ExtractBotService).Assembly, null)
            {
                UseNewContext = true
            };
            return installer;
        }

        private static void InstallService()
        {
            if (IsInstalled()) return;

            try
            {
                using (AssemblyInstaller installer = GetInstaller())
                {
                    IDictionary state = new Hashtable();
                    try
                    {
                        installer.Install(state);
                        installer.Commit(state);
                    }
                    catch
                    {
                        try
                        {
                            installer.Rollback(state);
                        }
                        catch { }

                        throw;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private static void UninstallService()
        {
            if (!IsInstalled()) return;

            try
            {
                using (AssemblyInstaller installer = GetInstaller())
                {
                    IDictionary state = new Hashtable();

                    try
                    {
                        installer.Uninstall(state);
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private static void StartService()
        {
            if (!IsInstalled()) return;

            using (ServiceController controller = new ServiceController("ExtractBotService"))
            {
                try
                {
                    if (controller.Status != ServiceControllerStatus.Running)
                    {
                        controller.Start();
                        controller.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(10));
                    }
                }
                catch
                {
                    throw;
                }
            }
        }

        private static void StopService()
        {
            if (!IsInstalled()) return;

            using (ServiceController controller = new ServiceController("ExtractBotService"))
            {
                try
                {
                    if (controller.Status != ServiceControllerStatus.Stopped)
                    {
                        controller.Stop();
                        controller.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(10));
                    }
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}
