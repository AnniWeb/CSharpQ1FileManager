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
            var commandParams = args.Length > 1 ? new ArraySegment<string>(args, 1, args.Length -1).ToArray() : args;
            
            var commands = new List<ICommand>();
            commands.Add(new LsCommand());
            commands.Add(new CopyCommand());
            commands.Add(new RmCommand());
            commands.Add(new InfoCommand());
            
            try
            { 
                var fileManager = new ConsoleView();
                fileManager.Explore();

                // foreach (var command in commands)
                // {
                //     if (command.GetCommand().ToLowerInvariant() != args[0].ToLowerInvariant()) 
                //     {
                //         continue;
                //     }
                //     
                //     command.Exec(commandParams);
                //     
                // }
                // При выходе должно сохраняться, последнее состояние
                // При успешном выполнение предыдущих пунктов – реализовать движение по истории команд (стрелочки вверх, вниз)
            }
            catch (Exception e)
            {
                // При успешном выполнение предыдущих пунктов – реализовать сохранение ошибки в
                // текстовом файле в каталоге errors/random_name_exception.txt
                Console.WriteLine(e);
                Console.ReadKey();
                // throw;
            }
        }
    }
}