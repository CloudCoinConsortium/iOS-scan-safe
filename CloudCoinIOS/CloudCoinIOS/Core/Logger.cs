using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;
using CloudCoinIOS;

namespace CloudCoin_SafeScan
{
    public static class Logger
    {
        public enum Level { Warning, Normal, Error, Debug }
        private static string logdir;
        private static FileInfo LogFile;

        public static void Initialize()
        {
            var DI = new DirectoryInfo(logdir);
            if (!DI.Exists)
            {
                DI.Create();
            }


            if (!LogFile.Exists)
            {
                LogFile.Create();
            }
        }

        static readonly TextWriter tw;

        static Logger()
        {
            var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
            logdir = Environment.ExpandEnvironmentVariables(appDelegate.LogDir);
            LogFile = new FileInfo(logdir + "/" + "cloudcoin.log");
            tw = TextWriter.Synchronized(File.AppendText(LogFile.FullName));
        }

        public static void Write(string logMessage, Level level)
        {
            try
            {
                Log(logMessage, tw, level);
            }
            catch (IOException e)
            {
                tw.Close();
            }
        }

        private static readonly object _syncObject = new object();

        public static void Log(string logMessage, TextWriter w, Level level)
        {
            // only one thread can own this lock, so other threads
            // entering this method will wait here until lock is
            // available.
            lock (_syncObject)
            {
                w.WriteLine("{0} {1} {2}", level, DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
                w.WriteLine("  :{0}", logMessage);
                w.WriteLine("-------------------------------");
                // Update the underlying file.
                w.Flush();
            }
        }
    }
}

