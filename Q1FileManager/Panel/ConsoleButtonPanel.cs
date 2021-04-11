using System;
using System.Collections.Generic;
using System.Text;

namespace Q1FileManager.View
{
    public class ConsoleButtonPanel : AConsolePanel, IPanel
    {
        private Dictionary<ConsoleKey, string> _menu = new Dictionary<ConsoleKey, string>();
        private ConsoleKey? _activeKey;

        public ConsoleButtonPanel(int width, int height, int left, int top)
        {
            initFrame(width, height, left,  top);
            _menu.Add(ConsoleKey.F2, "Переимен.");
            _menu.Add(ConsoleKey.F3, "Обновить");
            _menu.Add(ConsoleKey.F4, "Перейти");
            _menu.Add(ConsoleKey.F5, "Копир.");
            _menu.Add(ConsoleKey.F6, "Перемест.");
            _menu.Add(ConsoleKey.F7, "Новая дир.");
            _menu.Add(ConsoleKey.F8, "Удалить");
            _menu.Add(ConsoleKey.F10, "Выход");
        }

        public void SetKey(ConsoleKey key)
        {
            if (_menu.ContainsKey(key))
            {
                _activeKey = key;
            }
            else
            {
                _activeKey = null;
            }
            Show();
        }
        
        public void Show()
        {
            var ceilWidth = _widthPanel / _menu.Count;
            var i = 0;

            foreach (var btn in _menu)
            {
                var btnText = new StringBuilder($"{btn.Key:F} {btn.Value}");
                var emptySpace = ceilWidth - btnText.Length - 2;
                btnText.Insert(0, emptySpace > 0 ? new string(' ', emptySpace/2) : String.Empty);
                btnText.Append(emptySpace > 0 ? new string(' ', emptySpace/2 + (emptySpace % 2 == 1 ? 1 : 0)) : String.Empty);
                
                PsCon.PsCon.PrintFrameLine(_left + i * ceilWidth, _top, ceilWidth, _heightPanel, (ConsoleColor) ConsoleView.Color.BTN_FONT, (ConsoleColor) ConsoleView.Color.BTN_BG);
                if (_activeKey != null && btn.Key == _activeKey)
                {
                    PsCon.PsCon.PrintString(btnText.ToString(), _left + i * ceilWidth + 1, _top + 1, (ConsoleColor) ConsoleView.Color.BTN_ACTIVE_FONT, (ConsoleColor) ConsoleView.Color.BTN_ACTIVE_BG);
                }
                else
                {
                    PsCon.PsCon.PrintString(btnText.ToString(), _left + i * ceilWidth + 1, _top + 1, (ConsoleColor) ConsoleView.Color.BTN_FONT, (ConsoleColor) ConsoleView.Color.BTN_BG);
                }
                i++;
            }
        }
        
    }
}