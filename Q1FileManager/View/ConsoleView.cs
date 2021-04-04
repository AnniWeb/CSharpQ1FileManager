using System;
using System.Collections.Generic;

namespace Q1FileManager.View
{
    public class ConsoleView : IView
    {
        // private Dictionary<string, IPanel> _panels;
        private IPanel _infoPanel;
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

        public void Explore()
        {
            bool exit = false;
            var _activePanel = _leftPanel.Active ? _leftPanel : _rightPanel;

            while (!exit)
            {
                ConsoleKeyInfo userKey = Console.ReadKey(true);

                switch (userKey.Key)
                {
                    case ConsoleKey.UpArrow:
                        _activePanel.ScrollUp();
                        break;
                    case ConsoleKey.DownArrow:
                        _activePanel.ScrollDown();
                        break;
                    case ConsoleKey.Home:
                        _activePanel.ScrollHome();
                        break;
                    case ConsoleKey.End:
                        _activePanel.ScrollEnd();
                        break;
                    case ConsoleKey.PageUp:
                        _activePanel.ScrollPageUp();
                        break;
                    case ConsoleKey.PageDown:
                        _activePanel.ScrollPageDown();
                        break;
                    case ConsoleKey.Tab:
                        _leftPanel.ToggleActive();
                        _rightPanel.ToggleActive();
                        _activePanel = _leftPanel.Active ? _leftPanel : _rightPanel;
                        break;
                    case ConsoleKey.Enter:
                        _activePanel.ExecPath();
                        break;
                    case ConsoleKey.F10:
                        exit = true;
                        break;
                }
            }
        }
    }
}