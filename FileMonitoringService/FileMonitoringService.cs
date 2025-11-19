
/* 
===========================================
        📌 ملخص المشروع (Overview Summary)
===========================================

1) الهدف العام:
   إنشاء Windows Service يقوم بمراقبة مجلد محدد والتعامل تلقائيًا مع أي ملف جديد يظهر داخله.

2) المهام الأساسية للخدمة:
   - مراقبة مجلد معين لاكتشاف الملفات الجديدة.
   - معالجة كل ملف جديد عن طريق:
        a. إعادة تسمية الملف باستخدام GUID (اسم فريد).
        b. نقل الملف إلى مجلد الوجهة.
        c. حذف النسخة الأصلية من المجلد المراقَب.

3) تسجيل الأحداث (Logging):
   - تسجيل كل عملية تتم داخل الخدمة.
   - تسهيل تتبع الأخطاء والعمليات أثناء التشغيل.

4) الإعدادات الديناميكية:
   - استخدام ملف App.config لتحديد:
        a. مسار المجلد المراقَب (SourceFolder).
        b. مسار مجلد الوجهة (DestinationFolder).
        c. مسار ملف اللوج (LogFolder).
   - إمكانية تغيير هذه الإعدادات بدون الحاجة لإعادة بناء المشروع.

===========================================

*/

/// My Code


using System;
using System.Configuration;
using System.IO;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace FileMonitoringService
{
    public partial class FileMonitoringService : ServiceBase
    {
        private FileSystemWatcher watcher;

        // قراءة مسارات الفولدرات من App.config
        private string _SourceFolder = ConfigurationManager.AppSettings["SourceFolder"];
        private string _DestinationFolder = ConfigurationManager.AppSettings["DestinationFolder"];
        private string _LogFolder = ConfigurationManager.AppSettings["LogFolder"];
        private string _CurrentPath { get; set; }



        // دالة لتسجيل أي حدث في ملف اللوج
        private void LogServiceEvent(string Message)
        {
            string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {Message}\n";

            // كتابة الرسالة في ملف اللوج
            File.AppendAllText(_LogFolder, logMessage);

            // لو شغّال من الكونسل يعرض الرسائل كمان
            if (Environment.UserInteractive)
            {
                Console.WriteLine(logMessage);
            }
        }

        public FileMonitoringService()
        {
            InitializeComponent(); // تهيئة مكوّنات الخدمة
        }

        protected override void OnStart(string[] args)
        {
            LogServiceEvent("Services Started...");

            // إنشاء FileSystemWatcher لمراقبة الفولدر
            watcher = new FileSystemWatcher();
            watcher.Path = _SourceFolder;   // الفولدر اللي هيتراقب
            watcher.Filter = "*.txt";       // راقب فقط ملفات TXT
            watcher.EnableRaisingEvents = true; // يبدأ المراقبة

            // حدث عند إنشاء أي ملف جديد
            watcher.Created += OnFileCreated;
        }


        private void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            // حفظ مسار الملف الحالي
            this._CurrentPath = e.FullPath;

            LogServiceEvent($"File Detected {e.FullPath}");

            try
            {
                // تأخير بسيط للتأكد أن الملف مش لسه بيتكتب
                System.Threading.Thread.Sleep(400);

                // جلب امتداد الملف
                string extension = Path.GetExtension(e.FullPath);

                // توليد اسم جديد باستخدام GUID
                string uniqueName = $"{Guid.NewGuid()}{extension}";

                // تكوين المسار الجديد بعد النقل
                string newPath = Path.Combine(_DestinationFolder, uniqueName);

                // نقل (وإعادة تسمية) الملف
                File.Move(e.FullPath, newPath);

                LogServiceEvent($"File Moved: {e.FullPath} -> {newPath}");
            }
            catch (Exception ex)
            {
                // في حالة وجود خطأ يتم تسجيله
                LogServiceEvent($"Error renaming file: {ex.Message}");
            }
        }

        protected override void OnStop()
        {
            watcher.EnableRaisingEvents = false; // ايقاف المراقبة
            // عند إيقاف الخدمة
            LogServiceEvent("Services Stopped...");
        }


        // تشغيل الخدمة من الكونسل بدلًا من Windows Services (مفيد أثناء التطوير)
        public void StartInConsole()
        {
            OnStart(null);

            Console.WriteLine("Press any Key to stop the Service....");
            Console.ReadLine();

            OnStop();

            Console.ReadKey();
        }
    }
}
