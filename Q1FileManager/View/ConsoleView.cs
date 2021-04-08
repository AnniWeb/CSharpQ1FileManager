using System;
using System.Collections.Generic;

namespace Q1FileManager.View
{
    public class ConsoleView : IView
    {
        // private Dictionary<string, IPanel> _panels;
        private IInfoPanel _infoPanel;
        private ConsoleFilePanel _leftPanel;
        private ConsoleFilePanel _rightPanel;


        public ConsoleView()
        {
            Console.CursorVisible = false;
            Console.SetWindowSize(120, 41);
            Console.SetBufferSize(120, 41);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;

            _infoPanel = new ConsoleInfoPanel(120, 10, 0, 0);
            _leftPanel = new ConsoleFilePanel(60, 25, 0, 10);
            _rightPanel = new ConsoleFilePanel(60, 25, 60, 10);

            _leftPanel.Active = true;
            _rightPanel.Active = false;

            _leftPanel.ChangePath("N:\\defa");
            _rightPanel.ChangePath(null);
            _infoPanel.Show();
            _leftPanel.Show();
            _rightPanel.Show();
        }

        protected void RefrashCurentInfo()
        {
            var _activePanel = _leftPanel.Active ? _leftPanel : _rightPanel;
            _infoPanel.SetFile(_activePanel.GetCurrentPath().FullName);
        }

        protected ConsoleFilePanel GetActiveFilePanel()
        {
            return _leftPanel.Active ? _leftPanel : _rightPanel;
        }

        protected void ToggleActiveFilePanel()
        {
            _leftPanel.ToggleActive();
            _rightPanel.ToggleActive();
        }

        public void Explore()
        {
            bool exit = false;

            while (!exit)
            {
                var _activePanel = GetActiveFilePanel();
                try
                {
                    ConsoleKeyInfo userKey = Console.ReadKey(true);

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
                        case ConsoleKey.F10:
                            exit = true;
                            break;
                    }
                }
                catch (Exception e)
                {
                    _infoPanel.SetMsg(e.Message, AConsolePanel.MessageType.ERROR);
                    throw e;
                }
            }
        }
    }
}