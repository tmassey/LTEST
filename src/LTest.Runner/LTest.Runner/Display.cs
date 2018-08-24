using System;
using ColorConsole;

namespace LTest.Runner
{
    public static class Display
    {
        private static ConsoleWriter Writer = new ConsoleWriter();
        public static void Intro()
        {
            ClearScreen(ConsoleColor.Blue,ConsoleColor.White);
            SetCursor(0,0);
            Writer.WriteLine("LTest Runner Version 1.0");
        }

        public static void PrintHelp()
        {

        }

        public static void SetCursor(int row, int column)
        {
            Console.CursorTop = row;
            Console.CursorLeft = column;
        }

        public static void ClearScreen(ConsoleColor backColor, ConsoleColor foreColor)
        {
            Writer.SetBackGroundColor(backColor);
            Writer.SetForeGroundColor(foreColor);
            for (int row=0;row<25; row++)
                for (int column = 0; column < 80; column++)
                    Writer.Write(" ",foreColor,backColor);
            SetCursor(0,0);
        }
    }
}