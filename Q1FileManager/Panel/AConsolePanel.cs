using System;
using System.IO;
using PsCon;

namespace Q1FileManager.View
{
    public class AConsolePanel
    {
        protected int _heightPanel;
        protected int _widthPanel;
        
        protected int _top;
        public int Top { get =>this._top; }

        protected int _left;
        public int Left { get =>this._left; }
        
        public enum TypeContent
        {
            FILE_INFO,
            MESSAGE
        }

        protected ConsoleColor _colorBg; 
        protected ConsoleColor _colorFont; 
        
        /// <summary>
        /// Формирует размеры рамки панели
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <exception cref="ArgumentException"></exception>
        protected void initFrame (int width, int height, int left, int top)
        {
            if (width > Console.WindowWidth || height > Console.WindowHeight)
            {
                throw new ArgumentException($"Панель размером {width}*{height} выходит за пределы экрана");
            }
            if (width <= 0 || height <= 0)
            {
                throw new ArgumentException($"Недопустимые размеры панели {width}*{height}");
            }

            _heightPanel = height;
            _widthPanel = width;
            
            if (left < 0 || top < 0)
            {
                throw new ArgumentException($"Недопустимые координыты панели {width}*{height}");
            }
            if (top > Console.WindowHeight - _heightPanel || left > Console.WindowWidth - _heightPanel)
            {
                throw new ArgumentException($"Недопустимые координыты панели {width}*{height} выходящие за пределы экрана");
            }

            _left = left;
            _top = top;
        }

        /// <summary>
        /// Рисует рамку панели
        /// </summary>
        protected void ShowFrame()
        {
            PsCon.PsCon.PrintFrameDoubleLine(_left, _top, _widthPanel, _heightPanel, 
                (ConsoleColor) ConsoleView.Color.FRAME_FONT, (ConsoleColor) ConsoleView.Color.FRAME_BG
                );
        }
        
        /// <summary>
        /// Очсищение панели
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < _heightPanel; i++)
            {
                string space = new String(' ', _widthPanel);
                Console.SetCursorPosition(_left, _top + i);
                Console.Write(space);
            }
        }
        
        /// <summary>
        /// Печать строки с переносами
        /// </summary>
        /// <param name="text"></param>
        protected void PrintRow(string text)
        {
            int blockLength = _widthPanel - 2;
            for (int i = 0; i < text.Length; i += blockLength )
            {
                var textLength = text.Length - i > blockLength ? blockLength : text.Length - i;
                var space = textLength == blockLength ? String.Empty : new String(' ', blockLength - textLength);
                Console.Write(text.Substring(i, textLength) + space);
                Console.SetCursorPosition(_left + 1, Console.CursorTop + 1);
            }
        }
        
        protected void ResetColors()
        {
            Console.BackgroundColor = _colorBg;
            Console.ForegroundColor = _colorFont;
        }

        protected void FillEmpty(int frameWidth)
        {
            ResetColors();
            for (int i = Console.CursorTop; i < _top + _heightPanel - frameWidth; i++)
            {
                string space = new String(' ', _widthPanel - frameWidth * 2);
                Console.SetCursorPosition(_left + frameWidth, i);
                Console.Write(space);
            }
        }
    }
}