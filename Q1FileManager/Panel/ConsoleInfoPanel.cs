using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Q1FileManager.View
{
    public class ConsoleInfoPanel : AConsolePanel, IInfoPanel
    {
        private TypeContent _typeContent = TypeContent.MESSAGE;
        private string _msg = String.Empty;
        private ConsoleView.Color _typeMsg = ConsoleView.Color.MSG_INFO;
        private FileSystemInfo _file;
        
        public ConsoleInfoPanel(int width, int height, int left, int top)
        {
            initFrame(width, height, left,  top);
            _colorBg = (ConsoleColor) ConsoleView.Color.PANEL_I_BG;
            _colorFont = (ConsoleColor) ConsoleView.Color.PANEL_I_FONT;
        }
        public void Show()
        {
            ResetColors();
            ShowFrame();
            ResetColors();
            
            Console.SetCursorPosition(_left + 1, _top + 1);
            switch (_typeContent)
            {
                case TypeContent.FILE_INFO:
                    Console.ForegroundColor = (ConsoleColor) ConsoleView.Color.MSG_INFO;
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

                    if (_file.Attributes != 0)
                    {
                        var attrs = new List<string>();
                        if ((_file.Attributes & FileAttributes.ReadOnly) != 0)
                        {
                            attrs.Add("только для чтения");
                        }
                        if ((_file.Attributes & FileAttributes.System) != 0)
                        {
                            attrs.Add("системный");
                        }
                        if ((_file.Attributes & FileAttributes.Hidden) != 0)
                        {
                            attrs.Add("скрытый");
                        }
                        if ((_file.Attributes & FileAttributes.Temporary) != 0)
                        {
                            attrs.Add("временный");
                        }

                        if (attrs.Count > 0)
                        {
                            PrintRow($"Атрибуты: {attrs.Aggregate((i, j) => i + ", " + j).ToString()}");
                        }
                    }
                    break;
                case TypeContent.MESSAGE:
                    Console.ForegroundColor = (ConsoleColor) _typeMsg;
                    PrintRow(_msg);
                    break;
            }
            
            FillEmpty(1);
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

        public void SetMsg(string msg, ConsoleView.Color type = ConsoleView.Color.MSG_INFO)
        {
            _typeContent = TypeContent.MESSAGE;
            _typeMsg = type;
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