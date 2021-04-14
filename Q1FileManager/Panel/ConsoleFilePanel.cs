using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Q1FileManager.Command;
using Q1FileManager.Config;

namespace Q1FileManager.View
{
    public class ConsoleFilePanel : AConsolePanel, IFilePanel
    {
        private string _configNamePath;
        private bool _active = false;

        public bool Active
        {
            get => _active;
            set
            {
                // _activeFile = 0;
                _active = value;
                Show();
            }
        }
        
        private string _root;
        private List<FileSystemInfo> _fileList;
        private int _activeFile = 0;
        private int _pageCount = 0;

        public ConsoleFilePanel(int width, int height, int left, int top, string configNamePath = null)
        {
            initFrame(width, height, left,  top);
            _fileList = new List<FileSystemInfo>();
            _pageCount = height - 3;
            
            _colorBg = (ConsoleColor) ConsoleView.Color.PANEL_F_BG;
            _colorFont = (ConsoleColor) ConsoleView.Color.PANEL_F_FONT;
            
            if (configNamePath != null)
            {
                _configNamePath = configNamePath;
                _root = GetCorrectRoot(ManagerConfig.ReadOrCreateSetting(_configNamePath, () => Directory.GetCurrentDirectory()));
                _fileList = new LsCommand().Exec(_root);
            }
            else
            {
                _root = Directory.GetCurrentDirectory();
                _fileList = new LsCommand().Exec(_root);
            }
        }
        
        public void Show()
        {
            ResetColors();
            ShowFrame();
            ResetColors();

            var offset = _activeFile > 0 ? (_activeFile / _pageCount) * _pageCount : 0;
            var count = 0;
            var page = _fileList.GetRange(offset, offset + _pageCount > _fileList.Count ? _fileList.Count - offset : _pageCount);
            
            int blockLength = _widthPanel - 2;
            
            foreach (var path in page)
            {
                Console.SetCursorPosition(_left + 1, _top + count + 1);
                
                Console.ForegroundColor = (ConsoleColor) ((path.Attributes & FileAttributes.Directory) != 0
                ? ConsoleView.Color.DIR_FONT : ConsoleView.Color.FILE_FONT);
                Console.BackgroundColor = (ConsoleColor) (offset + count == _activeFile 
                    ? (
                        Active 
                            ? ConsoleView.Color.PATH_ACTIVE_SELECTED_BG 
                            : ConsoleView.Color.PATH_SELECTED_BG
                    ) 
                    : ConsoleView.Color.PATH_UNSELECTED_BG);
                
                var pathString = path.FullName == _root ? ".." : $"{path.Name}";
                var textLength = pathString.Length > blockLength ? blockLength : pathString.Length;
                
                PrintRow(pathString.Length > blockLength ? pathString.Substring(0, textLength - 3) + "..." : pathString);
                
                count++;
                if (count > _pageCount)
                {
                    break;
                }
            }

            FillEmpty(1);
        }

        public FileSystemInfo GetCurrentFile()
        {
            var curFile = _fileList[_activeFile];
            return curFile;
        }
        
        public FileSystemInfo GetCurrentDir()
        {
            return new DirectoryInfo(_root);
        }

        protected string GetCorrectRoot(string path)
        {
            if (!Directory.Exists(path))
            {
                var newRoot = _root;
                while (newRoot != null)
                {
                    newRoot = Path.GetPathRoot(newRoot);
                    if (Directory.Exists(newRoot))
                    {
                        return newRoot;
                    }
                }

                return Directory.GetCurrentDirectory();
            }

            return path;
        }

        public void RefrashFileList()
        {
            if (!Directory.Exists(_root))
            {
                var newRoot = _root;
                while (newRoot != null)
                {
                    newRoot = Path.GetPathRoot(newRoot);
                    if (Directory.Exists(newRoot))
                    {
                        ChangePath(newRoot);
                        return;
                    }
                }
                throw new ApplicationException("Ошибка в определении пути: перезапустите программу");
            }
            _fileList = new LsCommand().Exec(_root);
            if (_fileList.Count - 1 < _activeFile)
            {
                _activeFile = 0;
            }
            Show();
        }

        public void ChangePath(string path)
        {
            if (path != null && !Directory.Exists(path))
            {
                throw new ArgumentException($"Не доступный путь: {path}");
            }
            
            if (path != null && path == _root)
            {
                path = new DirectoryInfo(_root).Parent?.FullName ?? _root;
            }
            _fileList = new LsCommand().Exec(path);
            _root =  _fileList.First().FullName;
            ManagerConfig.AddUpdateAppSettings(_configNamePath, _root);

            if (new DirectoryInfo(_root).Parent == null)
            {
                _fileList.RemoveAt(0);
            }
            _activeFile = 0;
            if (_active)
            {
                Directory.SetCurrentDirectory(path);
            }
            Show();
        }

        public void ScrollUp()
        {
            _activeFile--;
            _activeFile = _activeFile > 0 ? _activeFile : 0;
            Show();
        }

        public void ScrollDown()
        {
            _activeFile++;
            _activeFile = _activeFile < _fileList.Count ? _activeFile : (_fileList.Count > 0 ? _fileList.Count - 1 : 0);
            Show();
        }

        public void ScrollHome()
        {
            _activeFile = 0;
            Show();
        }

        public void ScrollEnd()
        {
            _activeFile = _fileList.Count - 1;
            Show();
        }

        public void ScrollPageUp()
        {
            var curPage = (_activeFile / _pageCount) - 1;
            
            _activeFile = curPage >= 0 ? curPage * _pageCount : 0;
            Show();
        }

        public void ScrollPageDown()
        {
            var curPage = (_activeFile / _pageCount) + 1;
            
            _activeFile = curPage * _pageCount >= _fileList.Count ? _fileList.Count - 1 : curPage * _pageCount;
            Show();
        }

        public void ToggleActive()
        {
            Active = !_active;
            Show();
        }
        
        public void ExecPath()
        {
            var curElement = _fileList[_activeFile];
            if ((curElement.Attributes & FileAttributes.Directory) != 0)
            {
                ChangePath(curElement.FullName);
            }
            else
            {
                try
                {
                    var proc = new Process();
                    proc.StartInfo.FileName = @$"{curElement.FullName}";
                    proc.Start();
                }
                catch (Exception e)
                {
                    Logger.LogError(e);
                }
            }
        }
    }
}