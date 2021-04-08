using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Q1FileManager.Command;

namespace Q1FileManager.View
{
    public class ConsoleFilePanel : AConsolePanel, IFilePanel
    {
        private bool _active = false;

        public bool Active
        {
            get => _active;
            set
            {
                _activeFile = 0;
                _active = value;
            }
        }
        
        private string _root;
        private List<FileSystemInfo> _fileList;
        private int _activeFile = 0;
        private int _pageCount = 0;

        public ConsoleFilePanel(int width, int height, int left, int top)
        {
            initFrame(width, height, left,  top);
            _fileList = new List<FileSystemInfo>();
            _pageCount = height - 3;
        }
        
        public void Show()
        {
            Console.BackgroundColor = (ConsoleColor) ConsoleView.Color.PANEL_F_BG;
            Console.ForegroundColor = (ConsoleColor) ConsoleView.Color.PANEL_F_FONT;
            
            Clear();
            ShowFrame();
            
            Console.BackgroundColor = (ConsoleColor) ConsoleView.Color.PANEL_F_BG;
            Console.ForegroundColor = (ConsoleColor) ConsoleView.Color.PANEL_F_FONT;

            var offset = _activeFile > 0 ? (_activeFile / _pageCount) * _pageCount : 0;
            var count = 0;
            var page = _fileList.GetRange(offset, offset + _pageCount > _fileList.Count ? _fileList.Count - offset : _pageCount);
            
            foreach (var path in page)
            {
                Console.SetCursorPosition(_left + 1, _top + count + 1);
                
                Console.ForegroundColor = (ConsoleColor) ((path.Attributes & FileAttributes.Directory) != 0
                ? ConsoleView.Color.DIR_FONT : ConsoleView.Color.FILE_FONT);
                Console.BackgroundColor = (ConsoleColor) (offset + count == _activeFile 
                    ? ConsoleView.Color.PATH_SELECTED_BG : ConsoleView.Color.PATH_UNSELECTED_BG);
                
                int currentCursorTopPosition = Console.CursorTop;
                int currentCursorLeftPosition = Console.CursorLeft;
                
                PrintRow(path.FullName == _root ? ".." : $"{path.Name}");
                    
                
                Console.SetCursorPosition(currentCursorLeftPosition + _widthPanel / 2, currentCursorTopPosition);
                
                count++;
                if (count > _pageCount)
                {
                    break;
                }
            }
        }

        public FileSystemInfo GetCurrentPath()
        {
            return _fileList[_activeFile];
        }

        public void ChangePath(string path)
        {
            if (path != null && path == _root)
            {
                path = new DirectoryInfo(_root).Parent?.FullName ?? _root;
            }
            _fileList = new LsCommand().Exec(path);
            _root =  _fileList.First().FullName;

            if (new DirectoryInfo(_root).Parent == null)
            {
                _fileList.RemoveAt(0);
            }
            _activeFile = 0;
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
                }
            }
        }
    }
}