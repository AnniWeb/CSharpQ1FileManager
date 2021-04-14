using System;
using System.IO;
using System.Collections.Generic;

namespace Q1FileManager.Command
{
    public class CopyCommand : ICommand
    {
        public string GetCommand()
        {
            return "copy";
        }
        
        public delegate bool ForceCopyDelegate (string text, string answer);

        /// <summary>
        /// Поддержка копирование файлов, каталогов
        /// </summary>
        /// <param name="commandParams"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Exec(string[] commandParams)
        {
            throw new System.NotImplementedException();
        }

        public void Exec(string sourcePath, string targetPath, ForceCopyDelegate force)
        {
            if (sourcePath == targetPath)
            {
                throw new ArgumentException($"Недопустимая операция: Нельзя копировать файл/папку самого в себя");
            }

            var sourceFile = Path.GetFileName(sourcePath);
            
            if (sourceFile == null || Directory.Exists(sourcePath))
            {
                CopyDir(sourcePath, targetPath, force);
            }
            else
            {
                CopyFile(sourcePath, targetPath, force);
            }

        }
        
        /// <summary>
        /// Копирование файла
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="targetPath"></param>
        /// <param name="force"></param>
        /// <exception cref="ArgumentException"></exception>
        protected void CopyFile (string sourcePath, string targetPath, ForceCopyDelegate force) {
        
            var targetDir = Path.GetDirectoryName(targetPath);
            var targetFile = Path.GetFileName(targetPath);
            
            if (!File.Exists(sourcePath))
            {
                throw new ArgumentException($"Недопустимая операция: Файл не существует {sourcePath}");
            }

            if (targetFile != null && !Directory.Exists(targetPath))
            {
                if (File.Exists(targetPath))
                {
                    if (!force("Файл уже существует, перезаписать его (Y/N): ", "Y"))
                    {
                        throw new ArgumentException($"Недопустимая операция: Файл уже существует {targetPath}");
                    }
                }

                if (!Directory.Exists(targetDir))
                {
                    Directory.CreateDirectory(targetDir);
                }
                
                File.Copy(sourcePath, targetPath);
            }
            else
            {
                if (!Directory.Exists(targetPath))
                {
                    Directory.CreateDirectory(targetPath);
                }
                
                File.Copy(sourcePath, Path.Join(targetPath, Path.GetFileName(sourcePath)));
            }
        }
        
        protected void CopyDir (string sourcePath, string targetPath, ForceCopyDelegate force) {
        
            var targetDir = Path.GetDirectoryName(targetPath);
            var targetFile = Path.GetFileName(targetPath);
            
            if (!Directory.Exists(sourcePath))
            {
                throw new ArgumentException($"Недопустимая операция: Папка не существует {sourcePath}");
            }
            
            
            if (targetFile != null && File.Exists(targetPath))
            {
                throw new ArgumentException($"Недопустимая операция: Нельзя скопировать папку {sourcePath} в файл {targetPath}");
            }
            
            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }
            
            var tree = new Queue<string>();
            tree.Enqueue(sourcePath);
            while (tree.Count > 0)
            {
                var curNode = tree.Dequeue();
                var curNodeNewPath = curNode.Replace(sourcePath, targetPath);

                if (Directory.Exists(curNode))
                {
                    foreach (var path in Directory.GetDirectories(curNode))
                    {
                        var newPath = Path.Combine(curNodeNewPath, Path.GetFileName(Path.GetFileName(path)));
                        tree.Enqueue(path);
                        if (!Directory.Exists(newPath))
                        {
                            Directory.CreateDirectory(newPath);
                        }
                    }
                    foreach (var path in Directory.GetFiles(curNode))
                    {
                        var newPath = Path.Combine(curNodeNewPath, Path.GetFileName(Path.GetFileName(path)));
                        
                        if (File.Exists(targetPath))
                        {
                            if (force($"Файл {newPath} уже существует, перезаписать его (Y/N): ", "Y"))
                            {
                                File.Copy(path, newPath);
                            }
                        }
                        else
                        {
                            File.Copy(path, newPath);
                        }
                    }
                }
            }
        }
    }
    
}