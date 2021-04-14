using System;
using System.IO;

namespace Q1FileManager.Command
{
    public class MkDirCommand : ICommand
    {
        public string GetCommand()
        {
            return "mkdir";
        }

        public void Exec(string[] commandParams)
        {
            throw new System.NotImplementedException();
        }

        public void Exec(string path)
        {
            if (Directory.Exists(path) || File.Exists(path))
            {
                throw new ArgumentException($"Недопустимая операция: Папка/файл уже существует {path}");
            }

            Directory.CreateDirectory(path);

        }
    }
}