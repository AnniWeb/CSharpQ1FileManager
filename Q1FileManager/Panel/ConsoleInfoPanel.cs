using System;
using System.IO;

namespace Q1FileManager.View
{
    public class ConsoleInfoPanel : AConsolePanel, IInfoPanel
    {
        private TypeContent _typeContent = TypeContent.MESSAGE;
        private string _msg = String.Empty;
        private MessageType _typeMsg = MessageType.INFO;
        private FileSystemInfo _file;
        
        public ConsoleInfoPanel(int width, int height, int left, int top)
        {
            initFrame(width, height, left,  top);
        }
        public void Show()
        {
            this.Clear();
            PsCon.PsCon.PrintFrameDoubleLine(_left, _top, _widthPanel, _heightPanel, 
                ConsoleColor.White, ConsoleColor.Black);
            
            Console.SetCursorPosition(_left + 1, _top + 1);
            switch (_typeContent)
            {
                case TypeContent.FILE_INFO:
                    Console.ForegroundColor = (ConsoleColor) MessageType.INFO;
                    if (_file == null)
                    {
                        SetMsg("Неизвестный файл/папка");
                        break;
                    }
                    if ((_file.Attributes & FileAttributes.Directory) != 0)
                    {
                        PrintRow($"Каталог: {_file.FullName}");
                        PrintRow($"Дата создания: {_file.CreationTime.Date.ToString("dd.MM.yyyy hh:mm:ss")}");
                        PrintRow($"Дата изменения: {_file.LastWriteTime.Date.ToString("dd.MM.yyyy hh:mm:ss")}");
                    }
                    else
                    {
                        PrintRow($"Файл: {_file.FullName}");
                        PrintRow($"Дата создания: {_file.CreationTime.Date.ToString("dd.MM.yyyy hh:mm:ss")}");
                        PrintRow($"Дата изменения: {_file.LastWriteTime.Date.ToString("dd.MM.yyyy hh:mm:ss")}");
                        var fileInfo = new FileInfo(_file.FullName);
                        PrintRow($"Размер: {FormatFileSize(fileInfo.Length)}");
                    }
                    break;
                case TypeContent.MESSAGE:
                    Console.ForegroundColor = (ConsoleColor) _typeMsg;
                    PrintRow(_msg);
                    break;
            }
            
            ResetColors();
        }
        
        public void Clear()
        {
            for (int i = 0; i < _heightPanel; i++)
            {
                string space = new String(' ', _widthPanel);
                Console.SetCursorPosition(_left, _top + i);
                Console.Write(space);
            }
        }

        public void SetFile(string path)
        {
            _typeContent = TypeContent.FILE_INFO;
            if (Directory.Exists(path))
            {
                _file = new DirectoryInfo(path);
            } 
            else if (File.Exists(path))
            {
                _file = new FileInfo(path);
            }
            Show();
        }

        public void SetMsg(string msg, MessageType type = MessageType.INFO)
        {
            _typeContent = TypeContent.MESSAGE;
            _msg = msg;
            Show();
        }

        public string FormatFileSize(long fileSize)
        {
            var formatFileSize = (double) fileSize;
            long check = 1024 * 1024 * 1024 ;

            if (fileSize > check)
            {
                return $"{formatFileSize / check:f2} ГБ";
            }
            
            check /= 1024;
            if (fileSize > check)
            {
                return $"{formatFileSize / check:f2} МБ";
            }
            
            check /= 1024;
            if (fileSize > check)
            {
                return $"{formatFileSize / check:f2} КБ";
            }
            
            return $"{fileSize.ToString()} Б";
        }
    }
}