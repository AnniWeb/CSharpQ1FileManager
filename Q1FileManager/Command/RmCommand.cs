using System;
using System.IO;

namespace Q1FileManager.Command
{
    public class RmCommand : ICommand
    {
        public string GetCommand()
        {
            return "rm";
        }
        
        public delegate bool ForceRemoveDelegate (string text, string answer);

        /// <summary>
        /// Поддержка удаление файлов, каталогов
        /// </summary>
        /// <param name="commandParams"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Exec(string[] commandParams)
        {
            throw new System.NotImplementedException();
        }

        public void Exec(string path, ForceRemoveDelegate force)
        {
            if (File.Exists(path))
            {
                if (force($"Удалить файл {path} (Y/N): ", "Y"))
                {
                    File.Delete(path);
                }
            } 
            else if (Directory.Exists(path))
            {
                if (force($"Удалить папку {path} (Y/N): ", "Y"))
                {
                    Directory.Delete(path, true);
                }
            }
            else
            {
                throw new ArgumentException($"Недопустимая операция: Файл не найден {path}");
            }
        }
    }
}