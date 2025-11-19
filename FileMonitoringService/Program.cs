using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace FileMonitoringService
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new FileMonitoringService()
            };
            ServiceBase.Run(ServicesToRun);
        }


        // Console الجزء دا يستخدم لتشغيل الخدمة في 
        // مناسب للتطوير والاختبار
        // بدون تثبيت
        // Windows Service

        /*static void Main()
        {
            if (Environment.UserInteractive)
            {
                // Running in console mode
                Console.WriteLine("Running in console mode...");
                FileMonitoringService service = new FileMonitoringService();
                service.StartInConsole();
            }
            else
            {
                // Running as a Windows Service
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
            new FileMonitoringService()
                };
                ServiceBase.Run(ServicesToRun);
            }
        }*/




    }
}
