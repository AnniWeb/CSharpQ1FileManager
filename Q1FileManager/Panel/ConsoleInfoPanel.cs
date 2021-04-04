using System;
using System.IO;
using PsCon;

namespace Q1FileManager.View
{
    public class ConsoleInfoPanel : AConsolePanel, IPanel
    {
        public ConsoleInfoPanel(int width, int height, int left, int top)
        {
            initFrame(width, height, left,  top);
        }
        public void Show()
        {
            this.Clear();
            PsCon.PsCon.PrintFrameDoubleLine(_left, _top, _widthPanel, _heightPanel, 
                ConsoleColor.White, ConsoleColor.Black);
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
    }
}