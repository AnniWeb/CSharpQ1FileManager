using System;
using System.Collections.Generic;
using System.IO;

namespace Q1FileManager.Command
{
    public class LsCommand : ICommand
    {
        public string GetCommand()
        {
            return "ls";
        }

        /// <summary>
        /// Просмотр файловой структуры
        /// Вывод файловой структуры должен быть постраничным
        /// В конфигурационном файле должна быть настройка вывода количества элементов на страницу
        /// </summary>
        /// <param name="commandParams"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Exec(string[] commandParams)
        {
            var list = Exec(commandParams.Length > 0 ? commandParams[0] : null);

            foreach (var element in list)
            {
                var icon = (element.Attributes & FileAttributes.Directory) != 0 ? "*" : "";
                Console.WriteLine($"{icon}{element.Name}");
            }
        }

        public List<FileSystemInfo> Exec(string path = null)
        {
            path = path == null ? Directory.GetCurrentDirectory() : path;
            var list = new List<FileSystemInfo>();
            
            
            if (!Directory.Exists(path))
            {
                throw new ArgumentException("Путь к директории не найден");
            }
            
            var rootDir = new DirectoryInfo(path);
            // if (rootDir.Parent != null)
            // {
                list.Add(rootDir);
            // }

            foreach (var dir in rootDir.GetDirectories())
            {
                list.Add(dir);
            }
            foreach (var file in rootDir.GetFiles())
            {
                list.Add(file);
            }

            return list;
        }
    }
}