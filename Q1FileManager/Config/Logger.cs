using System;
using System.IO;

namespace Q1FileManager.Config
{
    public class Logger
    {
        private static string _dir;

        public static void Init()
        {
            if (_dir == null)
            {
                _dir = Path.Combine(Directory.GetCurrentDirectory(), "logs");
                if (!Directory.Exists(_dir))
                {
                    Directory.CreateDirectory(_dir);
                }
            }
        }

        public static void LogError (Exception error)
        {
            var now = DateTime.Now;
            var logFile = Path.Join(_dir, $"errors_{now.Year}-{now.Month}-{now.Day}.log");
            if (!File.Exists(logFile))
            {
                File.Create(logFile).Dispose();
            }
            File.AppendAllText(logFile, 
                $"{now.Hour}:{now.Minute}:{now.Second}" + " " 
                + error.Message + Environment.NewLine 
            );
        }
    }
}