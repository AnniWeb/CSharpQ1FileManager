using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic;
using Q1FileManager.Command;
using Q1FileManager.Config;

namespace Q1FileManager.View
{
    public class ConsoleView : IView
    {
        // private Dictionary<string, IPanel> _panels;
        private IInfoPanel _infoPanel;
        private ConsoleFilePanel _leftPanel;
        private ConsoleFilePanel _rightPanel;
        private ConsoleButtonPanel _btnPanel;

        /// <summary>
        /// Список цветов для разных ситуаций
        /// </summary>
        
        public enum Color
        {
            // Цвета сообщений
            MSG_ERROR = ConsoleColor.Red,
            MSG_WARNING = ConsoleColor.Yellow,
            MSG_INFO = ConsoleColor.White,
            MSG_SUCCESS = ConsoleColor.Green,
            
            // Панели
            PANEL_I_FONT = ConsoleColor.White,
            PANEL_I_BG = ConsoleColor.Black,
            PANEL_F_FONT = ConsoleColor.White,
            PANEL_F_BG = ConsoleColor.Black,
            
            FRAME_BG = ConsoleColor.Black,
            FRAME_FONT = ConsoleColor.Gray,
            
            // Кнопки
            BTN_BG = ConsoleColor.Black,
            BTN_FONT = ConsoleColor.White,
            BTN_ACTIVE_BG = ConsoleColor.White,
            BTN_ACTIVE_FONT = ConsoleColor.Black,
            
            // Файловая система
            PATH_ACTIVE_SELECTED_BG = ConsoleColor.Gray,
            PATH_SELECTED_BG = ConsoleColor.DarkGray,
            PATH_UNSELECTED_BG = ConsoleColor.Black,
            FILE_FONT = ConsoleColor.DarkCyan,
            DIR_FONT = ConsoleColor.DarkGreen,
            
        }

        public ConsoleView()
        {
            Console.CursorVisible = false;
            Console.SetWindowSize(120, 41);
            Console.SetBufferSize(120, 41);

            _infoPanel = new ConsoleInfoPanel(120, 10, 0, 0);
            _leftPanel = new ConsoleFilePanel(60, 25, 0, 10, "pathLeftPanel");
            _rightPanel = new ConsoleFilePanel(60, 25, 60, 10, "pathRightPanel");
            _btnPanel = new ConsoleButtonPanel(120, 3, 0, 35);

            _leftPanel.Active = true;
            _rightPanel.Active = false;
            
            _infoPanel.Show();
            _leftPanel.Show();
            _rightPanel.Show();
            _btnPanel.Show();
        }

        protected void RefrashCurentInfo()
        {
            var _activePanel = _leftPanel.Active ? _leftPanel : _rightPanel;
            _infoPanel.SetFile(_activePanel.GetCurrentFile().FullName);
        }

        protected ConsoleFilePanel GetActiveFilePanel()
        {
            return _leftPanel.Active ? _leftPanel : _rightPanel;
        }
        
        protected ConsoleFilePanel GetPasiveFilePanel()
        {
            return !_leftPanel.Active ? _leftPanel : _rightPanel;
        }

        protected void ToggleActiveFilePanel()
        {
            _leftPanel.ToggleActive();
            _rightPanel.ToggleActive();
        }

        protected void PrintCommand(string text)
        {
            Console.SetCursorPosition(1, 38);
            Console.Write(text);
        }

        protected void ClearCommand()
        {
            Console.SetCursorPosition(1, 38);
            
            for (int i = 38; i < 40; i++)
            {
                string space = new String(' ', 120);
                Console.SetCursorPosition(1, i);
                Console.Write(space);
            }
        }

        public void Explore()
        {
            bool exit = false;

            while (!exit)
            {
                var _activePanel = GetActiveFilePanel();
                try
                {
                    ClearCommand();
                    ConsoleKeyInfo userKey = Console.ReadKey(true);
                    _btnPanel.SetKey(userKey.Key);

                    switch (userKey.Key)
                    {
                        case ConsoleKey.UpArrow:
                            _activePanel.ScrollUp();
                            RefrashCurentInfo();
                            break;
                        case ConsoleKey.DownArrow:
                            _activePanel.ScrollDown();
                            RefrashCurentInfo();
                            break;
                        case ConsoleKey.Home:
                            _activePanel.ScrollHome();
                            RefrashCurentInfo();
                            break;
                        case ConsoleKey.End:
                            _activePanel.ScrollEnd();
                            RefrashCurentInfo();
                            break;
                        case ConsoleKey.PageUp:
                            _activePanel.ScrollPageUp();
                            RefrashCurentInfo();
                            break;
                        case ConsoleKey.PageDown:
                            _activePanel.ScrollPageDown();
                            RefrashCurentInfo();
                            break;
                        case ConsoleKey.Tab:
                            ToggleActiveFilePanel();
                            RefrashCurentInfo();
                            break;
                        case ConsoleKey.Enter:
                            _activePanel.ExecPath();
                            break;
                        case ConsoleKey.F2:
                            CommandRename();
                            break;
                        case ConsoleKey.F3:
                            _activePanel.RefrashFileList();
                            break;
                        case ConsoleKey.F4:
                            CommandChangePath();
                            break;
                        case ConsoleKey.F5:
                            CommandCopy();
                            break;
                        case ConsoleKey.F6: 
                            CommandMove();
                            break;
                        case ConsoleKey.F7:
                            CommandMakeDir();
                            break;
                        case ConsoleKey.F8:
                            CommandRemove();
                            break;
                        case ConsoleKey.F10:
                            exit = true;
                            break;
                    }
                }
                catch (Exception e)
                {
                    _infoPanel.SetMsg(e.Message, Color.MSG_ERROR);
                    Logger.LogError(e);
                }
            }
        }

        /// <summary>
        /// Переименование файла/папки
        /// </summary>
        protected void CommandRename()
        {
            PrintCommand("Введите новое имя:");
            
            var _activePanel = GetActiveFilePanel();
            var newName = Console.ReadLine();

            var invaludChars = Path.GetInvalidFileNameChars().ToList();
            
            invaludChars.Add(Path.PathSeparator);

            foreach (var invaludChar in invaludChars)
            {
                if (newName.Contains(invaludChar))
                {
                    throw new ArgumentException("Недопустимая операция: Запрещенные символы в имени");
                }
            }

            var activeFile = _activePanel.GetCurrentFile();
            
            var newPath = Path.Combine(Path.GetDirectoryName(activeFile.FullName), newName);
            FileSystem.Rename(activeFile.FullName, newPath);
            
            _activePanel.RefrashFileList();
        }

        /// <summary>
        /// Создание категории
        /// </summary>
        protected void CommandMakeDir()
        {
            PrintCommand("Введите имя каталога:");
            
            var _activePanel = GetActiveFilePanel();
            var command = new MkDirCommand();
            var newDir = Console.ReadLine();
            
            command.Exec(Path.Combine(_activePanel.GetCurrentDir().FullName, newDir));
            _activePanel.RefrashFileList();
        }
        
        /// <summary>
        /// Переход по новому пути, в том числе смена диска
        /// </summary>
        protected void CommandChangePath()
        {
            PrintCommand("Введите путь:");
            
            var _activePanel = GetActiveFilePanel();
            var newPath = Console.ReadLine();
            
            _activePanel.ChangePath(newPath);
        }

        /// <summary>
        /// Копирование файлов и папок
        /// </summary>
        protected void CommandCopy()
        {
            var defaultPath = GetPasiveFilePanel().GetCurrentDir().FullName;
            PrintCommand($"Введите полный путь для копирования (по умл. {defaultPath}):");
            var _activePanel = GetActiveFilePanel();
            var newPath = Console.ReadLine();

            var command = new CopyCommand();
            newPath = newPath == String.Empty || newPath == null ? defaultPath : newPath;
            command.Exec(_activePanel.GetCurrentFile().FullName, newPath, AcceptMessage);

            _leftPanel.RefrashFileList();
            _rightPanel.RefrashFileList();
        }
        
        protected void CommandMove()
        {
            var defaultPath = GetPasiveFilePanel().GetCurrentDir().FullName;
            PrintCommand($"Введите полный путь для перемещения (по умл. {defaultPath}):");
            var _activePanel = GetActiveFilePanel();
            var newPath = Console.ReadLine();

            var command = new MoveCommand();
            newPath = newPath == String.Empty || newPath == null
                ? GetPasiveFilePanel().GetCurrentDir().FullName
                : newPath;
            command.Exec(_activePanel.GetCurrentFile().FullName, newPath, AcceptMessage);
            
            _leftPanel.RefrashFileList();
            _rightPanel.RefrashFileList();
        }

        protected void CommandRemove()
        {
            var _activePanel = GetActiveFilePanel();
            var command = new RmCommand();
            command.Exec(_activePanel.GetCurrentFile().FullName, AcceptMessage);
            _activePanel.RefrashFileList();
        }

        public bool AcceptMessage(string text, string answer)
        {
            PrintCommand(text);
            return Console.ReadLine().ToUpper() == answer.ToUpper();
        }
        
    }
}