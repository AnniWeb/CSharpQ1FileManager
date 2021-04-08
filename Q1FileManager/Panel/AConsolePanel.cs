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
        
        public enum MessageType
        {
            ERROR = ConsoleColor.Red,
            WARNING = ConsoleColor.Yellow,
            INFO = ConsoleColor.Blue,
            SUCCESS = ConsoleColor.Green,
        }
        
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
                ConsoleColor.White, ConsoleColor.Black);
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
        /// Установка цветов по умл.
        /// </summary>
        public void ResetColors()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
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
                Console.Write(text.Substring(i, text.Length - i > blockLength ? blockLength : text.Length - i));
                Console.SetCursorPosition(_left + 1, Console.CursorTop + 1);
            }
        }
    }
}