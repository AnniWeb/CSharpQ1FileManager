using System;
using System.Collections.Generic;
using Q1FileManager.Command;
using Q1FileManager.Config;
using Q1FileManager.View;

namespace Q1FileManager
{
    
    class Program
    {
        static void Main(string[] args)
        {
            try
            { 
                Logger.Init();
                var fileManager = new ConsoleView();
                fileManager.Explore();
            }
            catch (Exception e)
            {
                Logger.LogError(e);
                Console.ReadKey();
            }
        }
    }
}