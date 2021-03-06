using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MenuSystem
{

    public class ConsoleKeyboardNav //Keyboard navigation for console menu.
    {
        public static string MenuSize(bool canCancel, List<string> options)
        {
            const int startX = 2;
            const int startY = 1;
            const int optionsPerLine = 1;
            const int spacingPerLine = 14;

            int currentSelection = 0;

            ConsoleKey key;

            Console.CursorVisible = false;

            do
            {
                Console.Clear();
                Console.WriteLine("===============> BATTLESHIP <===============", Console.ForegroundColor = ConsoleColor.DarkRed);
                Console.Write("");
                
                for (int i = 0; i < options.Count; i++)
                {
                    Console.SetCursorPosition(startX + (i % optionsPerLine) * spacingPerLine, startY + i / optionsPerLine);
                    
                    if (i == currentSelection)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;

                    }

                    Console.Write(options[i]);

                    Console.ResetColor();
                }
                Console.WriteLine("");
                Console.WriteLine("============================================", Console.ForegroundColor = ConsoleColor.DarkRed);
                if (Menu.ErrorMessage != "")
                {
                    Console.WriteLine(Menu.ErrorMessage);
                    Console.WriteLine("============================================");
                    Menu.ErrorMessage = "";
                }

                key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.LeftArrow:
                    {
                        if (currentSelection % optionsPerLine > 0)
                            currentSelection--;
                        break;
                    }
                    case ConsoleKey.RightArrow:
                    {
                        if (currentSelection % optionsPerLine < optionsPerLine - 1)
                            currentSelection++;
                        break;
                    }
                    case ConsoleKey.UpArrow:
                    {
                        if (currentSelection >= optionsPerLine)
                            currentSelection -= optionsPerLine;
                        break;
                    }
                    case ConsoleKey.DownArrow:
                    {
                        if (currentSelection + optionsPerLine < options.Count)
                            currentSelection += optionsPerLine;
                        break;
                    }
                    case ConsoleKey.Escape:
                    {
                        if (canCancel)
                            return "m";
                        break;
                    }
                }
            } while (key != ConsoleKey.Enter);

            Console.CursorVisible = true;

            string currentString = options[currentSelection].Substring(0, 1);
            
            return currentString;
        }
    }
}