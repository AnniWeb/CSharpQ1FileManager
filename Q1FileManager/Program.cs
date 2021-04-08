using System;
using System.Collections.Generic;
using Q1FileManager.Command;
using Q1FileManager.View;

namespace Q1FileManager
{
    
    class Program
    {
        static void Main(string[] args)
        {
            try
            { 
                var fileManager = new ConsoleView();
                fileManager.Explore();
                
                // При выходе должно сохраняться, последнее состояние
                // При успешном выполнение предыдущих пунктов – реализовать движение по истории команд (стрелочки вверх, вниз)
            }
            catch (Exception e)
            {
                // При успешном выполнение предыдущих пунктов – реализовать сохранение ошибки в
                // текстовом файле в каталоге errors/random_name_exception.txt
                Console.WriteLine(e);
                Console.ReadKey();
            }
        }
    }
}